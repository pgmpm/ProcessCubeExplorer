using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ExampleData;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm;
using pgmpm.Model.PetriNet;

namespace pgmpm.MiningAlgorithmTests
{
    /// <summary>
    /// Unit-Tests for the HeuristicMiner
    /// </summary>
    /// <author>Jannik Arndt</author>
    [TestClass]
    public class HeuristicMinerTests
    {
        /// <summary>
        /// Unit-Tests for the method ProcessModel Mine(Field field, double threshold, int max depth, string Name)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MineTest1()
        {
            MinerSettings.AddOrUpdateKey("AdjacencyThresholdSlider", 51);
            MinerSettings.AddOrUpdateKey("MaximumRecursionDepthSlider", 10);
            var field = new Field { EventLog = EventLogExample.ThreeCaseEventLog()};
            var miner = new HeuristicMiner(field);
            PetriNet actual = (PetriNet)miner.Mine();

            Assert.AreEqual(1, actual.Transitions.Count);
            Assert.AreEqual("A", actual.Transitions[0].Name);
            Assert.AreEqual(2, actual.Places.Count);
            Assert.AreEqual("End", actual.Places[1].Name);
        }

        /// <summary>
        /// Unit-Tests for the method ProcessModel Mine(Field field)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MineTest2()
        {
            MinerSettings.AddOrUpdateKey("AdjacencyThresholdSlider", 51);
            MinerSettings.AddOrUpdateKey("MaximumRecursionDepthSlider", 10);
            var field = new Field { EventLog = EventLogExample.ComplexEventLogVanDerAalst() };
            var miner = new HeuristicMiner(field);
            PetriNet actual = (PetriNet)miner.Mine();

            Assert.AreEqual(33, actual.Transitions.Count);
            Assert.AreEqual("D", actual.Transitions[2].Name);
            Assert.AreEqual(44, actual.Places.Count);
         //   Assert.AreEqual("End", actual.Places[].Name);
        }

        /// <summary>
        /// Unit-Tests for the method ProcessModel Mine(Field field)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MineTestFail1()
        {
            try
            {
                var miner = new HeuristicMiner(null);
                miner.Mine();
                Assert.Fail();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(Exception));
            }
        }

        /// <summary>
        /// Unit-Tests for the method ProcessModel Mine(Field field)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MineTestFail2()
        {
            try
            {
                MinerSettings.AddOrUpdateKey("AdjacencyThresholdSlider", 51);
                var field = new Field();
                var miner = new HeuristicMiner(field);
                miner.Mine();
                Assert.Fail();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(Exception));
            }
        }

        /// <summary>
        /// Unit-Tests for the method ProcessModel Mine(Field field)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void MineTestFail3()
        {
            try
            {
                MinerSettings.AddOrUpdateKey("MaximumRecursionDepthSlider", 10);
                var field = new Field();
                var miner = new HeuristicMiner(field);
                miner.Mine();
                Assert.Fail();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(Exception));
            }
        }

        /// <summary>
        /// Unit-Tests for the method double[,] CreateAdjacencyMatrix(Field field)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void CreateAdjacencyMatrixTest()
        {
            var field = new Field { EventLog = EventLogExample.ThreeCaseEventLog() };
            var miner = new HeuristicMiner(field);
            var actual = miner.CreateAdjacencyMatrix(field);

            var expected = new[,] { { 0.0, 0.5, 0.5, 0.0, 0.5 }, { -0.5, 0.0, 0.0, 0.5, 0.0 }, { -0.5, 0.0, 0.0, 0.5, 0.0 }, { 0.0, -0.5, -0.5, 0.0, -0.5 }, { -0.5, 0.0, 0.0, 0.5, 0.0 } };

            for (var i = 0; i < 5; i++)
                for (var j = 0; j < 5; j++)
                    Assert.AreEqual(expected[i, j], actual[i, j]);

        }

        /// <summary>
        /// Unit-Tests for the method double GetAdjacency(Event event1, Event event2, EventLog log)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void GetAdjacencyTest()
        {
            var field = new Field { EventLog = EventLogExample.ThreeCaseEventLog() };
            var miner = new HeuristicMiner(field);
            var event1 = new Event("A");
            var event2 = new Event("B");

            var actual = miner.GetAdjacency(event1, event2, field.EventLog);

            Assert.AreEqual(0.5, actual);
        }

        /// <summary>
        /// Unit-Tests for the method EventNode CreateDependencyGraph()
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void CreateDependencyGraphTest()
        {
            var field = new Field { EventLog = EventLogExample.ThreeCaseEventLog() };
            var miner = new HeuristicMiner(field);
            var event1 = new Event("A");
            var event2 = new Event("B");
            var event3 = new Event("C");
            var eventList = new List<Event> { event1, event2, event3 };
            var matrix = new[,] { { 0.0, 0.8, 0.0 }, { 0.0, 0.0, 0.8 }, { 0.0, 0.0, 0.0 } };

            var actual = miner.CreateDependencyGraph(eventList, matrix, 0.5, 0.0, 0, 10);

            var expected = new EventNode(event1, 0.8, new List<EventNode> { new EventNode(event2, 0.8, new List<EventNode> { new EventNode(event3, 0.8) }) });

            Assert.AreEqual(expected.InnerEvent, actual.InnerEvent);
            Assert.AreEqual(expected.ListOfFollowers[0].InnerEvent, actual.ListOfFollowers[0].InnerEvent);
            Assert.AreEqual(expected.ListOfFollowers[0].ListOfFollowers[0].InnerEvent, actual.ListOfFollowers[0].ListOfFollowers[0].InnerEvent);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void CalculateStandardDeviationTest()
        {
            var field = new Field { EventLog = EventLogExample.ThreeCaseEventLog() };
            var miner = new HeuristicMiner(field);
            var values = new List<double> { 0.2, 0.4, 0.6, 0.8 };

            var actual = miner.CalculateStandardDeviation(values);

            Assert.AreEqual(0.258, actual);
        }

        /// <summary>
        /// Unit-Tests for the method bool IsAndRelation(Event a, Event b, Event c, EventLog log)
        /// </summary>
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void IsAndRelationTest()
        {
            var eventLog = EventLogExample.ComplexEventLogVanDerAalst();
            var field = new Field { EventLog = eventLog };
            var miner = new HeuristicMiner(field);
            var event1 = eventLog.Cases[0].EventList[0];
            var event2 = eventLog.Cases[0].EventList[1];
            var event3 = eventLog.Cases[0].EventList[2];

            var actual = miner.IsAndRelation(event1, event2, event3, eventLog);

            Assert.IsTrue(actual);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void FindFirstCommonSuccessorTest()
        {
            var eventLog = EventLogExample.ComplexEventLogVanDerAalst();
            var field = new Field { EventLog = eventLog };
            var miner = new HeuristicMiner(field);

            var event1 = eventLog.Cases[0].EventList[0];
            var event2 = eventLog.Cases[0].EventList[1];
            var event3 = eventLog.Cases[0].EventList[2];

            var eventNode1 = new EventNode(event1, 0.8, new List<EventNode> { new EventNode(event2, 0.8, new List<EventNode> { new EventNode(event3, 0.8) }) });
            var eventNode2 = new EventNode(event1, 0.8, new List<EventNode> { new EventNode(event2, 0.8, new List<EventNode> { new EventNode(event3, 0.8) }) });

            var actual = miner.FindFirstCommonSuccessor(new List<EventNode> { eventNode1, eventNode2 });

            Assert.AreEqual("C", actual);
        }
    }
}
