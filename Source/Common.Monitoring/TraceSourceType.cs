namespace Common.Monitoring
{
    /// <summary>
    ///     Enumerates the types of sources of trace events (<see cref="TraceEventData" />).
    /// </summary>
    public enum TraceSourceType
    {
        /// <summary>
        ///     not set    
        /// </summary>
        Unset,

        /// <summary>
        ///     desktop clients
        /// </summary>
        Client,

        /// <summary>
        ///     webs
        /// </summary>
        Web,

        /// <summary>
        ///     web services
        /// </summary>
        WebService,

        /// <summary>
        ///     NT services
        /// </summary>
        Service,

        /// <summary>
        ///     batches
        /// </summary>
        Batch,

        /// <summary>
        ///     databases
        /// </summary>
        Database
    }
}
