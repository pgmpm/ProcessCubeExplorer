using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm;

namespace pgmpm.MiningAlgorithmTests
{
    [TestClass]
    public class EventNodeTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void EventNodeTest()
        {
            var event1 = new Event("A");
            var node = new EventNode(event1, 0.8);

            Assert.AreEqual("A", node.InnerEvent.Name);
            Assert.AreEqual(0.8, node.Adjacency);
            Assert.IsNotNull(node.ListOfFollowers);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void ToStringTest()
        {
            var event1 = new Event("A");
            var node = new EventNode(event1, 0.8);

            Assert.AreEqual("A", node.ToString());
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void FindNodeTest()
        {
            var event1 = new Event("A");
            var event2 = new Event("B");
            var event3 = new Event("C");

            var eventNode = new EventNode(event1, 0.8, new List<EventNode> { new EventNode(event2, 0.8, new List<EventNode> { new EventNode(event3, 0.8) }) });

            Assert.AreEqual("C", eventNode.FindNode("C").InnerEvent.Name);
            Assert.IsNull(eventNode.FindNode("D"));
        }
    }
}
