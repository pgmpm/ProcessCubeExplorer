using pgmpm.MainV2.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainV2Tests.FunctionsTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "ErrorHandlingTest" und soll
    ///alle ErrorHandlingTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass()]
    public class ErrorHandlingTest
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

        
        /// <summary>
        ///Ein Test für "ErrorHandling-Konstruktor"
        ///</summary>
        [TestMethod()]
        public void ErrorHandlingConstructorTest()
        {
            ErrorHandling target = new ErrorHandling();
            Assert.IsNotNull(target);
        }

    }
}
