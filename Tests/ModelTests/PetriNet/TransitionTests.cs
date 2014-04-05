using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ExampleData;
using pgmpm.Model.PetriNet;

namespace pgmpm.ModelTests.PetriNet
{
    /// <summary>
    /// Tests for the Transition-class
    /// </summary>
    /// <author>Jannik Arndt</author>
    [TestClass]
    public class TransitionTests
    {
        [TestMethod]
        public void FireTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start = testNet.AddPlace("Start", 1);
            Place end = testNet.AddPlace("End");
            Transition transition = testNet.AddTransition("Transition", incomingPlace: start, outgoingPlace: end);

            transition.Fire();

            Assert.AreEqual(1, end.Token);
            Assert.AreEqual(0, start.Token);
        }

        [TestMethod]
        public void TransitionTest()
        {
            Transition testTransition = new Transition();

            Assert.IsNotNull(testTransition.IncomingPlaces);
            Assert.IsNotNull(testTransition.OutgoingPlaces);
            Assert.IsFalse(testTransition.IsLoop);
            Assert.IsFalse(testTransition.IsANDJoin);
            Assert.IsFalse(testTransition.IsANDSplit);
        }

        [TestMethod]
        public void IsEnabled()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start1 = testNet.AddPlace("Start", 1);
            Place start2 = testNet.AddPlace("Start", 2);
            Transition transition = testNet.AddTransition("Transition", new List<Place> { start1, start2 });

            Assert.IsTrue(transition.IsEnabled);
        }

        [TestMethod]
        public void IsEnabledFail()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start1 = testNet.AddPlace("Start", 1);
            Place start2 = testNet.AddPlace("Start", 0);
            Transition transition = testNet.AddTransition("Transition", new List<Place> { start1, start2 });

            Assert.IsFalse(transition.IsEnabled);
        }

        /// <author>Thomas Meents</author>
        [TestMethod]
        public void GetTransitionLoopsTest()
        {
            Model.PetriNet.PetriNet pNet = PetriNetExample.PetriNetWithOneLoopThreePlacesAndThreeTransitions();
            List<String> transitionLoops = Transition.GetTransitionLoops(pNet);
            Assert.IsTrue(transitionLoops.Count == 1, "Loops=" + transitionLoops.Count);
        }

    }
}
