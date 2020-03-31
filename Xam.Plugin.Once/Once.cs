using System;
using System.Threading;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Xam.Plugin.Once
{
    /// <summary>
    /// Just a nice and simple static map based on the Static Google Maps api endpoint for your Xamarin Forms project
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Once
    {
        #region Variables

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static Once _Instance;

        #endregion


        #region Constructor & Destructor

        /// <summary>
        /// Default constructor
        /// </summary>
        private Once()
        { }

        #endregion


        #region Properties

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Once Instance
        {
            get
            {
                if (_Instance == null)
                    _ = Interlocked.CompareExchange(ref _Instance, new Once(), null);
                return _Instance;
            }
        }

        #endregion


        #region Public

        /// <summary>
        /// Get the last completed at date
        /// </summary>
        /// <param name="key">The key of the task</param>
        public DateTime? LastRunAt(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Once need a key to work with", nameof(key));

            return GetPreference(key);
        }

        /// <summary>
        /// Mark a task as completed
        /// </summary>
        /// <param name="key">The key of the task</param>
        /// <param name="at">Set this datetime as the completed date time</param>
        public void MarkRunAsDone(string key, DateTime? at = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Once need a key to work with", nameof(key));

            SetPreference(key, at ?? DateTime.Now);
        }

        /// <summary>
        /// Check if a task needs to be run
        /// </summary>
        /// <param name="key">The key of the task</param>
        /// <param name="runAfter">Set a runafter schedule</param>
        public bool NeedsToRun(string key, After runAfter = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Once need a key to work with", nameof(key));

            var at = GetPreference(key);
            if (at == null)
                return true; // probably just a run once task
            else
                return runAfter != null ? ItsTime(runAfter, at.Value) : false;
        }

        /// <summary>
        /// Check if a task needs to be run
        /// </summary>
        /// <param name="key">The key of the task</param>
        /// <param name="task">The Command to run when the time is right</param>
        /// <param name="autoMarkAsDone">Automaticly mark the task as done</param>
        /// <param name="runAfter">Set a runafter schedule</param>
        public void RunWhen(string key, Command task, bool autoMarkAsDone = false, After runAfter = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Once need a key to work with", nameof(key));
            if (task is null)
                throw new ArgumentNullException(nameof(task));

            var at = GetPreference(key);
            if (at == null || ItsTime(runAfter, at.Value))
            {
                task.Execute(key);
                if(autoMarkAsDone)
                    SetPreference(key, DateTime.Now);
            }
        }

        #endregion


        #region Private

        /// <summary>
        /// Check if its time to run
        /// </summary>
        private bool ItsTime(After runAfter, DateTime lastRun)
        {
            if (runAfter == null)
                return true;
            else
            {
                switch(runAfter.Type)
                {
                    case After.AfterType.Days:
                        var daysPast = (DateTime.Now - lastRun).TotalDays;
                        return daysPast >= runAfter.RunAfter;
                    case After.AfterType.Minutes:
                        var minutesPast = (DateTime.Now - lastRun).TotalMinutes;
                        return minutesPast >= runAfter.RunAfter;
                    case After.AfterType.Seconds:
                        var secondsPast = (DateTime.Now - lastRun).TotalSeconds;
                        return secondsPast >= runAfter.RunAfter;
                    case After.AfterType.Milliseconds:
                        var milliSecondsPast = (DateTime.Now - lastRun).TotalMilliseconds;
                        return milliSecondsPast >= runAfter.RunAfter;
                }
                return false;
            }
        }

        /// <summary>
        /// Set preference
        /// </summary>
        private void SetPreference(string key, DateTime? value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;
            var json = JsonConvert.SerializeObject(value, Formatting.None);
            Preferences.Set(key, json);
        }

        /// <summary>
        /// Get preference
        /// </summary>
        private DateTime? GetPreference(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            var json = Preferences.Get(key, null);
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<DateTime?>(json);
        }

        #endregion
    }
}
