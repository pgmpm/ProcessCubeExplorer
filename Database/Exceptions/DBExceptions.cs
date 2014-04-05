using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Database.Exceptions
{
    /// <summary>
    /// Exception that functions as an abstraction layer between control-elements and various Database-types.
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable]
    public class DBException : Exception
    {
        public DBException()
        {
            SetMessage();
        }
        public DBException(String message)
            : base(message)
        {
            ExceptionMessage = message;
        }

        public DBException(String message, Exception inner)
            : base(message, inner)
        {
            SetMessage();
        }

        protected DBException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
            ExceptionMessage = (String)si.GetValue("ExceptionMessage", typeof(string));
        }

        public string ExceptionMessage { set; get; }

        public virtual void SetMessage()
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ExceptionMessage", ExceptionMessage, ExceptionMessage.GetType());
        }

    }








}
