using pgmpm.Consolidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using pgmpm.Consolidation.Algorithm;
using System.Collections.Generic;
using pgmpm.MatrixSelection.Fields;

namespace ConsolidationTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "ConsolidatorFactoryTest" und soll
    ///alle ConsolidatorFactoryTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class ConsolidatorFactoryTest
    {


        private TestContext testContextInstance;

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
            ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", new HashSet<String>());
            ConsolidatorSettings.AddOrUpdateKey("SelectedEvents", new HashSet<String>());
            ConsolidatorSettings.AddOrUpdateKey("NumberOfEvents", 1);
            ConsolidatorSettings.AddOrUpdateKey("AndOrSelection", 1);
        }
        #endregion



        [TestMethod()]
        public void CreateConsolidatorTest()
        {

            HashSet<Field> listOfMatrixField = new HashSet<Field>();

            var consolidator = ConsolidatorFactory.CreateConsolidator<StandardConsolidator>(listOfMatrixField);
            Assert.IsInstanceOfType(consolidator, typeof(StandardConsolidator));
        }
    }
}
