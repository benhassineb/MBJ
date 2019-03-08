using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading.Tasks;
using Common.Monitoring.Listeners;
using System.Web;
using System.Globalization;
using Common.Resources;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines an implementation of <see cref="IFrameworkTracer" />
    ///     based on .NET <see cref="System.Diagnostics.TraceSource" />.
    /// </summary>
    /// <remarks>
    ///     Implementation choices:
    ///     - Each trace source type is mapped to a <see cref="System.Diagnostics.TraceSource" />.
    ///     - Correlation id is mapped to the ActivityId of the <see cref="System.Diagnostics.Trace.CorrelationManager" />:
    ///     this does not work with async/await (not used here). On the other side it enables correlation id to be
    ///     automatically
    ///     propagated from Wcf client to server when the following section is used in config files:
    ///     <code>
    ///      <source name="System.ServiceModel" propagateActivity="true">
    ///             <listeners>
    ///                 <add name="donotremove" type="System.Diagnostics.DefaultTraceListener" />
    ///             </listeners>
    ///         </source>
    ///   </code>
    /// </remarks>
    public class DotNetTracer : IFrameworkTracer, ISourceable
    {
        #region Constants

        /// <summary>
        /// The method to store key/value in a persisting storage accross thread execution.
        /// </summary>
        private static readonly Action<string, object> StoreData = (name, data) =>
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Add(name, data);
            else
                CallContext.LogicalSetData(name, data);
        };

        /// <summary>
        /// The method to load key/value from a persisting storage accross thread execution.
        /// </summary>
        private static readonly Func<string, object> LoadData = (name) =>
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Items[name];
            else
                return CallContext.LogicalGetData(name);
        };

        /// <summary>
        ///     The correlation id key used for <see cref="CallContext" />.
        /// </summary>
        private const string CorrelationIdKey = "¤CorrelationId";

        /// <summary>
        ///     The user id key used for <see cref="CallContext" />.
        /// </summary>
        private const string PrincipalIdKey = "¤PrincipalId";

        /// <summary>
        ///     The session id key used for <see cref="CallContext" />.
        /// </summary>
        private const string SessionIdKey = "¤SessionId";

        /// <summary>
        ///     A boolean indicating that exception handlers are already attached.
        /// </summary>
        private static volatile bool _exceptionHandlersAttached;

        /// <summary>
        /// The type of our own trace listener to prevents infinite loop.
        /// </summary>
        private static readonly string CurrentListenerType = typeof(AsyncDatabaseTraceListener).Name;// "Diagnostics.Listeners.AsyncDatabaseTraceListener";

        /// <summary>
        ///     Conversion table between <see cref="TraceCategory" /> and <see cref="System.Diagnostics.TraceEventType" />.
        /// </summary>
        private static readonly IDictionary<TraceCategory, System.Diagnostics.TraceEventType> CategoryMapper
            = new Dictionary<TraceCategory, System.Diagnostics.TraceEventType>
            {
                {TraceCategory.Error, System.Diagnostics.TraceEventType.Error},
                {TraceCategory.Warning, System.Diagnostics.TraceEventType.Warning},
                {TraceCategory.Information, System.Diagnostics.TraceEventType.Information},
                {TraceCategory.Performance, System.Diagnostics.TraceEventType.Information},
                {TraceCategory.Verbose, System.Diagnostics.TraceEventType.Verbose}
            };

        #endregion

        #region Fields

        /// <summary>
        ///     The underlying System.Diagnostics TraceSource.
        /// </summary>
        private TraceSource _traceSource;

        /// <summary>
        ///     The instance name of the trace source type.
        /// </summary>
        private string _traceSourceName;

        /// <summary>
        ///     The Trace source type.
        /// </summary>
        private TraceSourceType _traceSourceType;

        #endregion

        #region Methods

        [SecurityPermission(SecurityAction.Demand)]
        [Localizable(false)]
        private void TraceInternal(TraceCategory traceCategory, int errorCode, TraceEventType traceEventType, string traceEventName, DateTime? traceDate,
            TimeSpan? elapsedTime, Exception exception, string message, TraceContext context)
        {
            if (_traceSource == null) throw new InvalidOperationException("SetSource() must be called before.");
            // if no date were specified use now
            DateTime creationDate = traceDate ?? HighResolutionDateTime.UtcNow;
            // mapping TraceCategory System.Diagnostics.TraceEventType
            System.Diagnostics.TraceEventType dotnetTraceEventType = CategoryMapper[traceCategory];
            TraceEventData traceEvent =
                new TraceEventData
                {
                    CreationDate = creationDate,
                    ContextParameter = context?.ToString(),
                    CorrelationId = GetCorrelationId(),
                    SessionId = GetSessionId(),
                    UserName = GetPrincipalId(),
                    ElapsedTime = elapsedTime,
                    ErrorCode = errorCode,
                    RawException = exception,
                    TraceCategory = traceCategory,
                    TraceEventType = traceEventType,
                    TraceEventName = traceEventName,
                    TraceSourceName = _traceSourceName,
                    TraceSourceType = _traceSourceType,
                    Message = message,
                };
            traceEvent.ComputeAutomaticProperties();
            _traceSource.TraceData(dotnetTraceEventType, errorCode, traceEvent);
        }

        /// <summary>
        ///     Gets the most inner exception to get meaningfull information.
        /// </summary>
        /// <param name="exception">The exception to parse.</param>
        /// <param name="maxDepth">The max depth of the recursion to prevent infinite loop.</param>
        /// <returns>
        ///     The most inner managed <see cref="BaseException" /> or the most inner exception
        ///     associated to the specified exception .
        /// </returns>
        private static Exception GetInnerException(Exception exception, int maxDepth)
        {
            BaseException managedException = exception as BaseException;
            while (exception?.InnerException != null && maxDepth >= 0)
            {
                exception = exception.InnerException;
                // update managedException only if a deeper is found
                var baseException = exception as BaseException;
                if (baseException != null)
                    managedException = baseException;
                maxDepth = --maxDepth;
            }
            // we want the deepest managed exception if any, otherwise the deepest exception.
            return managedException ?? exception;
        }

        #endregion

        #region IFrameworkTracer Members

        /// <summary>
        ///     Sets the exception handling for unhandled exceptions.
        /// </summary>
        /// <param name="includeFirstExceptions">Include first exceptions.</param>
        public void SetExceptionHandling(bool includeFirstExceptions)
        {
            if (_traceSource == null) throw new InvalidOperationException("SetSource() must be called before.");
            if (_exceptionHandlersAttached) return;
            if (includeFirstExceptions)
            {
                // if specified log first chance exception
                AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
                {
                    // do not log exceptions of our own TraceListener to avoid nasty infinite loop
                    // and do not log our own user-caught exceptions as first-chance exceptions
                    if (Environment.StackTrace.IndexOf(CurrentListenerType, StringComparison.Ordinal) != -1 || e.Exception is BaseException) return;
                    TraceException(e.Exception, TraceEventType.FirstChanceError, ErrorCodes.FirstChanceError);
                };
            }
            // log unhandled exception
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => TraceException(e.ExceptionObject as Exception);

            // log task unobserved exception
            TaskScheduler.UnobservedTaskException += (sender, e) => TraceException(e.Exception, TraceEventType.GlobalLevelCaughtError, ErrorCodes.TaskError);
            _exceptionHandlersAttached = true;
        }


        /// <summary>
        ///     Traces the specified information message with an optional contextual parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <remarks>Log information only for crucial events of your application. Consider using TraceVerbose in other situations.</remarks>
        public void TraceInformation(string message, TraceEventType traceEventType = TraceEventType.Log, string traceEventName = null,
            TraceContext context = null)
        {
            if (string.IsNullOrEmpty(message))
                message = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);

            TraceInternal(TraceCategory.Information, 0, traceEventType, traceEventName, null, null, null, message, context);
        }

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
        /// <remarks>This is required to pass the caught exception if applicable.</remarks>
        public void TraceWarning(string message, int errorCode, Exception exception = null, TraceEventType traceEventType = TraceEventType.Log,
            string traceEventName = null, TraceContext context = null)
        {
            if (string.IsNullOrEmpty(message))
                message = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);

            TraceInternal(TraceCategory.Warning, errorCode, TraceEventType.Log, traceEventName, null, null, exception, message, context);
        }

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
        /// <remarks>This is required to pass the caught exception if applicable.</remarks>
        public void TraceError(string message, int errorCode, Exception exception = null, TraceEventType traceEventType = TraceEventType.Log,
            string traceEventName = null, TraceContext context = null)
        {
            int innerErrorCode = errorCode;
            string innerMessage = message;
            if (exception != null)
            {
                // try to find our own exceptions supposed to be more meaningfull
                Exception innerException = GetInnerException(exception, 10);
                innerMessage += " : " + innerException.Message;
                var managedException = innerException as BaseException;
                if (managedException != null)
                    innerErrorCode = managedException.ErrorCode;
            }
            else
                if (string.IsNullOrEmpty(innerMessage)) innerMessage = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);

            TraceInternal(TraceCategory.Error, innerErrorCode, TraceEventType.Log, traceEventName, null, null, exception, innerMessage, context);
        }

        /// <summary>
        ///     Traces the specified exception.
        /// </summary>
        /// <param name="exception">The specified exception.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="errorCode">The specified code. Optional. By default it is assumed that this is a global level caught exception.</param>
        /// <param name="context">Trace contextual information.</param>
        /// <remarks>This is the preferred method to log exceptions caught at global level.</remarks>
        public void TraceException(Exception exception, TraceEventType traceEventType = TraceEventType.GlobalLevelCaughtError, int errorCode = ErrorCodes.GlobalLevelError, TraceContext context = null)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            var baseException = exception as BaseException;
            int realCode = baseException?.ErrorCode ?? errorCode;
            string message = exception.Message;
            // try to find our own exceptions supposed to be more meaningfull
            Exception innerException = GetInnerException(exception, 10);
            if (innerException != exception)
            {
                message += " : " + innerException.Message;
                var internalBaseException = innerException as BaseException;
                realCode = internalBaseException?.ErrorCode ?? realCode;
            }
            var loadException = innerException as ReflectionTypeLoadException;
            if (loadException != null)
            {
                foreach (var exceptionMessage in loadException.LoaderExceptions.Select(e => e.Message).Distinct())
                {
                    message += "\r\n- " + exceptionMessage;
                }
            }

            TraceContext traceContext = context ?? TraceContext.Create(Constants.TraceParameter.ErrorCode, realCode);
            TraceInternal(TraceCategory.Error, realCode, traceEventType, null, null, null, exception, message, traceContext);
        }

        /// <summary>
        ///     Traces the specified verbose message with an optional contextual parameter.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        public void TraceVerbose(string message, TraceEventType traceEventType = TraceEventType.Log, string traceEventName = null,
            TraceContext context = null)
        {
            if (string.IsNullOrEmpty(message))
                message = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);

            TraceInternal(TraceCategory.Verbose, 0, TraceEventType.Log, traceEventName, null, null, null, message, context);
        }

        /// <summary>
        ///     Traces the specified message and execution time.
        /// </summary>
        /// <param name="message">The optional message, or the caller member if not specified.</param>
        /// <param name="traceEventType">The optional event type.</param>
        /// <param name="traceEventName">The calling context.</param>
        /// <param name="context">The optional contextual trace parameters.</param>
        /// <remarks>
        ///     If performance measure cannot be done in one method, consider using the
        ///     TraceStartPerformance/TraceStopPerformance methods instead.
        /// </remarks>
        /// <code>using (logger.TracePerformance("service call")) {...}</code>
        public IDisposable TracePerformance(string message = null, TraceEventType traceEventType = TraceEventType.CodeBlock,
            [System.Runtime.CompilerServices.CallerMemberName]string traceEventName = null, TraceContext context = null)
        {
            DateTime startTime = HighResolutionDateTime.UtcNow;
            if (string.IsNullOrEmpty(message))
                message = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);
            return
                new PerformanceWatch(
                    elapsedTime =>
                        TraceInternal(TraceCategory.Performance, 0, traceEventType, traceEventName, startTime, elapsedTime, null, message, context));
        }

        /// <summary>
        ///     Marks the start of a performance measure.
        /// </summary>
        /// <code>var startTime = logger.TraceStartPerformance();</code>
        /// <remarks>If performance measure can be done in the same method, consider using the TracePerformanece method instead.</remarks>
        public DateTime TraceStartPerformance()
        {
            return HighResolutionDateTime.UtcNow;
        }

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
        public void TraceStopPerformance(DateTime startTime, string message = null, TraceEventType traceEventType = TraceEventType.CodeBlock,
            string traceEventName = null, TraceContext context = null)
        {
            if (string.IsNullOrEmpty(message))
                message = string.Format(CultureInfo.CurrentCulture, InternalMessages.MethodCall, traceEventName ?? InternalMessages.EventUnknown);

            TraceInternal(TraceCategory.Performance, 0, traceEventType, traceEventName, startTime, HighResolutionDateTime.GetTimeSpan(startTime), null,
                message, context);
        }


        /// <summary>
        ///     Gets the correlation id for the current traces and logs.
        /// </summary>
        /// <returns>The current id to correlate traces and logs.</returns>
        public Guid GetCorrelationId()
        {
            return LoadDataAsGuid(CorrelationIdKey);
        }

        /// <summary>
        ///     Gets the session id for the current traces and logs.
        /// </summary>
        /// <returns>The current id to correlate user's session and logs.</returns>
        public string GetSessionId()
        {
            return LoadDataAsString(SessionIdKey);
        }

        /// <summary>
        ///     Gets the principal id for the current traces and logs.
        /// </summary>
        /// <returns>The current user id to correlate traces and logs.</returns>
        public string GetPrincipalId()
        {
            return LoadDataAsString(PrincipalIdKey);
        }

        /// <summary>
        ///     Sets the correlation id for the next traces and logs.
        /// </summary>
        /// <param name="correlationId">The id to correlate traces and logs.</param>
        public void SetCorrelationId(Guid correlationId)
        {
            StoreData(CorrelationIdKey, correlationId);
        }

        /// <summary>
        ///     Sets the session id for the next traces and logs.
        /// </summary>
        /// <param name="sessionId">The id to correlate user's session and logs.</param>
        public void SetSessionId(string sessionId)
        {
            StoreData(SessionIdKey, sessionId);
        }

        /// <summary>
        ///     Sets the principal id for the next traces and logs.
        /// </summary>
        /// <param name="principalId">The principal id to correlate traces and logs.</param>
        public void SetPrincipalId(string principalId)
        {
            StoreData(PrincipalIdKey, principalId);
        }

        /// <summary>
        ///     Gets the specified Guid from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="keyName">The name of the guid parameter.</param>
        /// <returns>The found guid or an empty guid otherwise.</returns>
        private static Guid LoadDataAsGuid(string keyName)
        {
            var storedObject = LoadData(keyName);
            Guid guid;
            if (storedObject != null && Guid.TryParse(storedObject.ToString(), out guid))
                return guid;
            return Guid.Empty;
        }

        /// <summary>
        ///     Gets the specified string from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="keyName">The name of the string parameter.</param>
        /// <returns>The found string or an empty string otherwise.</returns>
        private static string LoadDataAsString(string keyName)
        {
            var storedObject = LoadData(keyName);
            return storedObject?.ToString() ?? string.Empty;
        }
        #endregion

        #region ISourcable Members

        /// <summary>
        ///     Creates a trace source of the specified trace source type with the specified trace source name.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <param name="traceSourceName">The instance name of the trace source type.</param>
        public void SetSource(TraceSourceType traceSourceType, string traceSourceName)
        {
            if (_traceSource != null) return;
            _traceSourceName = traceSourceName;
            _traceSourceType = traceSourceType;
            _traceSource = new TraceSource(traceSourceType.ToString());
        }

        #endregion
    }
}
