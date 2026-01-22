namespace ExtremeUltraDeepCleaner.Models
{
    /// <summary>
    /// Represents a single cleaning task
    /// </summary>
    public class CleaningTask
    {
        /// <summary>
        /// Step number (1-18)
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Display name of the task
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Icon path for the task
        /// </summary>
        public string IconPath { get; set; } = string.Empty;

        /// <summary>
        /// Whether this task requires user confirmation (dangerous operation)
        /// </summary>
        public bool RequiresWarning { get; set; }

        /// <summary>
        /// Description of what this task does
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
