using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml.Linq;
using System.Xml.XPath;
using Common.Resources;

namespace Common.Monitoring.Listeners
{
    /// <summary>
    ///     Defines a .NET traces listener (<see cref="TraceListener" />) that writes asynchronously traces
    ///     in database or through a Web API.
    /// </summary>
    /// <remarks>
    ///     A timer will call periodically a stored procedure at a specified frequency. It will process up to a specified
    ///     number of trace eventsList.
    /// </remarks>
    /// <code>
    ///   <system.diagnostics>
    ///         <sharedListeners>
    ///             <add name="AsyncDbListener"
    ///                 type="AF.Color.Framework.Diagnostics.AsyncDatabaseTraceListener,AF.Color.Framework"
    ///                 connectionString="Data Source=.;Initial Catalog=MONITORING;Integrated Security=True;Connect Timeout=15;Encrypt=False;" />
    ///         </sharedListeners>
    ///         <sources>
    ///             <source name="Batch" switchValue="All">
    ///                 <listeners>
    ///                     <add name="AsyncDbListener" />
    ///                 </listeners>
    ///             </source>
    ///         </sources>
    ///     </system.diagnostics>
    /// </code>
    [DebuggerStepThrough]
    public class AsyncDatabaseTraceListener : TraceListener
    {
        #region Constructor

        public AsyncDatabaseTraceListener()
        {
            Initialize();
        }

        #endregion

        #region Fields

        // queue for storing trace events.
        private readonly ConcurrentQueue<TraceEventData> _tracesQueue = new ConcurrentQueue<TraceEventData>();
        // call frequency.
        private int _callFrequency;
        // the name of connection string to trace events database.
        private string _connectionStringName;
        // max queue size.
        private int _maxQueueSize;
        // max trace events that can be written during each call.
        private int _maxTraceEventsByCall;
        // remaining error to log.
        private int _remainingErrorsToLog = 100;
        // the uri to the rest api store
        private Uri _restApiStoreUri;

        // timer dedicated to trace send.
        private Timer _senderTimer;
        // stored procedure name to write trace events.
        private string _storedProcedureName = Default.MonitoringStoredProcedure;
        // the method for writing events to a store
        private Action<IEnumerable<TraceEventData>> _writeEventsToStore;

        #endregion

        #region Properties

        /// <summary>
        ///     Is the <see cref="AsyncDatabaseTraceListener" /> thread safe?
        /// </summary>
        public override bool IsThreadSafe => (true);

        #endregion

        #region Methods

        /// <summary>
        ///     Dispose internal stuff.
        /// </summary>
        /// <param name="disposing">Is the dispose the user sollicated call ?</param>
        protected override void Dispose(bool disposing)
        {
            while (SendTraces())
            {
            }
            _senderTimer?.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        ///     Returns the list of supported attributes for the <see cref="AsyncDatabaseTraceListener" />.
        /// </summary>
        /// <returns>The list of supported attributes.</returns>
        protected override string[] GetSupportedAttributes()
        {
            return new[]
            {
                AttributeName.MaxTraceEventsByCall,
                AttributeName.CallFrequency,
                AttributeName.MaxQueueSize,
                AttributeName.StoredProcedureName,
                AttributeName.ConnectionString
            };
        }

        /// <summary>
        ///     Trace the specified data.
        /// </summary>
        /// <param name="eventCache">The specified cache object.</param>
        /// <param name="source">The name of the source</param>
        /// <param name="eventType">The specified System.Dagnostics trace event type.</param>
        /// <param name="id">The specified event id.</param>
        /// <param name="data">The custom data object that will hold our <see cref="TraceEventData" /> object.</param>
        public override void TraceData(TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, object data)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                return;

            if (_tracesQueue.Count >= _maxQueueSize)
            {
                // No connection string
                if (--_remainingErrorsToLog > 0)
                    RawLogger.LogWarning(ErrorCodes.Framework.ListenerFlooded, InternalMessages.FrameworkListenerFlooded);
                return;
            }

            _tracesQueue.Enqueue(data as TraceEventData);
        }

