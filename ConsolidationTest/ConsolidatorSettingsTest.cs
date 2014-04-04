using pgmpm.Consolidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using pgmpm.Consolidation.Algorithm;
using pgmpm.Model;

namespace ConsolidationTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "ConsolidatorSettingsTest" und soll
    ///alle ConsolidatorSettingsTest Komponententests enthalten.
    ///</summary>
    [TestClass()]
    public class ConsolidatorSettingsTest
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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Mit ClassCleanup führen Sie Code aus, nachdem alle Tests in einer Klasse ausgeführt wurden.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Ein Test für "AddOrUpdateKey"
        ///</summary>
        [TestMethod()]
        public void AddOrUpdateKeyTest()
        {
            var myObject = new Object();
            var myObject2 = new Object();

            ConsolidatorSettings.AddOrUpdateKey("Test", myObject);

            var actual = ConsolidatorSettings.Get("Test");
            Assert.AreEqual(myObject, actual);

            ConsolidatorSettings.AddOrUpdateKey("Test", myObject2);
            actual = ConsolidatorSettings.Get("Test");
            Assert.AreEqual(myObject2, actual);

            actual = ConsolidatorSettings.Get("Does not exist");
            Assert.IsNull(actual);
        }

        
        /// <summary>
        ///Ein Test für "consolidationType"
        ///</summary>
        [TestMethod()]
        public void consolidationTypeTest()
        {
            Type expected = typeof(StandardConsolidator);
            Type actual;
            ConsolidatorSettings.ConsolidationType = expected;
            actual = ConsolidatorSettings.ConsolidationType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "processmodellType"
        ///</summary>
        [TestMethod()]
        public void processmodellTypeTest()
        {
            Type expected = typeof(ProcessModel);
            Type actual;
            ConsolidatorSettings.ProcessModelType = expected;
            actual = ConsolidatorSettings.ProcessModelType;
            Assert.AreEqual(expected, actual);
        }
    }
}
