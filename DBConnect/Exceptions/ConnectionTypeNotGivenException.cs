using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if the connectionParams-object is missing the Type.
    /// </summary>
    [Serializable]
    public class ConnectionTypeNotGivenException : DBException
    {
        public ConnectionTypeNotGivenException()
        {
        }
        public ConnectionTypeNotGivenException(string message)
        {
            ExceptionMessage = message;
        }

        public ConnectionTypeNotGivenException(string message, Exception ex)
            : base(message, ex)
        {
        }

        protected ConnectionTypeNotGivenException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public override void SetMessage()
        {
            ExceptionMessage = "There was no connection Type given. Please specify one!";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
