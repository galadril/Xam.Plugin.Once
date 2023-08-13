namespace Xam.Plugin.Once
{
    /// <summary>
    /// The schedule class
    /// </summary>
    public class After
    {
        /// <summary>
        /// Creates a new instance of After with specified time interval and type
        /// </summary>
        /// <param name="runAfter">The number of time units to schedule after.</param>
        /// <param name="type">The type of time unit to run the schedule</param>
        public After(int? runAfter, AfterType type)
        {
            RunAfter = runAfter;
            Type = type;
        }

        /// <summary>
        /// Run after interval counter
        /// </summary>
        public int? RunAfter { get; set; } = null;

        /// <summary>
        /// Runafter type (seconds, minutes, hours for example)
        /// </summary>
        public AfterType Type { get; set; } = AfterType.Days;

        /// <summary>
        /// After type
        /// </summary>
        public enum AfterType
        {
            Days,
            Hours,
            Minutes,
            Seconds,
            Milliseconds
        }
    }
}
