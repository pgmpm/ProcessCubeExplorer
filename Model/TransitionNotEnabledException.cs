using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace pgmpm.Model
{
    [Serializable]
    public class TransitionNotEnabledException : ProcessModelException
    {
        public string message;
        public TransitionNotEnabledException(string message)
        {
            this.message = message;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Message", message);
        }
    }
}
