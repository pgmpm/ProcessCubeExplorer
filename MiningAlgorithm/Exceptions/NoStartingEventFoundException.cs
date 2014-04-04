using System;

namespace pgmpm.MiningAlgorithm.Exceptions
{
    public class NoStartingEventFoundException : Exception
    {
        public NoStartingEventFoundException()
            : base()
        {

        }

        public NoStartingEventFoundException(String message)
            : base(message)
        {

        }
    }
}
