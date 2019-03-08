using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Monitoring
{
    /// <summary>
    ///     Defines a collection of parameters (<see cref="TraceParameter" />) associated to
    ///     a trace event (<see cref="TraceEventData" />).
    /// </summary>
    /// <remarks>
    ///     Usage:
    ///     <code>
    ///     TraceContext context = TraceContext.Create(Parameters.WebServiceName, "...");
    ///     </code>
    ///     <code>
    ///     TraceContext context = TraceContext.Create().Add(Parameters.P1,"...").Add(Parameters.P2,"...");
    ///     </code>
    /// </remarks>
    public class TraceContext
    {
        #region Constants

        private const string SqlCommand = "SqlCommand";
        private const string SqlCommandTimeout = "SqlCommandTimeout";

        // Aggregation trace parameter delegate
        private static readonly Func<string, TraceParameter, string> AggregateTraceParameters =
            (all, parameter) => all + "|" + parameter.Name + "¤" + parameter.Value;

        // Aggregation delegates for SQL command parameters
        private static readonly Func<string, string, string> AggregateDataParameterStrings =
            (all, parameterString) => all + ", " + parameterString;

        private static readonly Func<IDataParameter, string> DataParameterAsString = parameter =>
        {
            string parameterName = parameter.ParameterName.StartsWith("@", StringComparison.OrdinalIgnoreCase)
                ? parameter.ParameterName
                : "@" + parameter.ParameterName;
            if (parameter.Value != null)
            {
                if (parameter.Value.GetType().IsSubclassOf(typeof(DataTable)))
                {
                    var valDataTable = (DataTable) parameter.Value;
                    var sb = new StringBuilder().AppendLine();
                    var columnNames = valDataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                    sb.AppendLine(string.Join(",", columnNames));
                    foreach (DataRow row in valDataTable.Rows)
                    {
                        var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                        sb.AppendLine(string.Join(",", fields));
                    }
                    return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", parameterName, sb.ToString()); 
                }
                else
                {

                    string parameterValue = (parameter.DbType == DbType.Date || parameter.DbType == DbType.DateTime2 ||
                                             parameter.DbType == DbType.DateTime)
                        ? string.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", parameter.Value)
                        : parameter.Value.ToString();


                    return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", parameterName, parameterValue);
                }
            }
            else
                return string.Format(CultureInfo.InvariantCulture, "{0} IS NULL", parameterName);

        };

        #endregion

        #region Fields

        /// <summary>
        ///     The array of <see cref="TraceParameter" /> of the <see cref="TraceContext" />.
        /// </summary>
        private readonly List<TraceParameter> _parameters;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of <see cref="TraceContext" /> based on the specified array of <see cref="TraceParameter" />.
        /// </summary>
        private TraceContext(TraceParameter[] parameters)
        {
            _parameters = new List<TraceParameter>();
            if (parameters == null) return;
            _parameters.AddRange(parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates an instance of <see cref="TraceContext" /> based on the specified array of <see cref="TraceParameter" />.
        /// </summary>
        /// <param name="parameters">The specified <see cref="TraceParameter" />.</param>
        /// <returns>The new instance of <see cref="TraceContext" />.</returns>
        public static TraceContext Create(params TraceParameter[] parameters)
        {
            return new TraceContext(parameters);
        }

        /// <summary>
        ///     Creates an instance of <see cref="TraceContext" /> based on the specified <see cref="IDbCommand" />.
        /// </summary>
        /// <param name="command">The specified <see cref="IDbCommand" />.</param>
        /// <returns>The new instance of <see cref="TraceContext" />.</returns>
        public static TraceContext Create(IDbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            TraceParameter parameter1 = TraceParameter.Create(SqlCommandTimeout, command.CommandTimeout);
            string sqlCommandValue = string.Format(CultureInfo.InvariantCulture, "EXEC {0} ", command.CommandText);
            if (command.Parameters != null && command.Parameters.Count > 0)
                sqlCommandValue +=
                    command.Parameters.OfType<IDataParameter>().Select(p => DataParameterAsString(p)).Aggregate(AggregateDataParameterStrings);
            TraceParameter parameter2 = TraceParameter.Create(SqlCommand, sqlCommandValue);
            return Create(parameter2, parameter1);
        }


        /// <summary>
        ///     Creates an instance of <see cref="TraceContext" /> based on the specified parameter name and value.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The new instance of <see cref="TraceContext" />.</returns>
        public static TraceContext Create<T>(string parameterName, T parameterValue)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            return Create(TraceParameter.Create(parameterName, parameterValue));
        }

        /// <summary>
        ///     Adds the specified enumeration of <see cref="TraceParameter" /> to the current <see cref="TraceContext" />.
        /// </summary>
        /// <param name="parameters">The enumeration of trace parameters.</param>
        /// <returns>The current instance of <see cref="TraceContext" />.</returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public TraceContext Add(IEnumerable<TraceParameter> parameters)
        {
            if (parameters != null && parameters.Any())
                _parameters.AddRange(parameters);
            return this;
        }

        /// <summary>
        ///     Adds the specified parameter name and value to the current <see cref="TraceContext" />.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>The current instance of <see cref="TraceContext" />.</returns>
        public TraceContext Add<T>(string parameterName, T parameterValue)
        {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            ITraceable traceableObject = parameterValue as ITraceable;
            if (traceableObject != null)
            {
                Add(TraceParameter.CreateCollection(parameterName, traceableObject));
            }
            else
            {
                _parameters.Add(TraceParameter.Create(parameterName, parameterValue));
            }
            return this;
        }

        /// <summary>
        ///     Creates a string representation of the <see cref="TraceContext" />.
        /// </summary>
        /// <returns>
        ///     String representation is of the form:
        ///     Param1¤param1|Param2¤param2
        /// </returns>
        public override string ToString()
        {
            if (_parameters == null || _parameters.Count == 0) return string.Empty;
            return _parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Name)).Aggregate(string.Empty, AggregateTraceParameters);
        }

        #endregion
    }
}