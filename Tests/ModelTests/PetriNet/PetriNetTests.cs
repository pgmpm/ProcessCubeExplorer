using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Model.PetriNet;

namespace pgmpm.ModelTests.PetriNet
{
    /// <summary>
    /// Tests für the PetriNet-Class
    /// </summary>
    /// <author>Jannik Arndt</author>
    [TestClass]
    public class PetriNetTests
    {
        [TestMethod]
        public void PetriNetTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");

            Assert.IsNotNull(testNet.Places);
            Assert.IsNotNull(testNet.Transitions);
            Assert.AreEqual("TestNet", testNet.Name);
        }

        [TestMethod]
        public void IsOfKindTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");

            Assert.AreEqual("PetriNet", testNet.IsOfKind());
        }

        [TestMethod]
        public void AddPlaceTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");

            Place testPlace = testNet.AddPlace("TestPlace", 2);

            Assert.IsNotNull(testPlace);
            Assert.IsInstanceOfType(testPlace, typeof(Place));
            Assert.AreEqual(testNet.Places.First(), testPlace);
            Assert.AreEqual(2, testPlace.Token);
        }

        [TestMethod]
        public void GetPlaceByNameTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place testPlace = testNet.AddPlace("TestPlace", 2);

            Place theSamePlace = testNet.GetPlaceByName("TestPlace");

            Assert.AreEqual(theSamePlace, testPlace);
        }

        [TestMethod]
        public void GetPlacesWithoutFollowersTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place testPlace1 = testNet.AddPlace("TestPlace1", 2);
            Place testPlace2 = testNet.AddPlace("TestPlace2", 3);
            Place testPlace3 = testNet.AddPlace("TestPlace3", 4);
            testPlace1.AppendOutgoingTransition(new Transition());
            testPlace3.AppendOutgoingTransition(new Transition());

            List<Place> testlist = testNet.GetPlacesWithoutFollowers();

            Assert.AreEqual(testlist.First(), testPlace2);
        }

        [TestMethod]
        public void AddTransitionTestNoPlaces()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");

            Transition testTransition = testNet.AddTransition("TestTransition");

            Assert.IsNotNull(testTransition);
            Assert.IsInstanceOfType(testTransition, typeof(Transition));
            Assert.AreEqual(testNet.Transitions.First(), testTransition);
            Assert.IsNotNull(testTransition.IncomingPlaces);
            Assert.IsNotNull(testTransition.OutgoingPlaces);
        }

        [TestMethod]
        public void AddTransitionTestSinglePlaces()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place testPlace1 = testNet.AddPlace("TestPlace1", 2);
            Place testPlace2 = testNet.AddPlace("TestPlace2", 3);

            Transition testTransition = testNet.AddTransition("TestTransition", incomingPlace: testPlace1, outgoingPlace: testPlace2);

            Assert.AreEqual(testPlace1, testTransition.IncomingPlaces.First());
            Assert.AreEqual(testPlace2, testTransition.OutgoingPlaces.First());
        }

        [TestMethod]
        public void AddTransitionTestPlaceLists()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place testPlace1 = testNet.AddPlace("TestPlace1", 2);
            Place testPlace2 = testNet.AddPlace("TestPlace2", 3);
            Place testPlace3 = testNet.AddPlace("TestPlace3", 2);
            Place testPlace4 = testNet.AddPlace("TestPlace4", 3);
            List<Place> list1 = new List<Place> { testPlace1, testPlace2 };
            List<Place> list2 = new List<Place> { testPlace3, testPlace4 };

            Transition testTransition = testNet.AddTransition("TestTransition", list1, list2);

            Assert.AreEqual(testPlace1, testTransition.IncomingPlaces.First());
            Assert.AreEqual(testPlace3, testTransition.OutgoingPlaces.First());
        }

        [TestMethod]
        public void FindTransitionTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition = testNet.AddTransition("TestTransition");

            Transition transition2 = testNet.FindTransition("TestTransition");

            Assert.AreEqual(transition2, testTransition);
        }

        [TestMethod]
        public void FindTransitionTestNotFound()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place tempPlaceIn = testNet.AddPlace("place_1");
            Place tempPlaceOut = testNet.AddPlace("place_2");
            testNet.AddTransition("transition_1", incomingPlace: tempPlaceIn, outgoingPlace: tempPlaceOut);

            Assert.IsNull(testNet.FindTransition("not in the list"));
        }

        [TestMethod]
        public void CountTransitionsWithoutANDsTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            testNet.AddTransition("TestTransition1");
            testNet.AddTransition("AND");
            testNet.AddTransition();
            testNet.AddTransition("TestTransition4");

            int result = testNet.CountTransitionsWithoutANDs();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetTransitionsWithoutFollowersTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            Transition testTransition3 = testNet.AddTransition("TestTransition3");
            testNet.AddTransition("TestTransition4");
            testTransition1.AddOutgoingPlace(new Place());
            testTransition2.AddOutgoingPlace(new Place());

            List<Transition> testList = testNet.GetTransitionsWithoutFollowers();

            Assert.IsNotNull(testList);
            Assert.AreEqual(testTransition3, testList.First());
            Assert.AreEqual(2, testList.Count);
        }

        [TestMethod]
        public void GetTransitionsWithoutFollowersIgnoreLoopsTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            Transition testTransition3 = testNet.AddTransition("TestTransition3");
            testNet.AddTransition("TestTransition4");
            testTransition1.AddOutgoingPlace(new Place());
            testTransition2.AddOutgoingPlace(new Place());
            Place testPlace = new Place();
            testTransition3.AddOutgoingPlace(testPlace);
            testPlace.AppendOutgoingTransition(new Transition("", isLoop: true));

            List<Transition> testList = testNet.GetTransitionsWithoutFollowersIgnoreLoops();

            Assert.IsNotNull(testList);
            Assert.AreEqual(testTransition3, testList.First());
            Assert.AreEqual(2, testList.Count);
        }

        [TestMethod]
        public void GetNodesTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition = testNet.AddTransition("TestTransition");
            Place testPlace = testNet.AddPlace("TestPlace");

            List<Node> list = testNet.Nodes;

            Assert.IsTrue(list.Contains(testTransition));
            Assert.IsTrue(list.Contains(testPlace));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void GetSinksTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            testTransition2.AddOutgoingPlace(new Place());
            Place testPlace1 = testNet.AddPlace("TestPlace1");
            Place testPlace2 = testNet.AddPlace("TestPlace2");
            testPlace2.AppendOutgoingTransition(testTransition1);

            List<Node> list = testNet.GetSinks();

            Assert.IsTrue(list.Contains(testTransition1));
            Assert.IsTrue(list.Contains(testPlace1));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void GetSourcesTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            testTransition2.AddIncomingPlace(new Place());
            Place testPlace1 = testNet.AddPlace("TestPlace1");
            Place testPlace2 = testNet.AddPlace("TestPlace2");
            testPlace2.AppendIncomingTransition(testTransition1);

            List<Node> list = testNet.GetSources();

            Assert.IsTrue(list.Contains(testTransition1));
            Assert.IsTrue(list.Contains(testPlace1));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void RemoveNodeTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            Place testPlace1 = testNet.AddPlace("TestPlace1");
            Place testPlace2 = testNet.AddPlace("TestPlace2");

            testNet.RemoveNode(testTransition1);
            testNet.RemoveNode(testPlace1);

            Assert.AreEqual(1, testNet.Transitions.Count);
            Assert.AreEqual(1, testNet.Places.Count);
            Assert.AreEqual(testTransition2, testNet.Transitions.First());
            Assert.AreEqual(testPlace2, testNet.Places.First());
        }

        [TestMethod]
        public void RemoveNodesTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            Transition testTransition2 = testNet.AddTransition("TestTransition2");
            Place testPlace1 = testNet.AddPlace("TestPlace1");
            Place testPlace2 = testNet.AddPlace("TestPlace2");
            List<Node> nodesToRemove = new List<Node> { testTransition1, testPlace1 };

            testNet.RemoveNodes(nodesToRemove);

            Assert.AreEqual(1, testNet.Transitions.Count);
            Assert.AreEqual(1, testNet.Places.Count);
            Assert.AreEqual(testTransition2, testNet.Transitions.First());
            Assert.AreEqual(testPlace2, testNet.Places.First());
        }

        [TestMethod]
        public void RemoveNodeAndConnectionsTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place testPlace1 = testNet.AddPlace("TestPlace1");
            Place testPlace2 = testNet.AddPlace("TestPlace2");
            Place testPlace3 = testNet.AddPlace("TestPlace3");
            Transition testTransition1 = testNet.AddTransition("TestTransition1", incomingPlace: testPlace1, outgoingPlace: testPlace2);
            Transition testTransition2 = testNet.AddTransition("TestTransition2", incomingPlace: testPlace2, outgoingPlace: testPlace3);

            testNet.RemoveNodeAndConnections(testPlace2);

            Assert.AreEqual(0, testTransition1.OutgoingPlaces.Count);
            Assert.AreEqual(0, testTransition2.IncomingPlaces.Count);
            Assert.IsFalse(testNet.Places.Contains(testPlace2));

            testNet.RemoveNodeAndConnections(testTransition2);

            Assert.AreEqual(0, testPlace3.IncomingTransitions.Count);
        }

        [TestMethod]
        public void CountLoopsTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Transition testTransition1 = testNet.AddTransition("TestTransition1");
            testNet.AddTransition("TestTransition2");
            Transition testTransition3 = testNet.AddTransition("TestTransition3");
            testNet.AddTransition("TestTransition4");
            testTransition1.IsLoop = true;
            testTransition3.IsLoop = true;

            int result = testNet.CountLoops();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void AddLoopTestNoParameter()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");

            testNet.AddLoop();

            Assert.AreEqual(0, testNet.Transitions.Count);
            Assert.AreEqual(0, testNet.Places.Count);
        }

        [TestMethod]
        public void AddLoopTestGivenNameEasyTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start = testNet.AddPlace("Start");
            Place end = testNet.AddPlace("End");
            testNet.AddTransition("TestTransition", incomingPlace: start, outgoingPlace: end);

            testNet.AddLoop("TestTransition");

            Assert.AreEqual(2, testNet.Transitions.Count);
            Assert.AreEqual(2, testNet.Places.Count);
            Assert.IsTrue(testNet.Transitions[1].IsLoop);
        }

        [TestMethod]
        public void AddLoopTestGivenTransitionHardTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start = testNet.AddPlace("Start");
            Place midLeft1 = testNet.AddPlace("MidLeft1");
            Place midLeft2 = testNet.AddPlace("MidLeft2");
            Place midLeft3 = testNet.AddPlace("MidLeft3");
            Place midRight1 = testNet.AddPlace("MidRight1");
            Place midRight2 = testNet.AddPlace("MidRight2");
            Place midRight3 = testNet.AddPlace("MidRight3");
            Place end = testNet.AddPlace("End");
            testNet.AddTransition("Left", incomingPlace: start, outgoingPlaces: new List<Place> { midLeft1, midLeft2, midLeft3 });
            Transition testTransition = testNet.AddTransition("TestTransition", new List<Place> { midLeft1, midLeft2, midLeft3 }, new List<Place> { midRight1, midRight2, midRight3 });
            testNet.AddTransition("Right", new List<Place> { midRight1, midRight2, midRight3 }, outgoingPlace: end);

            testNet.AddLoop(transition: testTransition);

            Assert.AreEqual(6, testNet.Transitions.Count);
            Assert.AreEqual(10, testNet.Places.Count);
            Assert.IsTrue(testNet.Transitions[3].IsLoop);
            Assert.AreEqual(testTransition, midLeft1.OutgoingTransitions[0].OutgoingPlaces[0].OutgoingTransitions[0]);
        }

        [TestMethod]
        public void ConvertToDotTest()
        {
            Model.PetriNet.PetriNet testNet = new Model.PetriNet.PetriNet("TestNet");
            Place start = testNet.AddPlace("Start");
            Place midLeft1 = testNet.AddPlace("MidLeft1");
            Place midLeft2 = testNet.AddPlace("MidLeft2");
            Place midLeft3 = testNet.AddPlace("MidLeft3");
            Place midRight1 = testNet.AddPlace("MidRight1");
            Place midRight2 = testNet.AddPlace("MidRight2");
            Place midRight3 = testNet.AddPlace("MidRight3");
            Place end = testNet.AddPlace("End");
            testNet.AddTransition("Left", incomingPlace: start, outgoingPlaces: new List<Place> { midLeft1, midLeft2, midLeft3 });
            testNet.AddTransition("TestTransition", new List<Place> { midLeft1, midLeft2, midLeft3 }, new List<Place> { midRight1, midRight2, midRight3 });
            testNet.AddTransition("Right", new List<Place> { midRight1, midRight2, midRight3 }, outgoingPlace: end);

            String dotCode = testNet.ConvertToDot();

            Assert.IsNotNull(dotCode);
        }

        [TestMethod]
        public void MergeWithPetriNetTest()
        {
            Model.PetriNet.PetriNet testNet1 = new Model.PetriNet.PetriNet("TestNet");
            Place start1 = testNet1.AddPlace("Start");
            Place end1 = testNet1.AddPlace("End");
            Transition trans1 = testNet1.AddTransition("Trans1", incomingPlace: start1, outgoingPlace: end1);

            Model.PetriNet.PetriNet testNet2 = new Model.PetriNet.PetriNet("TestNet");
            Place start2 = testNet2.AddPlace("Start");
            Place end2 = testNet2.AddPlace("End");
            Transition trans2 = testNet2.AddTransition("Trans2", incomingPlace: start2, outgoingPlace: end2);

            testNet1.MergeWithPetriNet(testNet2, trans1);

            Assert.AreEqual(4, testNet1.Places.Count);
            Assert.AreEqual(2, testNet1.Transitions.Count);
            Assert.AreEqual(2, trans1.OutgoingPlaces.Count);
            Assert.AreEqual(trans2, trans1.OutgoingPlaces[1].OutgoingTransitions[0]);
        }
    }
}
