using System.Collections.Generic;

namespace Common.Monitoring
{
    /// <summary>
    ///     Indicates that the implementer has trace parameters to provide.
    /// </summary>
    public interface ITraceable
    {
        /// <summary>
        ///     Additional trace parameters to be added in the trace context.
        /// </summary>
        IEnumerable<TraceParameter> TraceParameters { get; }
    }
}
