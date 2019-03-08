using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable StaticMemberInGenericType

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines a generic factory of <see cref="IFrameworkTracer" />.
    /// </summary>
    public class TracerFactory<TLogger> : ITracerFactory
        where TLogger : IFrameworkTracer, ISourceable, new()
    {
        #region Constants

        // The lookup of all trace sources.
        private static readonly Dictionary<string, TLogger> TracesSources = new Dictionary<string, TLogger>();
        // The logger factory instance
        private static volatile ITracerFactory _instance;

        // A lock for singleton creation
        private static readonly object Lock = new object();

        #endregion

        #region Properties

        /// <summary>
        ///     A static instance of the <see cref="TracerFactory{TLogger}" />.
        /// </summary>
        public static ITracerFactory Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new TracerFactory<TLogger>();
                }
                return _instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Get the <see cref="IFrameworkTracer" /> corresponding to the specified traceSourceType.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <returns>
        ///     A <see cref="IFrameworkTracer" /> corresponding to specified parameter.
        /// </returns>
        /// <remarks>
        ///     A new one is created if necessary.
        /// </remarks>
        public IFrameworkTracer GetFrameworkLogger(TraceSourceType traceSourceType)
        {
            string traceSourceName = GetType().Name;
            return GetLoggerInternal(traceSourceType, traceSourceName);
        }

        /// <summary>
        ///     Get the specified logger for the specified trace source.
        /// </summary>
        /// <param name="traceSourceType">The type of the source.</param>
        /// <param name="traceSourceName">The instance name of the source type.</param>
        /// <returns>
        ///     The corresponding logger.
        /// </returns>
        private static TLogger GetLoggerInternal(TraceSourceType traceSourceType, string traceSourceName)
        {
            if (string.IsNullOrEmpty(traceSourceName)) throw new ArgumentNullException(nameof(traceSourceName));
            string key = traceSourceType + "¤" + traceSourceName;
            if (!TracesSources.ContainsKey(key))
            {
                TLogger newTraceSource = new TLogger();
                newTraceSource.SetSource(traceSourceType, traceSourceName);
                TracesSources.Add(key, newTraceSource);
            }
            return TracesSources[key];
        }

        #endregion

        #region ITracerFactory Members

        /// <summary>
        ///     Creates the <see cref="IFrameworkTracer" /> corresponding to the specified traceSourceType with specified
        ///     traceSourceName.
        /// </summary>
        /// <param name="traceSourceType">The trace source type.</param>
        /// <param name="traceSourceName">The instance name of the trace source type.</param>
        /// <returns>
        ///     A <see cref="IFrameworkTracer" /> corresponding to specified parameter.
        /// </returns>
        /// <remarks>
        ///     A new one is created if necessary.
        /// </remarks>
        public IFrameworkTracer CreateTracer(TraceSourceType traceSourceType, string traceSourceName)
        {
            if (string.IsNullOrEmpty(traceSourceName)) throw new ArgumentNullException(nameof(traceSourceName));

            return GetLoggerInternal(traceSourceType, traceSourceName);
        }

        /// <summary>
        ///     Gets the current <see cref="IFrameworkTracer" /> corresponding to a previously created one.
        /// </summary>
        /// <returns>
        ///     A <see cref="IFrameworkTracer" /> corresponding to specified parameter.
        /// </returns>
        /// <remarks>
        ///     A calls to CreateTracer must have been done.
        /// </remarks>
        public IFrameworkTracer GetCurrentTracer()
        {
            if (TracesSources.Count < 1)
            {
                // create a fail-safe trace source to avoid exception
                CreateTracer(TraceSourceType.Unset, "-");
            }
            return TracesSources.Values.OfType<IFrameworkTracer>().First();
        }

        #endregion
    }
}
