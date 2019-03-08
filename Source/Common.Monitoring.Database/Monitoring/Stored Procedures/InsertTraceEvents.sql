CREATE PROCEDURE [Monitoring].[InsertTraceEvents]
  @TraceTable AS [Monitoring].[TraceEventTableType] READONLY
AS
BEGIN

    INSERT 
    INTO [Monitoring].[Trace]
        ([CreationDate]
        ,[CorrelationId]
        ,[SourceType]
        ,[SourceName]
        ,[EventType]
        ,[EventName]
        ,[Category]
        ,[Message]
        ,[Context]
        ,[ElapsedTime]
        ,[ErrorCode]
        ,[Exception]
        ,[ExceptionType]
        ,[StackTrace]
        ,[ProcessName]
        ,[ProcessId]
        ,[ThreadName]
        ,[ThreadId]
        ,[MachineName]
        ,[SessionId]
        ,[UserName])
    SELECT
        [CreationDate]
        ,[CorrelationId]
        ,[SourceType]
        ,[SourceName]
        ,[EventType]
        ,[EventName]
        ,[Category]
        ,[Message]
        ,[Context]
        ,[ElapsedTime]
        ,[ErrorCode]
        ,[Exception]
        ,[ExceptionType]
        ,[StackTrace]
        ,[ProcessName]
        ,[ProcessId]
        ,[ThreadName]
        ,[ThreadId]
        ,[MachineName]
        ,[SessionId]
        ,[UserName]
    FROM @TraceTable

END
GO;

GRANT EXECUTE ON OBJECT::[Monitoring].[InsertTraceEvents] TO [MonitoringLogger];
GO;