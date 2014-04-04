using pgmpm.MainV2.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Model;
using System;

namespace MainV2Tests.FunctionsTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "SerializerTest" und soll
    ///alle SerializerTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass()]
    public class SerializerTest
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
     /// 
     /// </summary>
        [TestMethod()]
        public void DeserializeTest()
        {
            string filename = "Unit.tst";
            ConnectionParameters expected = new ConnectionParameters("TestString1", "TestString2", "TestString3", "TestString4", "TestString5", "TestString6", "TestString7");
            ConnectionParameters actual;

            Serializer.Serialize(filename, expected);
            actual = Serializer.Deserialize<ConnectionParameters>(filename);

            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Host, actual.Host);
            Assert.AreEqual(expected.IsLastUsedDatabase , actual.IsLastUsedDatabase);
            Assert.AreEqual(expected.Name , actual.Name );
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Port , actual.Port);
            Assert.AreEqual(expected.Type , actual.Type);
            Assert.AreEqual(expected.User , actual.User);
        }

        /// <summary>
        ///Ein Test für "Serialize"
        ///</summary>
        [TestMethod()]
        public void SerializeTest1()
        {
            string filename = "Unit.tst";
            ConnectionParameters con = new ConnectionParameters("TestString", "TestString", "TestString", "TestString", "TestString", "TestString", "TestString");

            bool expected = true;
            bool actual;

            actual = Serializer.Serialize(filename, con);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "Serialize"
        ///</summary>
        ///
        
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SerializeTest2()
        {
            string filename = "Unit.tst";
            ConnectionParameters con = null;
                       
            bool actual;

            actual = Serializer.Serialize(filename, con);
            Assert.Fail("An exception should have been thrown.");
        }
    }
}
