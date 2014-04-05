using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Helper;
using System;

namespace pgmpm.DBConnectTests.Helper
{


    /// <summary>
    ///This is a test class for DatabaseCreationHelperFromMXMLTest and is intended
    ///to contain all DatabaseCreationHelperFromMXMLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabaseCreationHelperFromMXMLTest
    {

        private DatabaseCreationHelperFromMXML _dbCreationHelper;
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{           
        //}
        ////
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateEmptyUnknownDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("unknown", "No File");
            string result = _dbCreationHelper.CreateEmptyDwh();
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateInsertUnknownDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("unknown", "No File");
            string result = _dbCreationHelper.CreateInsertSqlStatement();
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        public void CreateEmptyMySQLDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("mysql", "No File");
            string result = _dbCreationHelper.CreateEmptyDwh();
            Assert.IsNotNull(result, "DWH Statements created");
        }


        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateEmptyOracleDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("oracle", "No File");
            string result = _dbCreationHelper.CreateEmptyDwh();
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateInsertOracleDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("oracle", "No File");
            string result = _dbCreationHelper.CreateInsertSqlStatement();
            Assert.Fail("Exception expected");
        }
        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateEmptyPorstgreSQLDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("postgresql", "No File");
            string result = _dbCreationHelper.CreateEmptyDwh();
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateInsertPorstgreSQLDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("postgresql", "No File");
            string result = _dbCreationHelper.CreateInsertSqlStatement();
            Assert.Fail("Exception expected");
        }
        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateEmptySQLiteDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("sqlite", "No File");
            string result = _dbCreationHelper.CreateEmptyDwh();
            Assert.Fail("Exception expected");
        }

        /// <summary>
        ///A test for DatabaseCreationHelperFromMXML Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateInsertSQLiteDWHTest()
        {
            _dbCreationHelper = new DatabaseCreationHelperFromMXML("sqlite", "No File");
            string result = _dbCreationHelper.CreateInsertSqlStatement();
            Assert.Fail("Exception expected");
        }


    }
}
