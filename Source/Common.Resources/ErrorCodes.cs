using System.Diagnostics.CodeAnalysis;

namespace Common.Resources
{
    /// <summary>
    ///     Error codes for the solution.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ErrorCodes
    {
        #region Constants

        public const int TaskError = 266; // Task exception
        public const int FirstChanceError = 255; // First chance .NET exception : may be swallowed
        public const int GlobalLevelError = 500; // Exception uncaught that bubbles up to the global exception handler

        #endregion

        #region Nested type: Framework

        public static class Framework
        {
            #region Constants

            // Base code for framework error codes: add this to your error code
            private const int CommonBaseCode = 1500;

            public const int ListenerInvalidConfiguration = CommonBaseCode + 01; // 1501
            public const int ListenerLoggingError = CommonBaseCode + 02; // 1502
            public const int ListenerFlooded = CommonBaseCode + 03; // 1503

            #endregion
        }

        public static class Application
        {
            #region Constants

            // Base code for database error codes: add this to your error code
            private const int CommonBaseCode = 2000;

            public const int InvalidConfiguration = CommonBaseCode + 01; // 2001
            public const int UnexpectedCodeCondition = CommonBaseCode + 02; // 2002
            public const int UnexpectedUserAction = CommonBaseCode + 03; // 2002

            #endregion
        }

        public static class Infrastructure
        {
            #region Constants

            // Base code for infrastructure error codes: add this to your error code
            private const int CommonBaseCode = 3000;

            public const int DatabaseAccessError = CommonBaseCode + 01;  //3001
            public const int ApiAccessError = CommonBaseCode + 02; //3002

            #endregion
        }

        public static class Database
        {
            #region Constants

            // Base code for database error codes: add this to your error code
            private const int CommonBaseCode = 4000;

            public const int NullNotExpected = CommonBaseCode + 01; // 4001
            public const int MissingData = CommonBaseCode + 02; // 4002

            #endregion
        }

        #endregion

    }
}
