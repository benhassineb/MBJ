using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines a trace parameter associated to a trace event (<see cref="TraceEventData" />).
    /// </summary>
    public class TraceParameter
    {
        #region Constructors

        /// <summary>
        ///     Creates an instance of <see cref="TraceParameter" /> with the specified name and value.
        /// </summary>
        /// <param name="name">The specified name.</param>
        /// <param name="value">The specified value.</param>
        private TraceParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The name of the trace parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The value of the trace parameter.
        /// </summary>
        public string Value { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates an instance of <see cref="TraceParameter" /> with the specified name and value.
        /// </summary>
        /// <param name="parameterName">The specified parameter name.</param>
        /// <param name="parameterValue">The specified parameter value.</param>
        /// <returns>The instance of the <see cref="TraceParameter" />.</returns>
        public static TraceParameter Create<T>(string parameterName, T parameterValue)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            string value = "null";
            if (parameterValue != null)
                value = typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?)
                    ? ((DateTime) (object) parameterValue).ToString("s", CultureInfo.InvariantCulture)
                    : Convert.ToString(parameterValue, CultureInfo.InvariantCulture);
            return new TraceParameter(parameterName, value);
        }

        /// <summary>
        ///     Creates an enumeration of <see cref="TraceParameter" /> with the specified prefix and the specified
        ///     <see cref="ITraceable" /> object that exposes <see cref="TraceParameter" />.
        /// </summary>
        /// <param name="prefix">The specified parameter prefix.</param>
        /// <param name="traceableObject">The specified object that exposes trace parameters.</param>
        /// <returns>The enumeration of the <see cref="TraceParameter" />.</returns>
        public static IEnumerable<TraceParameter> CreateCollection(string prefix, ITraceable traceableObject)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            if (traceableObject == null) yield break;
            foreach (var parameter in traceableObject.TraceParameters)
            {
                yield return Create(prefix + "." + parameter.Name, parameter.Value);
            }
        }

        #endregion
    }
}
