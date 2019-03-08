namespace Common.Monitoring
{
    /// <summary>
    ///     Describes the ability to instanciate <see cref="IFrameworkTracer" /> for a given trace source.
    /// </summary>
    public interface ITracerFactory
    {
        #region Methods

        /// <summary>
        ///     Creates the <see cref="IFrameworkTracer" /> corresponding to the specified traceSourceType with the specified
        ///     traceSourceName.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <param name="traceSourceName">The trace source name.</param>
        /// <returns>
        ///     A <see cref="IFrameworkTracer" /> corresponding to specified parameter.
        /// </returns>
        /// <remarks>
        ///     A new one is created if necessary.
        /// </remarks>
        IFrameworkTracer CreateTracer(TraceSourceType traceSourceType, string traceSourceName);

        /// <summary>
        ///     Gets the current <see cref="IFrameworkTracer" /> corresponding to a previously created one.
        /// </summary>
        /// <returns>
        ///     A <see cref="IFrameworkTracer" /> corresponding to specified parameter.
        /// </returns>
        /// <remarks>
        ///     A calls to CreateTracer must have been done.
        /// </remarks>
        IFrameworkTracer GetCurrentTracer();

        #endregion
    }
}
