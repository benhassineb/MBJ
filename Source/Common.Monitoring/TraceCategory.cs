namespace Common.Monitoring
{
    /// <summary>
    ///     Enumerates the categories of trace events (<see cref="TraceEventData" />):
    ///     Info, Performance, Warning, Error.
    /// </summary>
    internal enum TraceCategory
    {
        /// <summary>
        ///     Information traces
        /// </summary>
        Information,

        /// <summary>
        ///     Performance traces
        /// </summary>
        Performance,

        /// <summary>
        ///     Verbose traces
        /// </summary>
        Verbose,

        /// <summary>
        ///     Warning traces
        /// </summary>
        Warning,

        /// <summary>
        ///     Error traces
        /// </summary>
        Error
    }
}
