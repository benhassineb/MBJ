namespace Common.Resources
{
    /// <summary>
    ///     Trace constants names.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Les noms des paramètres de traces (techniques).
        /// </summary>
        public static class TraceParameter
        {
            public const string ErrorCode = nameof(ErrorCode);
            public const string FilePath = nameof(FilePath);
            public const string Method = nameof(Method);
            public const string Uri = nameof(Uri);
            public const string Origin = nameof(Origin);
            public const string Timeout = nameof(Timeout);
            public const string Field = nameof(Field);
            public const string Column = nameof(Column);
            public const string Name = nameof(Name);
            public const string Operation = nameof(Operation);
            public const string Function = nameof(Function);
            public const string Module = nameof(Module);
            public const string Action = nameof(Action);
        }

        /// <summary>
        /// Les noms de stratégies.
        /// </summary>
        public static class PolicyName
        {
            public const string CorsSameDomain = nameof(CorsSameDomain);
        }

    }
}