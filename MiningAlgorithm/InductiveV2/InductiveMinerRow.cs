namespace pgmpm.MiningAlgorithm.InductiveV2
{
    /// <summary>
    /// A row stores information on how often an eventNode is followed by another node.
    /// </summary>
    public class InductiveMinerRow
    {
        /// <summary>
        /// From event
        /// </summary>
        public InductiveMinerGraphNode FromNode { get; set; }

        /// <summary>
        /// To event
        /// </summary>
        public InductiveMinerGraphNode ToNode { get; set; }

        /// <summary>
        /// Occurrence
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// Constructor from to node
        /// </summary>
        /// <param name="node">from node</param>
        /// <param name="follower">to node</param>
        public InductiveMinerRow(InductiveMinerGraphNode node, InductiveMinerGraphNode follower)
        {
            FromNode = node;
            ToNode = follower;
            Count = 1;
        }
    }
}