using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Model;
using pgmpm.Model.PetriNet;

namespace pgmpm.Diff.DiffAlgorithm.Tests
{
    [TestClass()]
    public class SnapshotDiffTests
    {
        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void CompareProcessModelsTest()
        {
            IDifference diff = DiffFactory.CreateDifferenceObject<SnapshotDiff>();

            PetriNet onetwothreefourfive = ExampleData.PetriNetExample.OneTwoThreeFourFive();
            PetriNet onethreefive = ExampleData.PetriNetExample.OneThreeFive();
            List<ProcessModel> listOfProcessModels = new List<ProcessModel> { onetwothreefourfive, onethreefive };
            //ProcessModel resultModel = SnapshotDiff.CompareProcessModels(listOfProcessModels);
            ProcessModel resultModel = diff.CompareProcessModels(listOfProcessModels);
            PetriNet result = resultModel as PetriNet;

            Assert.IsTrue(result != null && result.Transitions.Count(transition => transition.DiffStatus == DiffState.Added) == 0);
            Assert.IsTrue(result != null && result.Transitions.Count(transition => transition.DiffStatus == DiffState.Changed) == 0);
            Assert.IsTrue(result != null && result.Transitions.Count(transition => transition.DiffStatus == DiffState.Deleted) == 2);
            Assert.IsTrue(result != null && result.Transitions.Count(transition => transition.DiffStatus == DiffState.Unchanged) == 3);
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void CompareProcessModelsTest1()
        {
            IDifference diff = DiffFactory.CreateDifferenceObject<SnapshotDiff>();
            PetriNet onetwothreefourfive = ExampleData.PetriNetExample.OneTwoThreeFourFive();
            PetriNet onethreefive = ExampleData.PetriNetExample.OneThreeFive();
            List<ProcessModel> listOfProcessModels = new List<ProcessModel> { onetwothreefourfive, onethreefive };
            //ProcessModel resultModel = SnapshotDiff.CompareProcessModels(listOfProcessModels);
            ProcessModel resultModel = diff.CompareProcessModels(listOfProcessModels);
            PetriNet result = resultModel as PetriNet;

            Assert.IsTrue(result != null && result.Transitions.Count == 5, "Transitions: " + result.Transitions.Count);
            Assert.IsTrue(result != null && result.Places.Count == 6, "Places: " + result.Places.Count);
        }

        /*
        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void FindDifferencesTest()
        {
            PetriNet onetwothreefourfive = ExampleData.PetriNetExample.OneTwoThreeFourFive();
            PetriNet onethreefive = ExampleData.PetriNetExample.OneThreeFive();
            List<PetriNet> listOfProcessModels = new List<PetriNet> { onetwothreefourfive, onethreefive };
            List<Transition> listFirstTransitions = listOfProcessModels.First().Transitions;
            List<Transition> listSecondTransitions = listOfProcessModels.Last().Transitions;

            SnapshotDiff.FindDifferences(listFirstTransitions, listSecondTransitions);

            Assert.IsTrue(listFirstTransitions.Count(transition => transition.DiffStatus == DiffState.Deleted) == 2);

            Assert.IsTrue(listSecondTransitions.Count(transition => transition.DiffStatus == DiffState.Added) == 0);
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void AddAddedTransitionsTest()
        {
            PetriNet onetwothreefourfive = ExampleData.PetriNetExample.OneTwoThreeFourFive();
            PetriNet onethreefive = ExampleData.PetriNetExample.OneThreeFive();
            List<PetriNet> listOfProcessModels = new List<PetriNet> { onetwothreefourfive, onethreefive };
            List<Transition> listFirstTransitions = listOfProcessModels.First().Transitions;
            List<Transition> listSecondTransitions = listOfProcessModels.Last().Transitions;

            SnapshotDiff.FindDifferences(listSecondTransitions, listFirstTransitions);

            SnapshotDiff.AddAddedTransitions(onethreefive);

            Assert.IsTrue(onethreefive.Transitions.Count == 3, "Transitions: " + onethreefive.Transitions.Count);
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void FixEndingTest()
        {
            PetriNet onetwothreefourfive = ExampleData.PetriNetExample.OneTwoThreeFourFive();
            PetriNet onethreefive = ExampleData.PetriNetExample.OneThreeFive();
            List<PetriNet> listOfProcessModels = new List<PetriNet> { onetwothreefourfive, onethreefive };
            List<Transition> listFirstTransitions = listOfProcessModels.First().Transitions;
            List<Transition> listSecondTransitions = listOfProcessModels.Last().Transitions;

            SnapshotDiff.FindDifferences(listSecondTransitions, listFirstTransitions);

            SnapshotDiff.AddAddedTransitions(onethreefive);

            //modify the first and second (of three) places within the petri net
            onethreefive.Places[0].Name = "End";
            onethreefive.Places[1].Name = "End";

            SnapshotDiff.FixEnding(onethreefive);
            Assert.IsTrue(onethreefive.Places.Count(place => place.Name == "End") == 1);
        }*/
    }
}