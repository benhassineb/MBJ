using System;
using System.Diagnostics;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines a higher resolution DateTime class.
    /// </summary>
    /// <remarks>
    ///     Use HighResolutionDateTime.UtcNow rather than DateTime.UtcNow.
    ///     To calculate delay use HighResolutionDateTime.GetTimeSpan(dt) rather than (dt - HighResolutionDateTime.UtcNow)
    ///     to avoid potential time resync in-between causing negative and inaccurate value.
    /// </remarks>
    internal static class HighResolutionDateTime
    {
        #region Constants

        private static DateTime _startTime;
        private static Stopwatch _stopWatch;

        // default timespan before trying to resync with computer time 
        private static readonly TimeSpan MaxIdle = TimeSpan.FromSeconds(10);

        #endregion

        #region Properties

        /// <summary>
        ///     Get a <see cref="System.DateTime" /> object that is set to the current date and time on this computer, expressed
        ///     as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <remarks>
        ///     Use a <see cref="Stopwatch" /> object to provide higher resolution.
        /// </remarks>
        public static DateTime UtcNow => GetUtcNow(true);

        #endregion

        #region Methods

        /// <summary>
        ///     Get the delay between now and the specified <see cref="DateTime" />.
        /// </summary>
        /// <param name="dateTime">The specified datetime.</param>
        /// <returns>
        ///     The corresponding delay.
        /// </returns>
        public static TimeSpan GetTimeSpan(DateTime dateTime)
        {
            return GetUtcNow(false) - dateTime;
        }

        private static DateTime GetUtcNow(bool canResync)
        {
            if (_stopWatch != null && (!canResync || _startTime.Add(MaxIdle) >= DateTime.UtcNow))
                return _startTime.AddTicks(_stopWatch.Elapsed.Ticks);
            // resync time from time to time to avoid drifting
            _startTime = DateTime.UtcNow;
            _stopWatch = Stopwatch.StartNew();
            return _startTime.AddTicks(_stopWatch.Elapsed.Ticks);
        }

        #endregion
    }
}
