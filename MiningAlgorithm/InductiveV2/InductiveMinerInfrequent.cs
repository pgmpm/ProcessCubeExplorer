using pgmpm.MatrixSelection.Fields;
using pgmpm.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pgmpm.MiningAlgorithm.InductiveV2
{
    /// <summary>
    /// Implementation of the improved b' algorithm(Inductive Miner infrequent)
    /// </summary>
    /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>  
    public class InductiveMinerInfrequent : InductiveMiner
    {

        /// <summary>
        /// Initializes the InductiveMiner instance.
        /// </summary>
        /// <param name="field">Corresponding field</param>
        /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
        public InductiveMinerInfrequent(Field field)
            : base(field)
        {
        }

        /// <summary>
        /// Starts the mining process
        /// </summary>
        /// <returns>Process model</returns>
        /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
        public override ProcessModel Mine()
        {
            if (_field == null)
                throw new ArgumentNullException("field", "The field parameter was null");

            var processingTimeStart = Process.GetCurrentProcess().TotalProcessorTime;

            DirectFrequency();
            EventualFrequency();

            EventDictionary = EliminateInfrequentFirstListBuild(EventDictionary);

            BuildInitialDirectFollowGraph();
            BuildInitialEventualFollowGraph();

            foreach (KeyValuePair<Event, InductiveMinerGraphNode> pair in EventDictionary)
            {
                pair.Value.ReBuildeEventualFollower(null);
                pair.Value.CleanUpHelperList(null);
            }

            IMTree = new InductiveMinerTreeNode(IMPetriNet, EventDictionary[StartEvent], StartEvent);

            GeneratePetriNet();

            _field.ProcessModel = IMPetriNet;

            EndLog(processingTimeStart);
            return _field.ProcessModel;
        }

        /// <summary>
        /// Eliminates infrequent connections between nodes.
        /// </summary>
        /// <param name="workingDictionary">Dictionary to be analyses</param>
        /// <returns>Filtered dictionary</returns>
        /// <author>Bernd Nottbeck</author>
        private Dictionary<Event, InductiveMinerGraphNode> EliminateInfrequentFirstListBuild(Dictionary<Event, InductiveMinerGraphNode> workingDictionary)
        {
            foreach (KeyValuePair<Event, InductiveMinerGraphNode> pair in workingDictionary)
            {
                pair.Value.EliminateInfrequent();
            }
            return workingDictionary;
        }
    }
}
