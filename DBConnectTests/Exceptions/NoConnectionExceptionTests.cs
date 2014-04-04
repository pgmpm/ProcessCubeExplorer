using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;
namespace pgmpm.DBConnectTests.Exceptions
{
    /// <summary>
    ///This is a test class for NoConnectionExceptionTest and is intended
    ///to contain all NoConnectionExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NoConnectionExceptionTest
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
        ///A test for NoConnectionException Constructor
        ///</summary>
        [TestMethod()]
        public void NoConnectionExceptionConstructorTest()
        {
            NoConnectionException target = new NoConnectionException();
            Assert.IsNotNull(target, "NoConnectionException Object created.");
        }

        /// <summary>
        ///A test for NoConnectionException Constructor
        ///</summary>
        [TestMethod()]
        public void NoConnectionExceptionConstructorTest1()
        {
            string message = "Testmessage";
            NoConnectionException target = new NoConnectionException(message);
            Assert.IsNotNull(target, "NoConnectionException Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
        }

        /// <summary>
        ///A test for NoConnectionException Constructor
        ///</summary>
        [TestMethod()]
        public void NoConnectionExceptionConstructorTest2()
        {
            string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            NoConnectionException target = new NoConnectionException(message, innnerException);
            Assert.IsNotNull(target, "NoConnectionException Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Test the de/serialization for NoConnectionException.
        /// </summary>
        [TestMethod()]
        public void NoConnectionExceptionSerializeTest()
        {
            string message = "Error Serial test NoConnectionException";
            NoConnectionException testException = new NoConnectionException(message);
            string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            NoConnectionException result = (Serializer.Deserialize<NoConnectionException>(fileName));
            Assert.IsInstanceOfType(result, typeof(NoConnectionException), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }
    }
}
