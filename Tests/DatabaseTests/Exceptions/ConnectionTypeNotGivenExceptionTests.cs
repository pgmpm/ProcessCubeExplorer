using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;

namespace pgmpm.DBConnectTests.Exceptions
{
    /// <summary>
    ///This is a test class for ConnectionTypeNotGivenExceptionTest and is intended
    ///to contain all ConnectionTypeNotGivenExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConnectionTypeNotGivenExceptionTest
    {


        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        /// <summary>
        ///A test for ConnectionTypeNotGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void ConnectionTypeNotGivenExceptionConstructorTest()
        {
            ConnectionTypeNotGivenException target = new ConnectionTypeNotGivenException();
            Assert.IsNotNull(target, "ConnectionTypeNotGivenException Object created.");
        }

        /// <summary>
        ///A test for ConnectionTypeNotGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void ConnectionTypeNotGivenExceptionConstructorTest1()
        {
            string message = "Testmessage";
            ConnectionTypeNotGivenException target = new ConnectionTypeNotGivenException(message);
            Assert.IsNotNull(target, "ConnectionTypeNotGivenException Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
        }

        /// <summary>
        ///A test for ConnectionTypeNotGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void ConnectionTypeNotGivenExceptionConstructorTest2()
        {
            string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            ConnectionTypeNotGivenException target = new ConnectionTypeNotGivenException(message, innnerException);
            Assert.IsNotNull(target, "ConnectionTypeNotGivenException Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Tests the de/serialization for ConnectionTypeNotGivenException.
        /// </summary>
        [TestMethod()]
        public void ConnectionTypeNotGivenExceptionSerializeTest()
        {
            string message = "Error Serial test ConnectionTypeNotGivenException";
            ConnectionTypeNotGivenException testException = new ConnectionTypeNotGivenException(message);
            string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            ConnectionTypeNotGivenException result = (Serializer.Deserialize<ConnectionTypeNotGivenException>(fileName));
            Assert.IsInstanceOfType(result, typeof(ConnectionTypeNotGivenException), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }

    }
}
