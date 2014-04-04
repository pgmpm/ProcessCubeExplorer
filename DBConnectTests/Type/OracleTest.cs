using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.DBConnectTests.Properties;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Fields;
using System;

namespace pgmpm.DBConnectTests.Type
{
    [TestClass]
    public class OracleTest
    {

        private TestContext _testContextInstance;
        public static ConnectionParameters ConParams;
        public static MatrixSelectionModel Model;

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
        //// 
        //You can use the following additional attributes as you write your tests:

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Assert.Inconclusive("Oracle not yet tested");
            ConParams = new ConnectionParameters
            {
                Type = "Oracle",
                Name = "Oracle Connection",
                Host = Settings.Default.oracle_host,
                Database = Settings.Default.oracle_service,
                Port = Settings.Default.oracle_port,
                User = Settings.Default.oracle_user,
                Password = Settings.Default.oracle_password
            };

            DBWorker.ConfigureDBConnection(ConParams);
            DBWorker.OpenConnection();

            DBWorker.BuildMetadataRepository();

            Model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\OracleResults.mpm");

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DBWorker.Reset();
        }

        #endregion

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod()]
        public void CountEventTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateEventCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("11030", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod()]
        public void CountUniqueEventTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateUniqueEventCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("187", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod()]
        public void CountCaseTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateCaseCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("34", listCount, "Correct list count");
        }
    }
}
