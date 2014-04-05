using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainV2Tests.FunctionsTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "VisualizationHelpersTest" und soll
    ///alle VisualizationHelpersTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass()]
    public class VisualizationHelpersTest
    {


        private TestContext _testContextInstance;

        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }



        //[TestMethod()]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void SerializeTest2()
        //{
        //    string filename = "Unit.tst";
        //    ConnectionParameters con = null;

        //    bool actual;

        //    actual = Serializer.Serialize(filename, con);
        //    Assert.Fail("An exception should have been thrown.");
        //}
    }
}
