using System;
using System.Collections.Generic;

namespace pgmpm.Model.PetriNet
{
    /// <summary>
    /// The superclass for each petri net node. Transition and place nodes have to extends this class!
    /// This class is used to handle transitions and places as equally in various situations.
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable]
    public abstract class Node
    {
        public DiffState DiffStatus = DiffState.Unchanged;


        /// <summary>
        /// Constructor: Create a new Node
        /// </summary>
        /// <param name="name">The name of the node</param>
        protected Node(String name = "")
        {
            Name = name;
            Visibility = true;
            Row = 1;
            IsDrawn = false;
            PositionX = 0;
            PositionY = 0;
        }

        public String NodeId { get; set; }
        public String Name { get; set; }
        public Boolean Visibility { get; set; }
        public abstract List<Node> OutgoingNodes { get; }
        public abstract List<Node> OutgoingNodesWithoutLoops { get; }
        public abstract List<Node> OutgoingLoops { get; }
        public abstract List<Node> IncomingNodes { get; }
        public abstract List<Node> IncomingNodesWithoutLoops { get; }
        public double Row { get; set; }
        public bool IsDrawn { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}