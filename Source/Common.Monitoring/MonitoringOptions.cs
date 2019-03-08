namespace Common.Monitoring
{
    /// <summary>
    /// Defines the configuring options of the <see cref="ITracer"/>.
    /// </summary>
    public class MonitoringOptions
    {
        /// <summary>
        /// The connection string of the traces repository.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the source of traces.
        /// </summary>
        public string TraceSourceName { get; set; }

        /// <summary>
        /// The capability of tracing .Net first chance Exception.
        /// </summary>
        public bool LogFirstChanceExceptions { get; set; }

    }
}
