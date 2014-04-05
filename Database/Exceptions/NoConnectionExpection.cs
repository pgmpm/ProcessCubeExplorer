using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if a connection-error occurred.
    /// </summary>
    [Serializable]
    public class NoConnectionException : DBException
    {
        public NoConnectionException()
        {
        }

        public NoConnectionException(string message)
        {
            ExceptionMessage = message;
        }

        public NoConnectionException(String message, Exception inner)
            : base(message, inner)
        {
        }

        protected NoConnectionException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public override void SetMessage()
        {
            ExceptionMessage = "Could not establish a connection to the server.";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
