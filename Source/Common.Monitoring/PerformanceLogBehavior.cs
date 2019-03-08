using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Common.Monitoring
{
    /// <summary>
    ///     WCF extension of type <see cref="IEndpointBehavior" /> in order to insert behaviors.
    /// </summary>
    public class PerformanceLogBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        #region Constants

        private const string TraceSourceTypeAttributeName = "TraceSourceType";
        private const string TraceSourceNameAttributeName = "TraceSourceName";

        #endregion

        #region Fields

        private ConfigurationPropertyCollection _propertyCollection;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of <see cref="PerformanceLogBehavior" />.
        /// </summary>
        public PerformanceLogBehavior()
        {
        }

        /// <summary>
        ///     Creates an instance of <see cref="PerformanceLogBehavior" />.
        /// </summary>
        /// <param name="traceSourceType">The specified trace source type.</param>
        /// <param name="traceSourceName">The name of the trace source.</param>
        public PerformanceLogBehavior(TraceSourceType? traceSourceType = null, string traceSourceName = null)
        {
            TraceSourceType = traceSourceType;
            TraceSourceName = traceSourceName;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The type of the trace source.
        /// </summary>
        [ConfigurationProperty(TraceSourceTypeAttributeName, IsRequired = false)]
        public TraceSourceType? TraceSourceType
        {
            get => (TraceSourceType?)base[TraceSourceTypeAttributeName];
            set => base[TraceSourceTypeAttributeName] = value;
        }

        /// <summary>
        ///     The name of the trace source.
        /// </summary>
        [ConfigurationProperty(TraceSourceNameAttributeName, IsRequired = false)]
        public string TraceSourceName
        {
            get => (string)base[TraceSourceNameAttributeName];
            set => base[TraceSourceNameAttributeName] = value;
        }

        /// <summary>
        ///     Gets the collection of properties.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties => _propertyCollection ?? (_propertyCollection = new ConfigurationPropertyCollection
        {
            new ConfigurationProperty(TraceSourceTypeAttributeName, typeof(TraceSourceType?), null),
            new ConfigurationProperty(TraceSourceNameAttributeName, typeof(string), null)
        });

        /// <summary>
        ///     Gets the type of behavior.
        /// </summary>
        /// <returns>
        ///     The type of behavior.
        /// </returns>
        public override Type BehaviorType
        {
            get { return typeof(PerformanceLogBehavior); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        ///     The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new PerformanceLogBehavior(TraceSourceType, TraceSourceName);
        }

        /// <summary>
        ///     Copies the content of the specified configuration element to this configuration element.
        /// </summary>
        /// <param name="from">The specified configuration element.</param>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            PerformanceLogBehavior element = from as PerformanceLogBehavior;
            if (element == null) return;
            TraceSourceType = element.TraceSourceType;
            TraceSourceName = element.TraceSourceName;
        }

        #endregion

        #region IEndpointBehavior Members

        /// <summary>
        ///     Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        ///     Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime == null) throw new ArgumentNullException(nameof(clientRuntime));
            PerformanceLogInterceptor interceptor = new PerformanceLogInterceptor(TraceSourceType, TraceSourceName);
            clientRuntime.MessageInspectors.Add(interceptor);
        }

        /// <summary>
        ///     Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (endpointDispatcher == null) throw new ArgumentNullException(nameof(endpointDispatcher));
            PerformanceLogInterceptor interceptor = new PerformanceLogInterceptor(TraceSourceType, TraceSourceName);
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(interceptor);
        }

        /// <summary>
        ///     Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
