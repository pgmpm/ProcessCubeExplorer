/// <author>Jannik Arndt</author>
using System;
using System.Security.Permissions;
using System.Runtime.Serialization;


namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if the User tries to Open a Database that does not exist on the system.
    /// </summary>
    [Serializable]
    public class DatebaseDoesNotExist : DBException
    {

        public override void SetMessage()
        {
            ExceptionMessage = "The specified Database does not exist.";
        }

        public DatebaseDoesNotExist()
        {
        }

        public DatebaseDoesNotExist(string message)
        {
            this.ExceptionMessage = message;
        }

        public DatebaseDoesNotExist(String message, Exception inner)
            : base(message, inner)
        {
        }

        protected DatebaseDoesNotExist(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Message", ExceptionMessage);
        }
    }
}
