using pgmpm.Model.PetriNet;
using System.Collections.Generic;

namespace pgmpm.Visualization.PetriNetVisualization
{
    /// <summary>
    /// This class stores a column of the visualized PetriNet and contains a HashSet of Nodes that are displayed
    /// </summary>
    /// <author>Roman Bauer, Jannik Arndt</author>
    public class Column
    {
        public int ColumnNumber { get; set; }
        public HashSet<Node> HashSetOfNodes { get; set; }
        public bool ContainsAndTransitions = false;

        public Column(int number)
        {
            ColumnNumber = number;
            HashSetOfNodes = new HashSet<Node>();
        }

        public override string ToString()
        {
            return string.Join(", ", HashSetOfNodes);
        }
    }
}