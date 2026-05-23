namespace WarrantyBee.Shared.Core.Enums;

/// <summary>
/// Specifies the severity level of a log message within the telemetry system.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Informational messages that track the general flow of the application.
    /// </summary>
    Info,

    /// <summary>
    /// Warning messages indicating a potential issue or abnormal but non-terminating event.
    /// </summary>
    Warn,

    /// <summary>
    /// Error messages indicating a failure in a specific operation or request.
    /// </summary>
    Error,

    /// <summary>
    /// Detailed diagnostic messages used for debugging and fine-grained troubleshooting.
    /// </summary>
    Debug
}
