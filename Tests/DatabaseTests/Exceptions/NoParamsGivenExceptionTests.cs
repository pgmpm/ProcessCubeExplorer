using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;
namespace pgmpm.DBConnectTests.Exceptions
{
    /// <summary>
    ///This is a test class for NoParamsGivenExceptionTest and is intended
    ///to contain all NoParamsGivenExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NoParamsGivenExceptionTest
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
        ///A test for NoParamsGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void NoParamsGivenExceptionConstructorTest()
        {
            NoParamsGivenException target = new NoParamsGivenException();
            Assert.IsNotNull(target, "NoParamsGivenException Object created.");
        }

        /// <summary>
        ///A test for NoParamsGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void NoParamsGivenExceptionConstructorTest1()
        {
            string message = "Testmessage";
            NoParamsGivenException target = new NoParamsGivenException(message);
            Assert.IsNotNull(target, "NoParamsGivenException Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
        }

        /// <summary>
        ///A test for NoParamsGivenException Constructor
        ///</summary>
        [TestMethod()]
        public void NoParamsGivenExceptionConstructorTest2()
        {
            string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            NoParamsGivenException target = new NoParamsGivenException(message, innnerException);
            Assert.IsNotNull(target, "NoParamsGivenException Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Test the de/serialization for NoParamsGivenException.
        /// </summary>
        [TestMethod()]
        public void NoParamsGivenExceptionSerializeTest()
        {
            string message = "Error Serial test NoParamsGivenException";
            NoParamsGivenException testException = new NoParamsGivenException(message);
            string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            NoParamsGivenException result = (Serializer.Deserialize<NoParamsGivenException>(fileName));
            Assert.IsInstanceOfType(result, typeof(NoParamsGivenException), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }
    }
}
