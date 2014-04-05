using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Model.PetriNet;

namespace pgmpm.ModelTests.PetriNet
{
    /// <summary>
    /// Tests for the Place-class
    /// </summary>
    /// <author>Jannik Arndt</author>
    [TestClass]
    public class PlaceTests
    {
        [TestMethod]
        public void PlaceTest()
        {
            Place testPlace = new Place();

            Assert.AreEqual(0, testPlace.Token);
            Assert.IsNotNull(testPlace.IncomingTransitions);
            Assert.IsNotNull(testPlace.OutgoingTransitions);

            Place testPlace2 = new Place("Name", 2);

            Assert.AreEqual(2, testPlace2.Token);
            Assert.AreEqual("Name", testPlace2.Name);
        }

        [TestMethod]
        public void ToStringTest1()
        {
            Place testPlace = new Place("Name");

            Assert.AreEqual("Name", testPlace.ToString());
        }

        [TestMethod]
        public void AppendIncomingTransitionTest()
        {
            Place testPlace = new Place();
            testPlace.IncomingTransitions.Add(new Transition("t1"));
            testPlace.OutgoingTransitions.Add(new Transition("t2"));

            Assert.AreEqual("(t1) => (t2)", testPlace.ToString());
        }

        [TestMethod]
        public void IsBeforeLoopingEventTest()
        {
            Place testPlace = new Place();
            testPlace.IncomingTransitions.Add(new Transition("t1", isLoop: true));

            Assert.IsTrue(testPlace.IsBeforeLoopingEvent);
        }

        [TestMethod]
        public void IsAfterLoopingEventTest()
        {
            Place testPlace = new Place();
            testPlace.OutgoingTransitions.Add(new Transition("t1", isLoop: true));

            Assert.IsTrue(testPlace.IsAfterLoopingEvent);
        }

        [TestMethod]
        public void OutgoingANDTransitionsTest()
        {
            Place testPlace = new Place();
            Transition t1 = new Transition("t1");
            Transition t2 = new Transition("t2");
            t1.IsANDJoin = true;
            t2.IsANDSplit = true;
            testPlace.OutgoingTransitions.Add(t1);
            testPlace.OutgoingTransitions.Add(t2);

            List<Transition> result = testPlace.OutgoingANDTransitions;

            Assert.IsTrue(result.Contains(t1));
            Assert.IsTrue(result.Contains(t2));
        }

        [TestMethod]
        public void OutgoingTransitionsWithoutANDsTest()
        {
            Place testPlace = new Place();
            Transition t1 = new Transition("t1");
            Transition t2 = new Transition("t2");
            Transition t3 = new Transition("t3");
            t1.IsANDJoin = true;
            t2.IsANDSplit = true;
            testPlace.OutgoingTransitions.Add(t1);
            testPlace.OutgoingTransitions.Add(t2);
            testPlace.OutgoingTransitions.Add(t3);

            List<Transition> result = testPlace.OutgoingTransitionsWithoutANDs;

            Assert.IsFalse(result.Contains(t1));
            Assert.IsFalse(result.Contains(t2));
            Assert.IsTrue(result.Contains(t3));
        }
    }
}
