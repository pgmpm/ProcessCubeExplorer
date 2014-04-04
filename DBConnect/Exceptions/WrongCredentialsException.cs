using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Thrown if the Database reports wrong username or Password.
    /// </summary>
    [Serializable]
    public class WrongCredentialsException : DBException
    {
        public WrongCredentialsException()
        {
        }

        public WrongCredentialsException(string message)
        {
            ExceptionMessage = message;
        }

        public WrongCredentialsException(String message, Exception inner)
            : base(message, inner)
        {
            ExceptionMessage = message;
        }

        protected WrongCredentialsException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {

        }

        public override void SetMessage()
        {
            ExceptionMessage = "The username or Password is not correct.";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
