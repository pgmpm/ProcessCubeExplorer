using pgmpm.Consolidation.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using pgmpm.MatrixSelection.Fields;
using System.Collections.Generic;
using pgmpm.MatrixSelection;
using pgmpm.MainV2.Utilities;
using pgmpm.Consolidation;
using pgmpm.Model.PetriNet;

namespace ConsolidationTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "StandardConsolidatorTest" und soll
    ///alle StandardConsolidatorTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class StandardConsolidatorTest
    {


        private TestContext testContextInstance;
        public static MatrixSelectionModel Model;

        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Zusätzliche Testattribute
        // 
        //Sie können beim Verfassen Ihrer Tests die folgenden zusätzlichen Attribute verwenden:
        //
        //Mit ClassInitialize führen Sie Code aus, bevor Sie den ersten Test in der Klasse ausführen.
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\MatrixSelection.mpm");

            ConsolidatorSettings.ProcessModelType = typeof(PetriNet);

            ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", new HashSet<String>());
            ConsolidatorSettings.AddOrUpdateKey("SelectedEvents", new HashSet<String>());
            ConsolidatorSettings.AddOrUpdateKey("NumberOfEvents", 1);
            ConsolidatorSettings.AddOrUpdateKey("AndOrSelection", 1);
        }
       
        #endregion


        /// <summary>
        ///Ein Test für "StandardConsolidator-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void StandardConsolidatorConstructorTest()
        {
            HashSet<Field> listOfMatrixFields = new HashSet<Field>();

            foreach (Field currentField in Model.MatrixFields)
                listOfMatrixFields.Add(currentField);

            StandardConsolidator target = new StandardConsolidator(listOfMatrixFields);
            Assert.IsNotNull(target);
           
        }

        /// <summary>
        ///Ein Test für "Consolidate"
        ///</summary>
        [TestMethod()]
        public void ConsolidateTest()
        {
            ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", new HashSet<String> { "Loop", "Parallelism", "Events", "Min. Number of Events" });
            ConsolidatorSettings.AddOrUpdateKey("SelectedEvents", new HashSet<String>());
            ConsolidatorSettings.AddOrUpdateKey("NumberOfEvents", 1);
            ConsolidatorSettings.AddOrUpdateKey("AndOrSelection", 1);

            HashSet<Field> listOfMatrixFields = new HashSet<Field>();

            foreach (Field currentField in Model.MatrixFields)
                listOfMatrixFields.Add(currentField);
            
            StandardConsolidator target = new StandardConsolidator(listOfMatrixFields);
            HashSet<Field> actual = target.Consolidate();


            Assert.AreEqual(actual.Count, 7);

            ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", new HashSet<String> { "Loop"});
            target = new StandardConsolidator(listOfMatrixFields);
            actual = target.Consolidate();

            Assert.AreEqual(actual.Count, 2);


            ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", new HashSet<String> { "Parallelism" });
            target = new StandardConsolidator(listOfMatrixFields);
            actual = target.Consolidate();

            Assert.AreEqual(actual.Count, 0);
            
        }
    }
}
