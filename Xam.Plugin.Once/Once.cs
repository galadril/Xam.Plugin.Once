using System;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Xam.Plugin.Once
{
    /// <summary>
    /// A simple Xamarin Forms library to manage one-off operations.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Once
    {
        private static Once _Instance;
        private static readonly object _SyncLock = new object();

        private Once()
        { }

        public static Once Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncLock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Once();
                        }
                    }
                }
                return _Instance;
            }
        }

        public DateTime? LastRunAt(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Once need a key to work with", nameof(key));
            }

            return GetPreference(key);
        }

        public void MarkRunAsDone(string key, DateTime? at = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Once need a key to work with", nameof(key));
            }

            SetPreference(key, at ?? DateTime.Now);
        }

        public bool NeedsToRun(string key, After runAfter = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Once need a key to work with", nameof(key));
            }

            var at = GetPreference(key);
            if (at == null)
            {
                return true;
            }
            else
            {
                return runAfter != null ? ItsTime(runAfter, at.Value) : false;
            }
        }

        public void RunWhen(string key, Command task, bool autoMarkAsDone = false, After runAfter = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Once need a key to work with", nameof(key));
            }
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var at = GetPreference(key);
            if (at == null || ItsTime(runAfter, at.Value))
            {
                task.Execute(key);
                if (autoMarkAsDone)
                {
                    SetPreference(key, DateTime.Now);
                }
            }
        }

        private bool ItsTime(After runAfter, DateTime lastRun)
        {
            if (runAfter == null)
            {
                return true;
            }
            else
            {
                switch (runAfter.Type)
                {
                    case After.AfterType.Days:
                        var daysPast = (DateTime.Now - lastRun).TotalDays;
                        return daysPast >= runAfter.RunAfter;

                    case After.AfterType.Hours:
                        var hoursPast = (DateTime.Now - lastRun).TotalHours;
                        return hoursPast >= runAfter.RunAfter;

                    case After.AfterType.Minutes:
                        var minutesPast = (DateTime.Now - lastRun).TotalMinutes;
                        return minutesPast >= runAfter.RunAfter;

                    case After.AfterType.Seconds:
                        var secondsPast = (DateTime.Now - lastRun).TotalSeconds;
                        return secondsPast >= runAfter.RunAfter;

                    case After.AfterType.Milliseconds:
                        var milliSecondsPast = (DateTime.Now - lastRun).TotalMilliseconds;
                        return milliSecondsPast >= runAfter.RunAfter;

                    default:
                        return false;
                }
            }
        }

        private void SetPreference(string key, DateTime? value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }

            var json = JsonConvert.SerializeObject(value, Formatting.None);
            Preferences.Set(key, json);
        }

        private DateTime? GetPreference(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var json = Preferences.Get(key, null);
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<DateTime?>(json);
        }
    }
}
