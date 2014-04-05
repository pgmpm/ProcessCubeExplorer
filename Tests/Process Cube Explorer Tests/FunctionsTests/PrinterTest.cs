using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainV2Tests.FunctionsTests
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "PrinterTest" und soll
    ///alle PrinterTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass()]
    public class PrinterTest
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

 
    }
}
