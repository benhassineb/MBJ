using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Common.Monitoring
{
    /// <summary>
    ///     Throw <see cref="ApplicationConfigurationException" /> when configuration issues are faced.
    /// </summary>
    /// <remarks>
    ///     Throwing <see cref="ApplicationConfigurationException" /> helps to quickly identify configuration issues rather than generic exception
    ///     like
    ///     ObjectNullReference:
    ///     <code>if (_connectionString == null) throw new ConfigurationException(ErrorCode.EmptyConnectionString, usefullInfo)</code>
    /// </remarks>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Error code is required.")]
    public sealed class ApplicationConfigurationException : BaseException
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationConfigurationException" /> class with a specified error code, error
        ///     message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="errorCode">The error code that categorizes the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="parameters">The additional trace parameters.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference if no inner
        ///     exception is specified.
        /// </param>
        public ApplicationConfigurationException(int errorCode, string message, IEnumerable<TraceParameter> parameters = null, Exception innerException = null)
            : base(errorCode, message, parameters, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InfrastructureException" /> class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        ///     The class name is null or
        ///     <see cref="System.Exception.HResult" /> is zero (0).
        /// </exception>
        private ApplicationConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
