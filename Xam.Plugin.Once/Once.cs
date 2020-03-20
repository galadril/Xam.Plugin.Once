using System;
using System.Threading;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Xam.Plugin.Once
{
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
        public DateTime? LastRunAt(string key)
        {
            return GetPreference(key);
        }

        /// <summary>
        /// Mark a task as completed
        /// </summary>
        public void MarkRunAsDone(string key, DateTime? at = null)
        {
            SetPreference(key, at ?? DateTime.Now);
        }

        /// <summary>
        /// Check if a task needs to be run
        /// </summary>
        public bool NeedsToRun(string key, int? runAfterDays = null)
        {
            var at = GetPreference(key);
            if (at == null)
                return true; // probably just a run once task
            else
                return runAfterDays.HasValue ? ((DateTime.Now - at.Value).TotalDays >= runAfterDays) : false;
        }

        /// <summary>
        /// Check if a task needs to be run
        /// </summary>
        public void RunNeedsTo(string key, Command task, int? runAfterDays = null)
        {
            var at = GetPreference(key);
            if (at == null || (runAfterDays.HasValue && ((DateTime.Now - at.Value).TotalDays >= runAfterDays)))
            {
                task.Execute(key);
                SetPreference(key, DateTime.Now);
            }
        }

        #endregion


        #region Private

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
