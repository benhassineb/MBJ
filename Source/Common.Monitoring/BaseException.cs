using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Common.Monitoring
{
    /// <summary>
    ///     The <see cref="BaseException" /> implements ErrorCode so that every concrete exceptions
    ///     (<see cref="InfrastructureException" />, <see cref="UnexpectedDataException" />, <see cref="ApplicationConfigurationException" />)
    ///     is mapped to a dedicated event log id to provide efficient supervision.
    /// </summary>
    [Serializable]
    public abstract class BaseException : Exception, IHasTraceContext
    {

        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseException" /> class with a specified error code and a specified
        ///     error message.
        /// </summary>
        /// <param name="errorCode">The error code that categorizes the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="parameters">The additional trace parameters.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference if no inner
        ///     exception is specified.
        /// </param>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        protected BaseException(int errorCode, string message, IEnumerable<TraceParameter> parameters = null, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            if (parameters != null && parameters.Any())
                TraceContext.Add(parameters);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseException" /> class with serialized data.
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
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The error code associated to the Exception.
        /// </summary>
        public int ErrorCode { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is a null reference (Nothing in
        ///     Visual Basic).
        /// </exception>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("errorCode", ErrorCode);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" PathDiscovery="*AllFiles*" />
        /// </PermissionSet>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Error code:{0}, detail:{1}", ErrorCode, base.ToString());
        }

        #endregion

        #region IHasTraceContext Members

        /// <summary>
        ///     The specified <see cref="IHasTraceContext.TraceContext" />.
        /// </summary>
        public TraceContext TraceContext => TraceContext.Create();

        #endregion
    }
}
