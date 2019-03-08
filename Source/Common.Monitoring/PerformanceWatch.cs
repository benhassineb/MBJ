using System;
using System.Diagnostics;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines a performance logging trace that encapsulates <see cref="Stopwatch" />.
    /// </summary>
    /// <remarks>
    ///     This is used for sugar syntax like this:
    ///     <code>
    ///      using (logger.TracePerformance(TraceEventType.WebServiceCall, ...))
    ///      {
    ///          ...
    ///      }
    /// </code>
    /// </remarks>
    internal class PerformanceWatch : IDisposable
    {
        #region Fields

        private readonly Action<TimeSpan> _callback;
        private readonly Stopwatch _watch;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of <see cref="PerformanceWatch" />.
        /// </summary>
        /// <param name="callback">Callback when disposed.</param>
        [DebuggerStepThrough]
        public PerformanceWatch(Action<TimeSpan> callback)
        {
            _watch = new Stopwatch();
            _watch.Start();
            _callback = callback;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     Dispose is used to stop the watch.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            _watch.Stop();
            _callback?.Invoke(_watch.Elapsed);
        }

        #endregion
    }
}
