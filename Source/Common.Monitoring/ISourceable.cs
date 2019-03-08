namespace Common.Monitoring
{
    /// <summary>
    ///     Describes the ability for a tracer to specify its trace source.
    /// </summary>
    public interface ISourceable
    {
        #region Methods

        /// <summary>
        ///     Creates a trace source of the specified trace source type with the specified trace source name.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <param name="traceSourceName">The instance name of the trace source type.</param>
        void SetSource(TraceSourceType traceSourceType, string traceSourceName);

        #endregion
    }
}
