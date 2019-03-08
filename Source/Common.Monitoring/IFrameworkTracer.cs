using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common.Resources;

namespace Common.Monitoring
{
    /// <summary>
    ///     Describes logging and tracing capabilities for end to end monitoring.
    /// </summary>
    public interface IFrameworkTracer: ITracer
    {
        #region Methods

        /// <summary>
        ///     Sets the exception handling for unhandled exceptions.
        /// </summary>
        /// <param name="includeFirstExceptions">Include first exceptions.</param>
        void SetExceptionHandling(bool includeFirstExceptions);

        /// <summary>
        ///     Traces the specified exception.
        /// </summary>
        /// <param name="exception">The specified exception.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="errorCode">The specified code. Optional. By default it is assumed that this is a global level caught exception.</param>
        /// <param name="context">Trace contextual information.</param>
        /// <remarks>This is the preferred method to log exceptions caught at global level.</remarks>
        void TraceException(Exception exception, TraceEventType traceEventType = TraceEventType.GlobalLevelCaughtError, int errorCode = ErrorCodes.GlobalLevelError, TraceContext context = null);

        /// <summary>
        ///     Marks the start of a performance measure.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     If performance measure can be done in the same method, consider using the TracePerformanece method instead.
        /// </remarks>
        /// <code>var startTime = logger.TraceStartPerformance();</code>
        DateTime TraceStartPerformance();

        /// <summary>
        ///     Traces the specified message and execution time with the specified contextual parameter(s) based on the specified
        ///     start time.
        /// </summary>
        /// <param name="startTime">The start time of the time measure.</param>
        /// <param name="message">The optional message, or the caller member if not specified.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <code>logger.TraceStopPerformance(startTime, ...);</code>
        [Localizable(false)]
        void TraceStopPerformance(DateTime startTime, string message = null, TraceEventType traceEventType = TraceEventType.CodeBlock,
              [CallerMemberName]string traceEventName = null, TraceContext context = null);

        /// <summary>
        ///     Sets the correlation id for the next traces and logs.
        /// </summary>
        /// <param name="correlationId">The id to correlate traces and logs.</param>
        void SetCorrelationId(Guid correlationId);

        /// <summary>
        ///     Sets the session id for the next traces and logs.
        /// </summary>
        /// <param name="sessionId">The id to correlate user's sessions and logs.</param>
        void SetSessionId(string sessionId);

        /// <summary>
        ///     Sets the principal id for the next traces and logs.
        /// </summary>
        /// <param name="principalId">The principal id to correlate traces and logs.</param>
        void SetPrincipalId(string principalId);

        #endregion
    }
}