        /// <summary>
        ///     WriteToDatabase the specified message.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <remarks>
        ///     - Required to be overridden.
        /// </remarks>
        public override void Write(string message)
        {
        }

        /// <summary>
        ///     WriteToDatabase the specified message.
        /// </summary>
        /// <param name="message">The specified message.</param>
        /// <remarks>
        ///     - Required to be overridden.
        /// </remarks>
        public override void WriteLine(string message)
        {
        }

        /// <summary>
        ///     Flush the remaining trace events.
        /// </summary>
        public override void Flush()
        {
            SendTraces();
            base.Flush();
        }

        /// <summary>
        ///     Initialize the <see cref="AsyncDatabaseTraceListener" />.
        /// </summary>
        private void Initialize()
        {
            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            XElement configuration = XDocument.Load(configurationFile).XPathSelectElement($"//system.diagnostics/sharedListeners/add[starts-with(@type, '{GetType().FullName}')]");
            if (configuration == null)
            {
                RawLogger.LogError(ErrorCodes.Framework.ListenerInvalidConfiguration, InternalMessages.FrameworkListenerMissingConfiguration);
                return;
            }

            _maxTraceEventsByCall = GetAttributeValue(configuration, AttributeName.MaxTraceEventsByCall, Default.MaxTraceEventsByCall);
            _callFrequency = GetAttributeValue(configuration, AttributeName.CallFrequency, Default.CallFrequency);
            _maxQueueSize = GetAttributeValue(configuration, AttributeName.MaxQueueSize, Default.MaxQueueSize);

            var connectionStringAttribute = configuration.Attributes().FirstOrDefault(a => a.Name == AttributeName.ConnectionString);
            if (connectionStringAttribute == null)
            {
                // No connection string
                RawLogger.LogError(ErrorCodes.Framework.ListenerInvalidConfiguration,
                    string.Format(CultureInfo.CurrentCulture, InternalMessages.FrameworkListenerMissingAttribute, AttributeName.ConnectionString));
                return;
            }
            _connectionStringName = connectionStringAttribute.Value;
            // empty connection string
            if (string.IsNullOrWhiteSpace(_connectionStringName))
            {
                RawLogger.LogError(ErrorCodes.Framework.ListenerInvalidConfiguration,
                    string.Format(CultureInfo.CurrentCulture, InternalMessages.FrameworkListenerInvalidAttribute, AttributeName.ConnectionString));
                return;
            }
            // select tha appropriate method to write events to store by checking if connection string is rest api uri
            // if not, it is assumed to be a sql connection string
            _writeEventsToStore = WriteEventsToDatabase;
            bool isRestApiMethodSelected = Uri.IsWellFormedUriString(_connectionStringName, UriKind.Absolute);
            if (isRestApiMethodSelected)
            {
                _restApiStoreUri = new Uri(_connectionStringName, UriKind.Absolute);
                _writeEventsToStore = WriteEventsToRestApi;
            }
            // start the timer
            if (_senderTimer == null)
                _senderTimer = new Timer(TimerCallback, _senderTimer, 0, _callFrequency);
        }

        private static int GetAttributeValue(XElement configuration, string attributeName, int defaultValue)
        {
            var attribute = configuration.Attributes().FirstOrDefault(a => a.Name == attributeName);
            if (attribute == null) return defaultValue;
            int temp;
            if (int.TryParse(attribute.Value, out temp) && temp > 0)
                return temp;
            RawLogger.LogWarning(ErrorCodes.Framework.ListenerInvalidConfiguration,
                string.Format(CultureInfo.CurrentCulture, InternalMessages.FrameworkListenerInvalidAttributeBackToDefault,
                    attributeName, defaultValue));
            return defaultValue;
        }

