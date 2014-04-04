using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;
namespace pgmpm.DBConnectTests.Exceptions
{
    /// <summary>
    ///This is a test class for DatabaseDoesNotExistTest and is intended
    ///to contain all DatabaseDoesNotExistTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabaseDoesNotExistTest
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
        ///A test for DatabaseDoesNotExist Constructor
        ///</summary>
        [TestMethod()]
        public void DatabaseDoesNotExistConstructorTest()
        {
            DatabaseDoesNotExist target = new DatabaseDoesNotExist();
            Assert.IsNotNull(target, "DatabaseDoesNotExist Object created.");
        }

        /// <summary>
        ///A test for DatabaseDoesNotExist Constructor
        ///</summary>
        [TestMethod()]
        public void DatabaseDoesNotExistConstructorTest1()
        {
            string message = "Testmessage";
            DatabaseDoesNotExist target = new DatabaseDoesNotExist(message);
            Assert.IsNotNull(target, "DatabaseDoesNotExist Object created.");
            Assert.AreEqual(message, target.ExceptionMessage, "Messages are equal.");
        }

        /// <summary>
        ///A test for DatabaseDoesNotExist Constructor
        ///</summary>
        [TestMethod()]
        public void DatabaseDoesNotExistConstructorTest2()
        {
            string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            DatabaseDoesNotExist target = new DatabaseDoesNotExist(message, innnerException);
            Assert.IsNotNull(target, "DatabaseDoesNotExist Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Tests the de/serialization for DatabaseDoesNotExist.
        /// </summary>
        [TestMethod()]
        public void DatabaseDoesNotExistSerializeTest()
        {
            string message = "Error Serial test DatabaseDoesNotExist";
            DatabaseDoesNotExist testException = new DatabaseDoesNotExist(message);
            string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            DatabaseDoesNotExist result = (Serializer.Deserialize<DatabaseDoesNotExist>(fileName));
            Assert.IsInstanceOfType(result, typeof(DatabaseDoesNotExist), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }
    }
}
