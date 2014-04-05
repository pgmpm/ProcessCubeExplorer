using System;
using System.Linq;
using pgmpm.MiningAlgorithm;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;

namespace pgmpm.MiningAlgorithmTests
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für AlphaMinerTests
    /// </summary>
    [TestClass]
    public class AlphaMinerTests
    {
        #region Zusätzliche Testattribute
        //
        // Sie können beim Schreiben der Tests folgende zusätzliche Attribute verwenden:
        //
        // Verwenden Sie ClassInitialize, um vor Ausführung des ersten Tests in der Klasse Code auszuführen.
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Verwenden Sie ClassCleanup, um nach Ausführung aller Tests in einer Klasse Code auszuführen.
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen. 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// This method generates a simple test event log.
        /// </summary>
        /// <returns>EventLog</returns>
        /// <author>Naby Sow, Krystian Zielonka</author>
        public EventLog GenerateTestLog()
        {
            List<Case> listOfCases = new List<Case> { 
                new Case("case1", new List<Event> {
                    new Event("A"),
                    new Event("B"),
                    new Event("C"),
                    new Event("D")
                }),
                new Case("case2", new List<Event> {
                    new Event("A"),
                    new Event("C"),
                    new Event("B"),
                    new Event("D")
                }),
                new Case("case3", new List<Event> {
                    new Event("A"),
                    new Event("E"),
                    new Event("D")
                }),
            };

            EventLog log = new EventLog(name: "Test") { Cases = listOfCases };

            return log;
        }

        /// <author>Naby Sow, Krystian Zielonka</author>
        [TestMethod]
        public void DetectAllPlacesTestOntoTrue()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectAllPlaces");
            priObj.Invoke("AddsPlacesTogether");
            priObj.Invoke("RemoveAllDuplicatePlaces");
            List<AlphaMinerPlaces> alphaPlaces = (List<AlphaMinerPlaces>)priObj.GetField("_listOfAlphaPlaces");

            List<AlphaMinerPlaces> testPlaces = new List<AlphaMinerPlaces> {
                new AlphaMinerPlaces(new HashSet<string> { "A" }, new HashSet<string> { "B", "E" }),
                new AlphaMinerPlaces(new HashSet<string> { "A" }, new HashSet<string> { "C", "E" }), 
                new AlphaMinerPlaces(new HashSet<string> { "C", "E" }, new HashSet<string> { "D" }),
                new AlphaMinerPlaces(new HashSet<string> { "B", "E" }, new HashSet<string> { "D" })
            };

            Assert.IsTrue(alphaPlaces.Count.Equals(testPlaces.Count));

            foreach (AlphaMinerPlaces testPlace in testPlaces)
            {
                int count = alphaPlaces
                                .Count(k => k.PredecessorHashSet.SetEquals(testPlace.PredecessorHashSet) &&
                                            k.FollowerHashSet.SetEquals(testPlace.FollowerHashSet));
                Assert.IsTrue(count == 1);
            }
        }

        [TestMethod]
        public void DetectAllPlacesTestOntoFalse()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectAllPlaces");
            priObj.Invoke("AddsPlacesTogether");
            priObj.Invoke("RemoveAllDuplicatePlaces");
            List<AlphaMinerPlaces> alphaPlaces = (List<AlphaMinerPlaces>)priObj.GetField("_listOfAlphaPlaces");

            List<AlphaMinerPlaces> testPlaces = new List<AlphaMinerPlaces> {
                new AlphaMinerPlaces(new HashSet<string> { "A" }, new HashSet<string> { "B", "E" }), 
                new AlphaMinerPlaces(new HashSet<string> { "A" }, new HashSet<string> { "C", "E" }),
                new AlphaMinerPlaces(new HashSet<string> { "G", "E" }, new HashSet<string> { "F" }), // correctly -> "C,E", "D"
                new AlphaMinerPlaces(new HashSet<string> { "B", "E" }, new HashSet<string> { "D" }),
                new AlphaMinerPlaces(new HashSet<string> { "D" }, new HashSet<string> { "F" })       // too much
            };

            Assert.IsFalse(alphaPlaces.Count.Equals(testPlaces.Count));
            Assert.IsFalse(testPlaces[2].PredecessorHashSet.SetEquals(alphaPlaces[2].PredecessorHashSet));
            Assert.IsFalse(testPlaces[2].FollowerHashSet.SetEquals(alphaPlaces[2].FollowerHashSet));
        }


        /// <summary>
        /// To test, if the Activityset are not empty .
        /// </summary>
        /// <author>Naby Sow, Krystian Zielonka</author>
        [TestMethod]
        public void AlphaMinerDetectActivitySetTest()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectActivitiySet");
            HashSet<String> listOfActivities = (HashSet<String>)priObj.GetField("_listOfActivities");

            Assert.IsNotNull(listOfActivities);
        }

        /// <summary>
        /// To test, if all start-activities are in a list 
        /// </summary>
        /// <author>Naby Sow, Krystian Zielonka</author>
        [TestMethod]
        public void AlphaMinerDetectStartActivitiesTest()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectStartAndEndActivitiesInTraces");
            HashSet<String> listOfStart = (HashSet<String>)priObj.GetField("_listOfStartActivities");

            Assert.IsNotNull(listOfStart);
        }

        /// <summary>
        /// To test, if all end-activities are in a list 
        /// </summary>
        /// <author>Naby Sow, Krystian Zielonka</author>
        [TestMethod]
        public void AlphaMinerDetectEndActivitiesTest()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectStartAndEndActivitiesInTraces");
            HashSet<String> listOfEndActivities = (HashSet<String>)priObj.GetField("_listOfEndActivities");

            Assert.IsNotNull(listOfEndActivities);
        }

        /// <summary>
        /// To test, that the net will be build correctly.
        /// </summary>
        /// <author>Naby Sow, Krystian Zielonka</author>
        [TestMethod]
        public void AlphaMinerBuildTheNetTest()
        {
            Field field = new Field { EventLog = GenerateTestLog() };

            AlphaMiner alphaMiner = new AlphaMiner(field);
            PrivateObject priObj = new PrivateObject(alphaMiner);

            priObj.Invoke("DetectAllPlaces");
            priObj.Invoke("DetectStartAndEndActivitiesInTraces");
            priObj.Invoke("AddsPlacesTogether");
            priObj.Invoke("RemoveAllDuplicatePlaces");
            priObj.Invoke("BuildTheNet");

            PetriNet petriNet = (PetriNet)priObj.GetField("_petriNet");
            Place startPlace = petriNet.GetPlaceByName("start");
            Place endPlace = petriNet.GetPlaceByName("end");

            Assert.IsNotNull(petriNet);
            Assert.IsNotNull(startPlace);
            Assert.IsNotNull(endPlace);

            //MiningAlgorithm.AlphaMiner.PetriNet = new PetriNet("");

            //MiningAlgorithm.AlphaMiner.ListOfStartActivities.Add("A");
            //MiningAlgorithm.AlphaMiner.ListOfEndActivities.Add("D");

            //MiningAlgorithm.AlphaMiner.ListOfActivities.Add("A");
            //MiningAlgorithm.AlphaMiner.ListOfActivities.Add("B");
            //MiningAlgorithm.AlphaMiner.ListOfActivities.Add("C");
            //MiningAlgorithm.AlphaMiner.ListOfActivities.Add("D");
            //MiningAlgorithm.AlphaMiner.ListOfActivities.Add("E");

            //MiningAlgorithm.AlphaMiner.ListOfAlphaPlaces = ListAlphaMinerPlaces;

            //MiningAlgorithm.AlphaMiner.BuildTheNet();

        }
    }
}
