namespace Common.Monitoring
{
    /// <summary>
    ///     Enumerates (technical) types of trace events (<see cref="TraceEventData" />).
    /// </summary>
    /// <remarks>
    ///     - This enables events to be categorized and filtered.
    /// </remarks>
    public enum TraceEventType
    {
        /// <summary>
        ///     The default type : log entry
        /// </summary>
        Log = 0,

        /// <summary>
        ///     an error caught at global level
        /// </summary>
        GlobalLevelCaughtError,

        /// <summary>
        ///     an error caught at a local level
        /// </summary>
        FirstChanceError,

        /// <summary>
        ///     a trace event type relative to code execution
        /// </summary>
        CodeBlock,

        /// <summary>
        ///     a trace event type relative to an http request
        /// </summary>
        HttpRequest,

        /// <summary>
        ///     a trace event type relative to an http response
        /// </summary>
        HttpResponse,

        /// <summary>
        ///     a trace event type relative to a database call
        /// </summary>
        DatabaseCall,

        /// <summary>
        ///     a trace event type relative to a command launched by the user
        /// </summary>
        UserCommand

    }
}
