using System;

namespace pgmpm.Visualization.PetriNetVisualization
{
    class PetriNetVisualizerException : Exception
    {
        public PetriNetVisualizerException(string message)
            : base(message)
        {
        }
    }
}