        /// <summary>
        ///     The timer callback.
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallback(object state)
        {
            _senderTimer.Change(Timeout.Infinite, Timeout.Infinite);
            SendTraces();
            _senderTimer.Change(_callFrequency, _callFrequency);
        }

        /// <summary>
        ///     Provides a <see cref="DataTable" /> of <see cref="TraceEventData" /> in order
        ///     to facilitate Sql table-valued-parameter usage.
        /// </summary>
        /// <param name="traceEvents">The trace events.</param>
        /// <returns></returns>
        private DataTable GetDataTable(IEnumerable<TraceEventData> traceEvents)
        {
            DataTable table = new DataTable { Locale = CultureInfo.CurrentCulture };
            table.Columns.Add("CreationDate", typeof(DateTime)); //DATETIME2        NOT NULL,
            table.Columns.Add("CorrelationId", typeof(Guid)); //UNIQUEIDENTIFIER NOT NULL,
            table.Columns.Add("SourceType", typeof(string)); //NVARCHAR (64)	NOT NULL,
            table.Columns.Add("SourceName", typeof(string)); //NVARCHAR (64)	NOT NULL,
            table.Columns.Add("EventType", typeof(string)); //NVARCHAR (64)	NOT NULL,
            table.Columns.Add("EventName", typeof(string)); //NVARCHAR (256)	NULL,
            table.Columns.Add("Category", typeof(int)); //INT              NOT NULL,
            table.Columns.Add("Message", typeof(string)); //NVARCHAR (MAX)   NULL,
            table.Columns.Add("ContextParameter", typeof(string)); //NVARCHAR (MAX)   NULL,
            table.Columns.Add("ElapsedTime", typeof(DateTime)); //TIME             NULL,
            table.Columns.Add("ErrorCode", typeof(int)); //INT				NULL,
            table.Columns.Add("Exception", typeof(string)); //NVARCHAR (MAX)   NULL,
            table.Columns.Add("ExceptionType", typeof(string)); //NVARCHAR (64)	NULL,
            table.Columns.Add("StackTrace", typeof(string)); //NVARCHAR (MAX)   NULL,
            table.Columns.Add("ProcessName", typeof(string)); //NVARCHAR (64)	NULL,
            table.Columns.Add("ProcessId", typeof(int)); //INT              NULL,
            table.Columns.Add("ThreadName", typeof(string)); //NVARCHAR (64)	NULL,
            table.Columns.Add("ThreadId", typeof(int)); //INT              NULL,
            table.Columns.Add("MachineName", typeof(string)); //NVARCHAR (64)	NULL,
            table.Columns.Add("SessionId", typeof(string)); //NVARCHAR (64)	NULL,
            table.Columns.Add("UserName", typeof(string)); //NVARCHAR (64)	NULL

            foreach (TraceEventData traceEvent in traceEvents.OrderBy(t => t.CreationDate))
            {
                table.Rows.Add(
                    traceEvent.CreationDate,
                    traceEvent.CorrelationId,
                    traceEvent.TraceSourceType.ToString().ToSize(64),
                    traceEvent.TraceSourceName.ToSize(64),
                    traceEvent.TraceEventType.ToString().ToSize(64),
                    traceEvent.TraceEventName.ToSize(256),
                    traceEvent.TraceCategory,
                    traceEvent.Message,
                    traceEvent.ContextParameter,
                    traceEvent.ElapsedTime.HasValue ? new DateTime(Math.Abs(traceEvent.ElapsedTime.Value.Ticks)) : DateTime.MinValue,
                    traceEvent.ErrorCode,
                    traceEvent.Exception,
                    traceEvent.ExceptionType?.ToSize(64),
                    traceEvent.StackTrace,
                    traceEvent.ProcessName.ToSize(64),
                    traceEvent.ProcessId,
                    traceEvent.ThreadName.ToSize(64),
                    traceEvent.ThreadId,
                    traceEvent.MachineName.ToSize(64),
                    traceEvent.SessionId.ToSize(64),
                    traceEvent.UserName.ToSize(64)
                );
            }
            return table;
        }

