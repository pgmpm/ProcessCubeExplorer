using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;

namespace pgmpm.DBConnectTests.Exceptions
{


    /// <summary>
    ///This is a test class for WrongCredentialsExceptionTest and is intended
    ///to contain all WrongCredentialsExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WrongCredentialsExceptionTest
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
        ///A test for WrongCredentialsException Constructor
        ///</summary>
        [TestMethod()]
        public void WrongCredentialsExceptionConstructorTest()
        {
            WrongCredentialsException target = new WrongCredentialsException();
            Assert.IsNotNull(target, "WrongCredentialsException Object created.");
        }

        /// <summary>
        ///A test for WrongCredentialsException Constructor
        ///</summary>
        [TestMethod()]
        public void WrongCredentialsExceptionConstructorTest1()
        {
            string message = "Testmessage";
            WrongCredentialsException target = new WrongCredentialsException(message);
            Assert.IsNotNull(target, "WrongCredentialsException Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
        }

        /// <summary>
        ///A test for WrongCredentialsException Constructor
        ///</summary>
        [TestMethod()]
        public void WrongCredentialsExceptionConstructorTest2()
        {
            string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            WrongCredentialsException target = new WrongCredentialsException(message, innnerException);
            Assert.IsNotNull(target, "WrongCredentialsException Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Test the de/serialization for WrongCredentialsException.
        /// </summary>
        [TestMethod()]
        public void WrongCredentialsExceptionSerializeTest()
        {
            string message = "Error Serial test WrongCredentialsException";
            WrongCredentialsException testException = new WrongCredentialsException(message);
            string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            WrongCredentialsException result = (Serializer.Deserialize<WrongCredentialsException>(fileName));
            Assert.IsInstanceOfType(result, typeof(WrongCredentialsException), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }
    }
}
