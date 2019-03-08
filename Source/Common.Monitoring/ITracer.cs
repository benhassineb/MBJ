using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Common.Monitoring
{
    /// <summary>
    ///     Describes tracing capabilities for end to end monitoring.
    /// </summary>
    public interface ITracer
    {
        #region Methods

        /// <summary>
        ///     Traces the specified information message with an optional contextual parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <remarks>
        ///     Log information only for crucial events of your application. Consider using TraceVerbose in other situations.
        /// </remarks>
        void TraceInformation(string message, TraceEventType traceEventType = TraceEventType.Log, string traceEventName = null,
            TraceContext context = null);

        /// <summary>
        ///     Traces the specified warning message with the specified code, the optional exception and an optional contextual
        ///     parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="errorCode">The specified error code.</param>
        /// <param name="exception">The optional exception.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <remarks>
        ///     This is required to pass the caught exception if applicable.
        /// </remarks>
        void TraceWarning(string message, int errorCode, Exception exception = null, TraceEventType traceEventType = TraceEventType.Log,
             string traceEventName = null, TraceContext context = null);

        /// <summary>
        ///     Traces the specified error message with the specified code, the optional exception and an optional contextual
        ///     parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="errorCode">The specified error code.</param>
        /// <param name="exception">The optional exception.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <remarks>
        ///     This is required to pass the caught exception if applicable.
        /// </remarks>
        void TraceError(string message, int errorCode, Exception exception = null, TraceEventType traceEventType = TraceEventType.Log,
             string traceEventName = null, TraceContext context = null);

        /// <summary>
        ///     Traces the specified verbose message with an optional contextual parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        void TraceVerbose(string message, TraceEventType traceEventType = TraceEventType.Log,  string traceEventName = null,
            TraceContext context = null);

        /// <summary>
        ///     Traces the specified message and execution time.
        /// </summary>
        /// <param name="message">The optional message, or the caller member if not specified.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <returns></returns>
        /// <remarks>
        ///     If performance measure cannot be done in one method, consider using the TraceStartPerformance/TraceStopPerformance
        ///     methods instead.
        /// </remarks>
        /// <code>using (logger.TracePerformance("service call")) {...}</code>
        IDisposable TracePerformance(string message = null, TraceEventType traceEventType = TraceEventType.CodeBlock,
              [CallerMemberName]string traceEventName = null, TraceContext context = null);

        /// <summary>
        ///     Gets the correlation id for the current traces and logs.
        /// </summary>
        /// <returns>
        ///     The current id to correlate traces and logs.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "By Design")]
        Guid GetCorrelationId();

        /// <summary>
        ///     Gets the session id for the current traces and logs.
        /// </summary>
        /// <returns>
        ///     The current id to correlate user's sessions and logs.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "By Design")]
        string GetSessionId();

        /// <summary>
        ///     Gets the user id for the current traces and logs.
        /// </summary>
        /// <returns>
        ///     The current principal id to correlate traces and logs.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "By Design")]
        string GetPrincipalId();

        #endregion
    }
}
