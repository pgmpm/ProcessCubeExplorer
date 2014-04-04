using System.Linq;
using pgmpm.MatrixSelection.Fields;
using System.Collections.Generic;

namespace pgmpm.MiningAlgorithm
{
    /// <summary>
    /// Stores an event, its adjacency to the event before and a list of its followers.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public class EventNode
    {
        public Event InnerEvent { get; set; }
        public double Adjacency { get; set; }
        public List<EventNode> ListOfFollowers { get; set; }

        /// <summary>
        /// Constructor, which set the following parameter.
        /// </summary>
        /// <param name="innerEvent">The InnerEvent</param>
        /// <param name="adjacency">The Adjacency</param>
        /// <param name="listOfFollowers">List of followers</param>
        public EventNode(Event innerEvent, double adjacency, List<EventNode> listOfFollowers = null)
        {
            InnerEvent = innerEvent;
            Adjacency = adjacency;
            ListOfFollowers = listOfFollowers ?? new List<EventNode>();
        }

        /// <summary>
        /// Overrides the method ToString()
        /// </summary>
        /// <returns>The name of the innerEvent</returns>
        public override string ToString()
        {
            return InnerEvent.Name;
        }

        /// <summary>
        /// Recursively finds the node with the given name
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>The located EventNode by name </returns>
        /// <author>Jannik Arndt</author>
        public EventNode FindNode(string name)
        {
            if (InnerEvent.Name == name)
                return this;
            return ListOfFollowers.Select(eventNode => eventNode.FindNode(name)).FirstOrDefault(temp => temp != null);
        }
    }
}