        /// <summary>
        ///     Sends up to MaxTraceEventbyPacket trace events to the writing delegate.
        /// </summary>
        /// <returns>True if there is still traces to emit, false otherwise.</returns>
        private bool SendTraces()
        {
            if (_writeEventsToStore == null || _tracesQueue.IsEmpty) return false;

            int traceEventsCount = Math.Min(_maxTraceEventsByCall, _tracesQueue.Count);
            List<TraceEventData> eventsList = new List<TraceEventData>();
            for (int i = 0; i < traceEventsCount; i++)
            {
                TraceEventData item;
                if (_tracesQueue.TryDequeue(out item))
                {
                    eventsList.Add(item);
                }
                else
                {
                    break;
                }
            }
            if (eventsList.Count <= 0) return false;
            try
            {
                _writeEventsToStore(eventsList);
            }
            catch (Exception exception)
            {
                // 1. Because the TraceListener is written to manage Exception we do not want to rethrow it,
                //    once again to avoid infinite loop: but we log the error in the event-log to trace the problem.
                // 2. We also decrement a counter to avoid filling the event-log.
                if (--_remainingErrorsToLog > 0)
                    RawLogger.LogWarning(ErrorCodes.Framework.ListenerLoggingError,
                        string.Format(CultureInfo.CurrentCulture, InternalMessages.FrameworkListenerLoggingError, exception));
            }
            return true;
        }

        /// <summary>
        ///     Writes the specified trace events by calling a Web API.
        /// </summary>
        /// <param name="traceEvents">The collection of trace events.</param>
        private void WriteEventsToRestApi(IEnumerable<TraceEventData> traceEvents)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Credentials = CredentialCache.DefaultCredentials;
                //string jsonResult = JsonConvert.SerializeObject(traceEvents.ToArray());
                string jsonResult = "JsonConvert not available";
                client.UploadString(_restApiStoreUri, jsonResult);
            }
        }

        /// <summary>
        ///     Writes the specified trace events to the database in a batch way.
        /// </summary>
        /// <param name="traceEvents">The collection of trace events.</param>
        private void WriteEventsToDatabase(IEnumerable<TraceEventData> traceEvents)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (SqlConnection connection = new SqlConnection(_connectionStringName))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = _storedProcedureName;
                using (DataTable data = GetDataTable(traceEvents))
                {
                    // Configure the SqlCommand and SqlParameter (table value parameter).
                    SqlParameter parameter = command.Parameters.AddWithValue(Default.MonitoringParameter, data);
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.TypeName = "[Monitoring].[TraceEventTableType]";
                    // Execute the command.
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Nested type: AttributeName

        /// <summary>
        ///     TraceListener attributes names.
        /// </summary>
        private static class AttributeName
        {
            #region Constants

            internal const string StoredProcedureName = "storedProcedureName";
            internal const string MaxTraceEventsByCall = "maxTraceEventsByCall";
            internal const string CallFrequency = "callFrequency";
            internal const string MaxQueueSize = "maxQueueSize";
            internal const string ConnectionString = "connectionString";

            #endregion
        }

        #endregion

        #region Nested type: Default

        /// <summary>
        ///     TraceListener default attributes values.
        /// </summary>
        private static class Default
        {
            #region Constants

            // default stored procedure name used to write trace events.
            internal const string MonitoringStoredProcedure = "[Monitoring].[InsertTraceEvents]";
            internal const string MonitoringParameter = "@TraceTable";

            // default max trace eventsList by call
            internal const int MaxTraceEventsByCall = 1000;
            // default call frequency in milliseconds: each 2s.
            internal const int CallFrequency = 1000 * 2;
            // default max queue size: 5000.
            internal const int MaxQueueSize = 250000;

            #endregion
        }

        #endregion
    }
}
