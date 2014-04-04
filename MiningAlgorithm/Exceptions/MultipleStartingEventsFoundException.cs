using System;

namespace pgmpm.MiningAlgorithm.Exceptions
{
    public class MultipleStartingEventsFoundException : Exception
    {
        public MultipleStartingEventsFoundException()
            : base()
        { }

        public MultipleStartingEventsFoundException(String message)
            : base(message)
        { }
    }
}
