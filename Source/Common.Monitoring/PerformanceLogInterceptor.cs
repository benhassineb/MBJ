using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Common.Resources;

namespace Common.Monitoring
{
    /// <summary>
    ///     WCF extension that modifies inbound and outbound application messages to provide performance measure and
    ///     correlation propagation.
    /// </summary>
    /// <remarks>
    ///     - This extension is meaned to be used client and server side.
    /// </remarks>
    [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
    public class PerformanceLogInterceptor : IDispatchMessageInspector, IClientMessageInspector
    {
        #region Constants

        private const string CorrelationIdHeader = "CorrelationIdHeader";
        private const string UserNameHeader = "UserNameHeader";
        private const string SessionIdHeader = "SessionIdHeader";

        #endregion

        #region Fields

        private readonly IFrameworkTracer _tracer;
        private DateTime _clientSideStartTime;
        private DateTime _serverSideStartTime;
        private string _webServiceName, _operationName;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of the <see cref="PerformanceLogInterceptor" />.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <param name="traceSourceName">The name of the trace source.</param>
        public PerformanceLogInterceptor(TraceSourceType? traceSourceType, string traceSourceName)
        {
            if (!traceSourceType.HasValue || string.IsNullOrEmpty(traceSourceName))
                _tracer = DotNetTracerFactory.Instance.GetCurrentTracer();
            else
                _tracer = DotNetTracerFactory.Instance.CreateTracer(traceSourceType.Value, traceSourceName);
        }

        #endregion

        #region Methods

        private static void AddMessageHeader<T>(Message message, string headerName, T headerValue)
        {
            HttpRequestMessageProperty httpRequestProperty;
            if (!message.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                httpRequestProperty = new HttpRequestMessageProperty();
                message.Properties.Add(HttpRequestMessageProperty.Name, httpRequestProperty);
            }
            httpRequestProperty = message.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            httpRequestProperty?.Headers.Add(headerName, Convert.ToString(headerValue, CultureInfo.InvariantCulture));
        }

        private static string GetMessageHeader(Message message, string headerName)
        {
            HttpRequestMessageProperty httpRequestProperty = message.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            return httpRequestProperty?.Headers[headerName];
        }

        private static void ExtractNames(Message message, IClientChannel channel, out string webServiceName, out string operationName)
        {
            webServiceName = channel.GetType().Name;
            operationName = message.Headers.Action;
            Uri tempUri;
            if (Uri.TryCreate(operationName, UriKind.Absolute, out tempUri))
            {
                var components = tempUri.GetComponents(UriComponents.Path, UriFormat.Unescaped).Split('/');
                if (components.Length > 0)
                {
                    operationName = components.Last();
                }
            }
        }

        #endregion

        #region IClientMessageInspector Members

        /// <summary>
        ///     Enables inspection or modification of a message after a reply message is received but prior to passing it back to
        ///     the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply == null || _tracer == null) return;

            TraceContext context = TraceContext.Create(Constants.TraceParameter.Name, _webServiceName)
                .Add(Constants.TraceParameter.Operation, _operationName);

            string eventName = _webServiceName + "." + _operationName;
            string message = string.Format(CultureInfo.CurrentCulture, InternalMessages.WebServiceCall, eventName);

            _tracer.TraceStopPerformance(_clientSideStartTime, message, TraceEventType.HttpRequest, eventName, context);
        }

        /// <summary>
        ///     Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <returns>
        ///     The object that is returned as the correlationState argument of the <see cref="AfterReceiveReply" /> method.
        ///     This is null if no correlation state is used.
        /// </returns>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (request == null || _tracer == null) return null;

            ExtractNames(request, channel, out _webServiceName, out _operationName);

            Guid correlationId = _tracer.GetCorrelationId();
            if (correlationId != Guid.Empty)
                AddMessageHeader(request, CorrelationIdHeader, correlationId);

            string userName = _tracer.GetPrincipalId();
            if (!string.IsNullOrWhiteSpace(userName))
                AddMessageHeader(request, UserNameHeader, userName);

            string sessionId = _tracer.GetSessionId();
            if (!string.IsNullOrWhiteSpace(sessionId))
                AddMessageHeader(request, SessionIdHeader, sessionId);

            _clientSideStartTime = _tracer.TraceStartPerformance();
            return null;
        }

        #endregion

        #region IDispatchMessageInspector Members

        /// <summary>
        ///     Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        ///     The object used to correlate state. This object is passed back in the
        ///     <see
        ///         cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" />
        ///     method.
        /// </returns>
        /// <param name="request">The message message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (request == null || _tracer == null) return null;

            ExtractNames(request, channel, out _webServiceName, out _operationName);

            Guid correlationGuid;
            string correlationId = GetMessageHeader(request, CorrelationIdHeader);
            if (!string.IsNullOrWhiteSpace(correlationId) && Guid.TryParse(correlationId, out correlationGuid) && correlationGuid != Guid.Empty)
                _tracer.SetCorrelationId(correlationGuid);

            string userName = GetMessageHeader(request, UserNameHeader);
            if (!string.IsNullOrWhiteSpace(userName))
                _tracer.SetPrincipalId(userName);

            string sessionId = GetMessageHeader(request, SessionIdHeader);
            if (!string.IsNullOrWhiteSpace(sessionId))
                _tracer.SetSessionId(sessionId);

            _serverSideStartTime = _tracer.TraceStartPerformance();
            return null;
        }

        /// <summary>
        ///     Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">
        ///     The correlation object returned from the
        ///     <see
        ///         cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" />
        ///     method.
        /// </param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (_tracer == null) return;

            TraceContext context = TraceContext.Create(Constants.TraceParameter.Name, _webServiceName)
                .Add(Constants.TraceParameter.Operation, _operationName);
            string eventName = _webServiceName + "." + _operationName;
            string message = string.Format(CultureInfo.CurrentCulture, InternalMessages.WebServiceResponse, eventName);
            _tracer.TraceStopPerformance(_serverSideStartTime, message, TraceEventType.HttpResponse, eventName, context);
        }

        #endregion
    }
}
