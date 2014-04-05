///// <author>Jannik Arndt</author>
//using System;
//using System.Security.Permissions;
//using System.Runtime.Serialization;

////TODO Ganze Klasse raus?
//namespace pgmpm.Database.Exceptions
//{
//    /// <summary>
//    /// Thrown if the Database reports a warning.
//    /// </summary>
//    [Serializable]
//    public class WarningException : DBException
//    {
//        public override void SetMessage()
//        {
//            ExceptionMessage = "Warning";
//        }

//        public WarningException()
//        {
//        }

//        public WarningException(string message)
//        {
//            this.ExceptionMessage = message;
//        }

//        public WarningException(String message, Exception inner)
//            : base(message, inner)
//        {
//        }

//        protected WarningException(SerializationInfo si, StreamingContext sc)
//            : base(si, sc)
//        {
//        }

//        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
//        public override void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            base.GetObjectData(info, context);

//            info.AddValue("Message", ExceptionMessage);
//        }
//    }
//}
