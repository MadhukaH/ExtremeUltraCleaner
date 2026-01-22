namespace ExtremeUltraDeepCleaner.Models
{
    /// <summary>
    /// Log message severity levels
    /// </summary>
    public enum LogLevel
    {
        Info,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// Represents a single log entry
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Timestamp when the log was created
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Log message text
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Severity level of the log
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Formatted timestamp for display
        /// </summary>
        public string FormattedTime => Timestamp.ToString("HH:mm:ss");

        /// <summary>
        /// Color for the log level (for UI binding)
        /// </summary>
        public string LevelColor => Level switch
        {
            LogLevel.Success => "#4CAF50",  // Green
            LogLevel.Warning => "#FF9800",  // Orange
            LogLevel.Error => "#F44336",    // Red
            _ => "#FFFFFF"                  // White
        };

        /// <summary>
        /// Icon for the log level
        /// </summary>
        public string LevelIcon => Level switch
        {
            LogLevel.Success => "✓",
            LogLevel.Warning => "⚠",
            LogLevel.Error => "✗",
            _ => "•"
        };
    }
}
