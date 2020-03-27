namespace Xam.Plugin.Once
{
    /// <summary>
    /// The schedule class
    /// </summary>
    public class After
    {
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
            Minutes,
            Seconds,
            Milliseconds
        }
    }
}
