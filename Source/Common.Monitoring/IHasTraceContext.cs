namespace Common.Monitoring
{
    /// <summary>
    /// Indicates that the implementer provides its own <see cref="TraceContext"/>.
    /// </summary>
    public interface IHasTraceContext
    {
        /// <summary>
        /// The specified <see cref="TraceContext"/>.
        /// </summary>
        TraceContext TraceContext { get; }
    }
}
