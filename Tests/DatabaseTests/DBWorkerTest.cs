using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System;
using pgmpm.DBConnectTests.Properties;

namespace pgmpm.DBConnectTests
{
    /// <summary>
    ///This is a test class for DBWorkerTest and is intended
    ///to contain all DBWorkerTest Unit Tests
    ///</summary>
    [TestClass]
    public class DBWorkerTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void MyTestCleanup()
        {
            DBWorker.CloseConnection();
        }

        /// <summary>
        ///A test for ConfigureDBConnection with an empty connectionParameter object
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NoParamsGivenException))]
        public void ConfigureDBConnectionTestFail0()
        {
            // arrange
            DBWorker.CloseConnection();
            ConnectionParameters conParams = new ConnectionParameters("non-existingType");
            // act
            DBWorker.ConfigureDBConnection(conParams);
        }

        /// <summary>
        ///A test for ConfigureDBConnection with empty connectionParameters and Type SQLite 
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NoParamsGivenException))]
        public void ConfigureDBConnectionTestFail1()
        {
            // arrange
            DBWorker.CloseConnection();
            ConnectionParameters conParams = new ConnectionParameters("SQLite");
            // act
            DBWorker.ConfigureDBConnection(conParams);
        }

        /// <summary>
        ///A test for ConfigureDBConnection with an empty connectionParameter type
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NoParamsGivenException))]
        public void ConfigureDBConnectionTestFail2()
        {
            // arrange
            DBWorker.CloseConnection();
            ConnectionParameters conParams = new ConnectionParameters("MySQL");
            // act
            DBWorker.ConfigureDBConnection(conParams);
        }

        /// <summary>
        ///A test for ConfigureDBConnection with an empty connectionParameter type
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(ConnectionTypeNotGivenException))]
        public void ConfigureDBConnectionTestFail3()
        {
            // arrange
            DBWorker.CloseConnection();
            ConnectionParameters conParams = new ConnectionParameters("", "123", "123", "123", "123", "123", "123");
            // act
            DBWorker.ConfigureDBConnection(conParams);
        }

        /// <summary>
        ///A test for CloseConnection
        ///</summary>
        [TestMethod]
        public void CloseConnectionTest()
        {
            // act
            bool actual = DBWorker.CloseConnection();
            // assert
            Assert.IsTrue(actual);
            Assert.IsFalse(DBWorker.IsOpen());
        }

        /// <summary>
        ///A test for GetConnectionName
        ///</summary>
        [TestMethod]
        public void GetConnectionNameNoConnectionTest()
        {
            // arrange
            DBWorker.Reset();
            const string expected = "No connection";
            // act
            string actual = DBWorker.GetConnectionName();
            // assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ConfigureDBConnection with an empty connectionParameter object
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NoParamsGivenException))]
        public void GetFactsException()
        {
            DBWorker.GetFacts(null, null, null);
            Assert.Fail("An Exception was expected");
        }

        /// <summary>
        ///Oracle Tests
        ///</summary>
        [TestMethod]
        public void OracleTests()
        {
            // arrange
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "Oracle",
                Name = "Oracle Connection",
                Host = Settings.Default.oracle_host,
                Database = Settings.Default.oracle_service,
                Port = Settings.Default.oracle_port,
                User = Settings.Default.oracle_user,
                Password = Settings.Default.oracle_password
            };
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "Oracle connection configured");
            Assert.IsTrue(DBWorker.OpenConnection(), "Oracle connection opened");
            Assert.IsTrue(DBWorker.IsOpen(), "Oracle connection is currently open");
            Assert.IsFalse(string.IsNullOrEmpty(DBWorker.GetConnectionName()), "Oracle connection name correct.");
            Assert.IsTrue(DBWorker.CloseConnection(), "Oracle connection closed");
            Assert.IsTrue(DBWorker.TryConnection(), "Try Connection");
            Assert.IsTrue(DBWorker.DisposeConnection(), "Connection disposed");

            DBWorker.Reset();
        }

        /// <summary>
        ///Oracle Tests
        ///</summary>
        [TestMethod]
        public void GetMetaDataOracleTests()
        {
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "Oracle",
                Name = "Oracle Connection",
                Host = Settings.Default.oracle_host,
                Database = Settings.Default.oracle_service,
                Port = Settings.Default.oracle_port,
                User = Settings.Default.oracle_user,
                Password = Settings.Default.oracle_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "Oracle connection configured");
            Assert.IsNotNull(DBWorker.GetMetaData(DBWorker.SqlCreator.FactTableName, DBWorker.SqlCreator.EventTableName), "Load MetaData for Oracle");
            DBWorker.Reset();
        }

        /// <summary>
        ///MySQL Tests
        ///</summary>
        [TestMethod]
        public void MySQLTests()
        {
            // arrange
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "MySQL",
                Name = "MySQL Connection",
                Host = Settings.Default.mysql_host,
                Database = Settings.Default.mysql_database,
                Port = Settings.Default.mysql_port,
                User = Settings.Default.mysql_user,
                Password = Settings.Default.mysql_password
            };
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "MySQL connection configured");
            Assert.IsTrue(DBWorker.OpenConnection(), "MySQL connection opened");
            Assert.IsTrue(DBWorker.IsOpen(), "MySQL connection is currently open");
            Assert.IsFalse(string.IsNullOrEmpty(DBWorker.GetConnectionName()), "MySQL connection name correct.");
            Assert.IsTrue(DBWorker.CloseConnection(), "MySQL connection closed");
            DBWorker.Reset();
        }

        [TestMethod]
        public void GetMetaDataMySQLTests()
        {
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "MySQL",
                Name = "MySQL Connection",
                Host = Settings.Default.mysql_host,
                Database = Settings.Default.mysql_database,
                Port = Settings.Default.mysql_port,
                User = Settings.Default.mysql_user,
                Password = Settings.Default.mysql_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "MySQL connection configured");
            Assert.IsNotNull(DBWorker.GetMetaData(DBWorker.SqlCreator.FactTableName, DBWorker.SqlCreator.EventTableName), "Load MetaData for Oracle");
            DBWorker.Reset();
        }

        /// <summary>
        ///PostgreSQL Tests
        ///</summary>
        [TestMethod]
        public void PostgreSQLTests()
        {
            // arrange
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "PostgreSQL",
                Name = "PostgreSQL Connection",
                Host = Settings.Default.postgresql_host,
                Database = Settings.Default.postgresql_database,
                Port = Settings.Default.postgresql_port,
                User = Settings.Default.postgresql_user,
                Password = Settings.Default.postgresql_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "PostgreSQL connection configured");
            Assert.IsTrue(DBWorker.OpenConnection(), "PostgreSQL connection opened");
            Assert.IsTrue(DBWorker.IsOpen(), "PostgreSQL connection is currently open");
            Assert.IsFalse(string.IsNullOrEmpty(DBWorker.GetConnectionName()), "PostgreSQL connection name correct.");
            Assert.IsTrue(DBWorker.CloseConnection(), "PostgreSQL connection closed");
            DBWorker.Reset();
        }

        [TestMethod]
        public void GetMetaDataPostgreSQLTests()
        {
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "PostgreSQL",
                Name = "PostgreSQL Connection",
                Host = Settings.Default.postgresql_host,
                Database = Settings.Default.postgresql_database,
                Port = Settings.Default.postgresql_port,
                User = Settings.Default.postgresql_user,
                Password = Settings.Default.postgresql_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "PostgreSQL connection configured");
            Assert.IsNotNull(DBWorker.GetMetaData(DBWorker.SqlCreator.FactTableName, DBWorker.SqlCreator.EventTableName), "Load MetaData for Oracle");
            DBWorker.Reset();
        }

        /// <summary>
        ///SQLite Tests
        ///</summary>
        [TestMethod]
        public void SQLiteTests()
        {

            // arrange
            string testDbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Files\test.sqlite.db";

            // To enable tests in a virtual machine (change the letter accordingly)
            if (!Char.IsLetter(testDbPath.First()))
                testDbPath = "Z:" + testDbPath.Substring(10);

            ConnectionParameters conParams = new ConnectionParameters("SQLite", "SQLite Connection", "", testDbPath);
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "SQLite connection configured");
            Assert.IsTrue(DBWorker.OpenConnection(), "SQLite connection opened");
            Assert.IsTrue(DBWorker.IsOpen(), "SQLite connection is currently open");
            Assert.IsFalse(string.IsNullOrEmpty(DBWorker.GetConnectionName()), "SQLite connection name correct.");
            Assert.IsTrue(DBWorker.CloseConnection(), "SQLite connection closed");
            DBWorker.Reset();
        }

        [TestMethod]
        public void GetMetaDataSQLiteTests()
        {
            string testDbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Files\test.sqlite.db";

            // To enable tests in a virtual machine (change the letter accordingly)
            if (!Char.IsLetter(testDbPath.First()))
                testDbPath = "Z:" + testDbPath.Substring(10);

            ConnectionParameters conParams = new ConnectionParameters("SQLite", "SQLite Connection", "", testDbPath);
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "SQLite connection configured");
            Assert.IsNotNull(DBWorker.GetMetaData(DBWorker.SqlCreator.FactTableName, DBWorker.SqlCreator.EventTableName), "Load MetaData for Oracle");
            DBWorker.Reset();
        }

        /// <summary>
        ///MS-SQL Tests
        ///</summary>
        [TestMethod]
        public void MSSQLTests()
        {
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "MS-SQL",
                Name = "MS-SQL Connection",
                Host = Settings.Default.mssql_host,
                Database = Settings.Default.mssql_database,
                Port = Settings.Default.mssql_port,
                User = Settings.Default.mssql_user,
                Password = Settings.Default.mssql_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "MS-SQL connection configured");
            Assert.IsTrue(DBWorker.OpenConnection(), "MS-SQL connection opened");
            Assert.IsTrue(DBWorker.IsOpen(), "MS-SQL connection is currently open");
            Assert.IsFalse(string.IsNullOrEmpty(DBWorker.GetConnectionName()), "MS-SQL connection name correct.");
            Assert.IsTrue(DBWorker.CloseConnection(), "MS-SQL connection closed");
            DBWorker.Reset();
        }

        [TestMethod]
        public void GetMetaDataMSSQLTests()
        {
            ConnectionParameters conParams = new ConnectionParameters
            {
                Type = "MS-SQL",
                Name = "MS-SQL Connection",
                Host = Settings.Default.mssql_host,
                Database = Settings.Default.mssql_database,
                Port = Settings.Default.mssql_port,
                User = Settings.Default.mssql_user,
                Password = Settings.Default.mssql_password
            }; 
            Assert.IsTrue(DBWorker.ConfigureDBConnection(conParams), "MS-SQL connection configured");
            Assert.IsNotNull(DBWorker.GetMetaData(DBWorker.SqlCreator.FactTableName, DBWorker.SqlCreator.EventTableName), "Load MetaData for Oracle");
            DBWorker.Reset();
        }

        /// <summary>
        ///SQLite Tests
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(DatabaseDoesNotExist))]
        public void ConfigureDBConnectionUnknownDBException()
        {
            ConnectionParameters conParams = new ConnectionParameters("Unknown", "Unknown", "Unknown", "Unknown", "Unknown", "Unknown", "Unknown");
            Assert.IsFalse(DBWorker.ConfigureDBConnection(conParams), "DBDoes not Exist Exception");
        }
    }
}