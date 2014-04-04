using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Model;

namespace pgmpm.DBConnectTests
{


    /// <summary>
    ///This is a test class for ConnectionParamsTest and is intended
    ///to contain all ConnectionParamsTest Unit Tests
    ///</summary>
    ///<author>Bernd Nottbeck</author>
    [TestClass()]
    public class ConnectionParamsTest
    {

        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        ///<author>Bernd Nottbeck</author>
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
        ///A test for ConnectionParameters Constructor
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void ConnectionParamsConstructorTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ConnectionParameters Constructor
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void ConnectionParamsConstructorTest1()
        {
            string type = "Teststring";
            string name = "Teststring";
            string host = "Teststring";
            string database = "Teststring";
            string user = "Teststring";
            string password = "Teststring";
            string port = "Teststring";
            ConnectionParameters target = new ConnectionParameters(type, name, host, database, user, password, port);
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// Calls is Complete expecting false
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void IsCompleteFalseTest1()
        {
            ConnectionParameters target = new ConnectionParameters();
            Assert.IsFalse(target.IsComplete(),"Connecting String was not Complete");
        }

        /// <summary>
        /// Calls is Complete expecting false
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void IsCompleteTrueTest1()
        {
            string type = "Teststring";
            string name = "Teststring";
            string host = "Teststring";
            string database = "Teststring";
            string user = "Teststring";
            string password = "Teststring";
            string port = "Teststring";
            ConnectionParameters target = new ConnectionParameters(type, name, host, database, user, password, port);
            Assert.IsTrue(target.IsComplete(), "ConnectionParameters Complete");
        }
        /// <summary>
        ///A test for Database
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void databaseTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.Database = expected;
            actual = target.Database;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Host
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void hostTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.Host = expected;
            actual = target.Host;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void NameTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Tests the de-/encryption of a password string.
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void PasswordTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "SecretKey4711";
            string actual;
            target.Password = expected;
            actual = target.Password;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Port
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void PortTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.Port = expected;
            actual = target.Port;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void TypeTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.Type = expected;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for User
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void UserTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            string expected = "Teststring";
            string actual;
            target.User = expected;
            actual = target.User;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for User
        ///</summary>
        ///<author>Bernd Nottbeck</author>
        [TestMethod()]
        public void EmptyPasswordTest()
        {
            ConnectionParameters target = new ConnectionParameters();
            target.Password = "";
            Assert.IsTrue(string.IsNullOrEmpty(target.Password));
        }
    }
}
