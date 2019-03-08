CREATE TABLE [Monitoring].[Trace] (
    [LogID]             INT              IDENTITY (1, 1) NOT NULL,
    [CreationDate]      DATETIME2 (7)    NOT NULL,
    [CorrelationId]     UNIQUEIDENTIFIER NOT NULL,
    [SourceType]        NVARCHAR (64)    NOT NULL,
    [SourceName]        NVARCHAR (64)    NOT NULL,
    [EventType]         NVARCHAR (64)    NOT NULL,
    [EventName]         NVARCHAR (256)   NULL,
    [Category]          INT              NOT NULL,
    [Message]           NVARCHAR (MAX)   NULL,
    [Context]           NVARCHAR (MAX)   NULL,
    [ElapsedTime]       TIME (7)         NULL,
    [ErrorCode]         INT              NULL,
    [Exception]         NVARCHAR (MAX)   NULL,
    [ExceptionType]     NVARCHAR (64)    NULL,
    [StackTrace]        NVARCHAR (MAX)   NULL,
    [ProcessName]       NVARCHAR (64)    NULL,
    [ProcessId]         INT              NULL,
    [ThreadName]        NVARCHAR (64)    NULL,
    [ThreadId]          INT              NULL,
    [MachineName]       NVARCHAR (64)    NULL,
    [SessionId]         NVARCHAR (64)    NULL,
    [UserName]          NVARCHAR (64)    NULL,
    CONSTRAINT [PK_MonitoringTrace] PRIMARY KEY CLUSTERED ([LogID] ASC)
);

GO
CREATE NONCLUSTERED INDEX [IX_Trace_CorrelationIdCategory]
    ON [Monitoring].[Trace]([CorrelationId] ASC, [Category] ASC);
