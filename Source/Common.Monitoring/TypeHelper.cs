using System;

namespace Common.Monitoring
{
    /// <summary>
    ///     Helper class for type manipulation.
    /// </summary>
    public static class TypeHelper
    {
        #region Methods

        /// <summary>
        ///     Generates a Guid based on the specified int value.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>A guid based on the specified int value.</returns>
        public static Guid ToGuid(this int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        /// <summary>
        ///     Ensures the specified <see cref="string" /> is shorten to the specified length if applicable.
        /// </summary>
        /// <param name="data">The <see cref="string" /> to shorten.</param>
        /// <param name="length">The maximal length.</param>
        /// <returns>The shortened <see cref="string" /> or null.</returns>
        public static string ToSize(this string data, int length)
        {
            if (data == null || data.Length <= length) return data;
            return data.Substring(0, length);
        }

        /// <summary>
        ///     Ensures the specified <see cref="Exception" /> string representation is shorten to the specified length if
        ///     applicable.
        /// </summary>
        /// <param name="exception">The <see cref="Exception" /> string representation to shorten.</param>
        /// <param name="length">The maximal length.</param>
        /// <returns>The shortened <see cref="Exception" /> string representation or null.</returns>
        public static string ToSize(this Exception exception, int length)
        {
            return exception?.ToString().ToSize(length);
        }

        /// <summary>
        ///     Get the ticks of the specified nullable <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan" /> to get the ticks.</param>
        /// <returns>0 if null or the number of ticks otherwise.</returns>
        public static long ToTicks(this TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue) return timeSpan.Value.Ticks;
            return 0;
        }

        #endregion
    }
}
