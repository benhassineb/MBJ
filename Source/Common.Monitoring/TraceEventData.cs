using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines the trace event class.
    /// </summary>
    internal class TraceEventData
    {
        #region Constants

        private static readonly string CurrentComputerName = Environment.MachineName;
        private static readonly string CurrentProcessName = Process.GetCurrentProcess().ProcessName;
        private static readonly int CurrentProcessId = Process.GetCurrentProcess().Id;

        #endregion

        #region Properties

        /// <summary>
        ///     Trace event source type.
        /// </summary>
        internal TraceSourceType TraceSourceType { get; set; }

        /// <summary>
        ///     Trace event source name.
        /// </summary>
        internal string TraceSourceName { get; set; }

        /// <summary>
        ///     Type of trace event.
        /// </summary>
        internal TraceEventType TraceEventType { get; set; }

        /// <summary>
        ///     Name of the instance of the trace event.
        /// </summary>
        internal string TraceEventName { get; set; }

        /// <summary>
        ///     Category of trace event.
        /// </summary>
        internal TraceCategory TraceCategory { get; set; }

        /// <summary>
        ///     Execution time for Performance event trace.
        /// </summary>
        internal TimeSpan? ElapsedTime { get; set; }

        /// <summary>
        ///     Error code in case of Error/Warning event trace.
        /// </summary>
        internal int? ErrorCode { get; set; }

        /// <summary>
        ///     Exception in case of Error/Warning event trace.
        /// </summary>
        internal Exception RawException { get; set; }

        /// <summary>
        ///     Exception in case of Error/Warning event trace.
        /// </summary>
        internal string Exception { get; set; }

        /// <summary>
        ///     Message associated to the event trace.
        /// </summary>
        internal string Message { get; set; }

        /// <summary>
        ///     Context parameters.
        /// </summary>
        internal string ContextParameter { get; set; }

        /// <summary>
        ///     Correlation id between trace eventsList.
        /// </summary>
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        internal Guid CorrelationId { get; set; }

        /// <summary>
        ///     Creation date of the trace.
        /// </summary>
        internal DateTime CreationDate { get; set; }

        /// <summary>
        ///     The name of the process that is generating the trace.
        /// </summary>
        internal string ProcessName { get; set; }

        /// <summary>
        ///     The unique id of the process that is generating the trace.
        /// </summary>
        internal int ProcessId { get; set; }

        /// <summary>
        ///     The name of the thread that is generating the trace.
        /// </summary>
        internal string ThreadName { get; set; }

        /// <summary>
        ///     The manage id of the thread that is generating the trace.
        /// </summary>
        internal int ThreadId { get; set; }

        /// <summary>
        ///     The Id of the session that is generating the trace.
        /// </summary>
        internal string SessionId { get; set; }

        /// <summary>
        ///     The name of the computer that is generating the trace.
        /// </summary>
        internal string MachineName { get; set; }

        /// <summary>
        ///     The name of the computer that is generating the trace.
        /// </summary>
        internal string UserName { get; set; }

        /// <summary>
        ///     The type of the exception if any.
        /// </summary>
        internal string ExceptionType { get; set; }

        /// <summary>
        ///     The stack trace of the exception if any.
        /// </summary>
        internal string StackTrace { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Compute automatic properties.
        /// </summary>
        internal void ComputeAutomaticProperties()
        {
            ProcessId = CurrentProcessId;
            ProcessName = CurrentProcessName;
            MachineName = CurrentComputerName;
            var thread = Thread.CurrentThread;
            ThreadId = thread.ManagedThreadId;
            ThreadName = thread.Name;
            if (RawException == null) return;
            Exception = RawException.ToString();
            if (RawException.InnerException != null)
                Exception += Environment.NewLine + Environment.NewLine + RawException.InnerException;
            StackTrace = (new StackTrace(4, true)).ToString(); // seems more accurate than RawException.StackTrace;
            ExceptionType = RawException.GetType().Name;
        }

        /// <summary>
        ///     String serialization of <see cref="TraceEventData" />.
        /// </summary>
        /// <returns>A string representing the <see cref="TraceEventData" /> instace.</returns>
        /// <remarks>
        ///     To help visualization in basic TraceListener like the DefaultTraceListener (Visual Studio, DebugViewer).
        /// </remarks>
        public override string ToString()
        {
            if (TraceCategory == TraceCategory.Performance)
                return string.Format(CultureInfo.CurrentCulture, "{0}-{1}|{2}-{3}|{4}|{5}|{6}|{7}ms",
                    TraceSourceType.ToString(), TraceSourceName,
                    TraceEventType.ToString(), TraceEventName,
                    TraceCategory,
                    CreationDate,
                    Message,
                    ElapsedTime);
            if (TraceCategory == TraceCategory.Error || TraceCategory == TraceCategory.Warning && RawException != null)
                return string.Format(CultureInfo.CurrentCulture, "{0}-{1}|{2}-{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                    TraceSourceType.ToString(), TraceSourceName,
                    TraceEventType.ToString(), TraceEventName,
                    TraceCategory,
                    CreationDate,
                    Message,
                    ExceptionType,
                    RawException,
                    StackTrace);
            return string.Format(CultureInfo.CurrentCulture, "{0}-{1}|{2}-{3}|{4}|{5}|{6}",
                TraceSourceType.ToString(), TraceSourceName,
                TraceEventType.ToString(), TraceEventName,
                TraceCategory,
                CreationDate,
                Message);
        }

        #endregion
    }
}
