using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using System;

namespace pgmpm.DBConnectTests.Exceptions
{
    [TestClass]
    public class DBExceptionTests
    {
        public void DBExceptionTest()
        {
        }


        /// <summary>
        ///A test for DBException Constructor
        ///</summary>
        [TestMethod]
        public void DBExceptionConstructorTest()
        {
            DBException target = new DBException();
            Assert.IsNotNull(target, "DBException Object created.");
        }

        /// <summary>
        ///A test for DBException Constructor
        ///</summary>
        [TestMethod]
        public void DBExceptionConstructorTest1()
        {
            const string message = "Testmessage";
            DBException target = new DBException(message);
            Assert.IsNotNull(target, "DBException Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
        }

        /// <summary>
        ///A test for DBException Constructor
        ///</summary>
        [TestMethod]
        public void DBExceptionConstructorTest2()
        {
            const string message = "Testmessage";
            Exception innnerException = new Exception("Inner exception");
            DBException target = new DBException(message, innnerException);
            Assert.IsNotNull(target, "DBException Object created.");
            Assert.AreEqual(message, target.Message, "Messages are equal.");
            Assert.AreSame(innnerException, target.InnerException, "Inner exception equal.");
        }

        /// <summary>
        /// Test the de/serialization for DBException.
        /// </summary>
        [TestMethod]
        public void DBExceptionSerializeTest()
        {
            const string message = "Error Serial test DBException";
            DBException testException = new DBException(message);
            const string fileName = "Exception.test";
            Assert.IsTrue(Serializer.Serialize(fileName, testException), "Serialized");
            DBException result = (Serializer.Deserialize<DBException>(fileName));
            Assert.IsInstanceOfType(result, typeof(DBException), "Deserialized");
            Assert.IsTrue(message.Equals(result.ExceptionMessage), "Exception message equal");
        }
    }
}
