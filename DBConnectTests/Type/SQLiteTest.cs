using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.DBConnectTests.Type
{
    [TestClass]
    public class SQLiteTest
    {
        public static ConnectionParameters ConParams;
        public static MatrixSelectionModel Model;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //// 
        //You can use the following additional attributes as you write your tests:

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            string testDbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Files\test.sqlite.db";

            // To enable tests in a virtual machine (change the letter accordingly)
            if (!Char.IsLetter(testDbPath.First()))
                testDbPath = "Z:" + testDbPath.Substring(10);

            ConParams = new ConnectionParameters("SQLite", "SQLite Connection", "", testDbPath, "", "", "");

            DBWorker.ConfigureDBConnection(ConParams);
            DBWorker.OpenConnection();

            DBWorker.BuildMetadataRepository();

            Model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\SQLiteResults.mpm");

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {

            DBWorker.Reset();
        }

        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod]
        public void CountEventTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateEventCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("70", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod]
        public void CountUniqueEventTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateUniqueEventCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("14", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod]
        public void CountCaseTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateCaseCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("17", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadColumnsForTableExceptionTest()
        {
            MPMSQLiteConnection connection = new MPMSQLiteConnection(ConParams);
            connection.ReadColumnsForTable("No Table");
        }
    }
}
