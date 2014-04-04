using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if the User tries to Open a Database that does not exist on the system.
    /// </summary>
    [Serializable]
    public class DatabaseDoesNotExist : DBException
    {
        public DatabaseDoesNotExist()
        {
        }

        public DatabaseDoesNotExist(string message)
        {
            ExceptionMessage = message;
        }

        public DatabaseDoesNotExist(String message, Exception inner)
            : base(message, inner)
        {
        }

        protected DatabaseDoesNotExist(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public override void SetMessage()
        {
            ExceptionMessage = "The specified Database does not exist.";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
