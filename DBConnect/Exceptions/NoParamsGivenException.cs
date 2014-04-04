using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if the connectionParams-object is missing parameters.
    /// </summary>
    [Serializable]
    public class NoParamsGivenException : DBException
    {
        public NoParamsGivenException()
        {
        }

        public NoParamsGivenException(string message)
        {
            ExceptionMessage = message;
        }

        public NoParamsGivenException(string message, Exception ex)
            : base(message, ex)
        {
        }

        protected NoParamsGivenException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public override void SetMessage()
        {
            ExceptionMessage = "No connection-parameters were given.";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
