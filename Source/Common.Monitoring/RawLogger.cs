using System.Diagnostics;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines an internal low-level logger.
    /// </summary>
    /// <remarks>
    ///     - It is used to provide low-level logging to high-level monitoring API.
    ///     - It is based on Windows Event Log API.
    /// </remarks>
    public static class RawLogger
    {
        #region Methods

        /// <summary>
        ///     Log warning to windows event log.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public static void LogWarning(int errorCode, string message)
        {
            EventLog.WriteEntry("Application", message, EventLogEntryType.Warning, errorCode);
        }

        /// <summary>
        ///     Log error to windows event log.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public static void LogError(int errorCode, string message)
        {
            EventLog.WriteEntry("Application", message, EventLogEntryType.Error, errorCode);
        }

        #endregion
    }
}
