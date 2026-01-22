namespace ExtremeUltraDeepCleaner.Models
{
    /// <summary>
    /// Summary statistics for a completed cleaning operation
    /// </summary>
    public class CleaningSummary
    {
        /// <summary>
        /// Total number of files deleted
        /// </summary>
        public int TotalFilesDeleted { get; set; }

        /// <summary>
        /// Total bytes freed (in bytes)
        /// </summary>
        public long TotalBytesFreed { get; set; }

        /// <summary>
        /// Time taken to complete all cleaning tasks
        /// </summary>
        public TimeSpan TimeTaken { get; set; }

        /// <summary>
        /// Gets total space freed in GB (formatted)
        /// </summary>
        public string SpaceFreedGB => $"{TotalBytesFreed / (1024.0 * 1024.0 * 1024.0):F2} GB";

        /// <summary>
        /// Gets time taken in formatted string
        /// </summary>
        public string TimeTakenFormatted => TimeTaken.TotalMinutes < 1
            ? $"{TimeTaken.TotalSeconds:F1} seconds"
            : $"{TimeTaken.TotalMinutes:F1} minutes";
    }
}
