using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Model;
using pgmpm.DBConnectTests.Properties;
using pgmpm.MatrixSelection;
using pgmpm.Database;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.DBConnectTests.Type
{
    [TestClass]
    public class PostgreSQLTest
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
            ConParams = new ConnectionParameters
            {
                Type = "PostgreSQL",
                Name = "PostgreSQL Connection",
                Host = Settings.Default.postgresql_host,
                Database = Settings.Default.postgresql_database,
                Port = Settings.Default.postgresql_port,
                User = Settings.Default.postgresql_user,
                Password = Settings.Default.postgresql_password
            };

            DBWorker.ConfigureDBConnection(ConParams);
            DBWorker.OpenConnection();

            DBWorker.BuildMetadataRepository();

            Model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\PostgreResults.mpm");

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
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
        [TestMethod()]
        public void CountEventTest()
        {
            Field testField = Model.MatrixFields[0];
            String listCount = DBWorker.GetCountFromSqlStatement(DBWorker.CreateEventCountSqlStatement(Model.SelectedDimensions, testField));
            Assert.IsNotNull(listCount, "Empty list");
            Assert.AreEqual("59", listCount, "Correct list count");

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
            Assert.AreEqual("12", listCount, "Correct list count");
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
            Assert.AreEqual("17", listCount, "Correct list count");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod()]
        public void OpenConnectionNullTest()
        {
            ConnectionParameters postgresqlConParams = new ConnectionParameters
            {
                Type = "PostgreSQL",
                Name = "PostgreSQL Connection",
                Host = Settings.Default.postgresql_host,
                Database = Settings.Default.postgresql_database,
                Port = Settings.Default.postgresql_port,
                User = Settings.Default.postgresql_user,
                Password = Settings.Default.postgresql_password
            }; 
            MPMdBConnection connection = new MPMPostgreSQLConnection(postgresqlConParams);
            connection.KillConnection();
            Assert.IsFalse(connection.Open(), "No Connection to open");
        }

        /// <summary>
        ///A test for GetFactDataTable
        ///</summary>
        [TestMethod()]
        public void CloseConnectionNullTest()
        {
            ConnectionParameters postgresqlConParams = new ConnectionParameters
            {
                Type = "PostgreSQL",
                Name = "PostgreSQL Connection",
                Host = Settings.Default.postgresql_host,
                Database = Settings.Default.postgresql_database,
                Port = Settings.Default.postgresql_port,
                User = Settings.Default.postgresql_user,
                Password = Settings.Default.postgresql_password
            }; 
            MPMdBConnection connection = new MPMPostgreSQLConnection(postgresqlConParams);
            connection.KillConnection();
            Assert.IsFalse(connection.Close(), "No Connection to open");
        }

        [TestMethod()]
        public void MatrixSelectionTests()
        {
            double original = MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality;
            MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality = 5.0;
            MatrixSelection.Properties.Settings.Default.Save();
            MatrixSelection.Properties.Settings.Default.Reload();

            Assert.IsTrue(MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality.Equals(5.0));

            MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality = original;
            MatrixSelection.Properties.Settings.Default.Save();
            MatrixSelection.Properties.Settings.Default.Reload();

            Assert.IsTrue(original.Equals(MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality));

        }

    }
}
