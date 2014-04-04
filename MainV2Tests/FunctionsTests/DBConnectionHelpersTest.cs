using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;

namespace MainV2Tests.FunctionsTests
{


    /// <summary>
    ///Dies ist eine Testklasse für "DBConnectionHelpersTest" und soll
    ///alle DBConnectionHelpersTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass]
    public class DBConnectionHelpersTest
    {
        public static ConnectionParameters ConParams;
        public static MatrixSelectionModel Model;


        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Zusätzliche Testattribute
        // 
        //Sie können beim Verfassen Ihrer Tests die folgenden zusätzlichen Attribute verwenden:
        //
        //Mit ClassInitialize führen Sie Code aus, bevor Sie den ersten Test in der Klasse ausführen.
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            string testDbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Files\test.sqlite.db";

            // To enable tests in a virtual machine (change the letter accordingly)
            if (!Char.IsLetter(testDbPath.First()))
                testDbPath = "Z:" + testDbPath.Substring(10);

            ConParams = new ConnectionParameters("SQLite", "SQLite Connection", "", testDbPath);

            DBWorker.ConfigureDBConnection(ConParams);
            DBWorker.OpenConnection();

            DBWorker.BuildMetadataRepository();

            Model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\MatrixSelection.mpm");
        }


        #endregion

        /// <summary>
        ///Test for the "CurrentConnectionParameters"
        ///</summary>
        [TestMethod]
        public void CurrentConnectionParametersTest()
        {
            ConnectionParameters expected = new ConnectionParameters("TestString", "TestString", "TestString", "TestString", "TestString", "TestString", "TestString");
            ConnectionParameters actual;

            DBConnectionHelpers.CurrentConnectionParameters = expected;
            actual = DBConnectionHelpers.CurrentConnectionParameters;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Test for the "ConnectionParametersList"
        ///</summary>
        [TestMethod]
        public void ConnectionParametersListTest()
        {
            List<ConnectionParameters> expected = new List<ConnectionParameters>();
            List<ConnectionParameters> actual;

            DBConnectionHelpers.ConnectionParametersList = expected;
            actual = DBConnectionHelpers.ConnectionParametersList;

            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///Ein Test für "LoadLastUsedDatabase"
        ///</summary>
        [TestMethod]
        public void LoadLastUsedDatabaseTest()
        {
            ConnectionParameters expected = new ConnectionParameters("TestString", "TestString", "TestString", "TestString", "TestString", "TestString", "TestString");
            ConnectionParameters actual;


            expected.IsLastUsedDatabase = true;
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.ConnectionParametersList.Add(expected);

            actual = DBConnectionHelpers.LoadLastUsedDatabase();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ein Test für "LoadLastUsedDatabase"
        ///</summary>
        [TestMethod]
        public void LoadLastUsedDatabaseTest2()
        {
            ConnectionParameters expected = new ConnectionParameters("TestString", "TestString", "TestString", "TestString", "TestString", "TestString", "TestString");
            ConnectionParameters actual;


            expected.IsLastUsedDatabase = false;
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.ConnectionParametersList.Add(expected);

            actual = DBConnectionHelpers.LoadLastUsedDatabase();

            Assert.IsNull(actual);
        }

        /// <summary>
        ///Ein Test für "LoadConnectionParameters"
        ///</summary>
        [TestMethod]
        public void LoadConnectionParametersTest()
        {
            bool expected;
            bool actual;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\DatabaseSettings.mpm"))
                expected = true;
            else
                expected = false;

            actual = DBConnectionHelpers.LoadConnectionParameters();
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///Test for "EstablishDatabaseConnection"
        ///Conparams are correct and the test should be true
        ///</summary>
        [TestMethod]
        public void EstablishDatabaseConnectionTest()
        {
            DBConnectionHelpers.EstablishDatabaseConnection(ConParams);
            Assert.IsNotNull(DBWorker.MetaData);

        }

        /// <summary>
        ///Test for "AddDatabaseConnectionToConnectionList"
        ///</summary>
        [TestMethod]
        public void AddDatabaseConnectionToConnectionListTest()
        {
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();

            DBConnectionHelpers.AddDatabaseConnectionToConnectionList(ConParams);
            Assert.IsTrue(DBConnectionHelpers.ConnectionParametersList.Contains(ConParams));
        }

        /// <summary>
        ///Test for "SaveConnectionParameters"
        ///</summary>
        [TestMethod]
        public void SaveConnectionParametersTest()
        {
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.AddDatabaseConnectionToConnectionList(ConParams);

            DBConnectionHelpers.SaveConnectionParameters();
            Assert.IsTrue(File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "DatabaseSettings.mpm"));

        }


        /// <summary>
        ///Test for "SaveLastUsedDatabase"
        ///</summary>
        [TestMethod]
        public void SaveLastUsedDatabaseTest()
        {
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.AddDatabaseConnectionToConnectionList(ConParams);

            DBConnectionHelpers.SaveLastUsedDatabase(ConParams);
            Assert.IsTrue(DBConnectionHelpers.ConnectionParametersList[0].IsLastUsedDatabase);

            ConnectionParameters c = new ConnectionParameters();
            DBConnectionHelpers.SaveLastUsedDatabase(c);
            Assert.IsTrue(DBConnectionHelpers.ConnectionParametersList[0].IsLastUsedDatabase == false);
        }

        /// <summary>
        ///Test for "RemoveConnectionParameter"
        ///</summary>
        [TestMethod]
        public void RemoveConnectionParameterTest()
        {
            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.AddDatabaseConnectionToConnectionList(ConParams);

            DBConnectionHelpers.RemoveConnectionParameter(ConParams.Name);
            Assert.IsFalse(DBConnectionHelpers.ConnectionParametersList.Contains(ConParams));
        }



        ///// <summary>
        /////Ein Test für "CheckIfInputIsEmpty"
        /////</summary>
        //[TestMethod()]
        //public void CheckIfInputIsEmptyTest()
        //{
        //    ConnectionParameters connectionParams = new ConnectionParameters("TestString", "TestString", "TestString", "TestString", "TestString", "TestString", "TestString");

        //    bool expected = true;
        //    bool actual;

        //    actual = DBConnectionHelpers.CheckIfInputIsEmpty(connectionParams);
        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        /////Ein Test für "CheckIfInputIsEmpty"
        /////</summary>
        //[TestMethod()]
        //public void CheckIfInputIsEmptyTest2()
        //{
        //    ConnectionParameters connectionParams = new ConnectionParameters("", "", "", "", "", "", "");

        //    bool expected = false;
        //    bool actual;

        //    actual = DBConnectionHelpers.CheckIfInputIsEmpty(connectionParams);
        //    Assert.AreEqual(expected, actual);
        //}

        /// <summary>
        ///Ein Test für "CheckIfDBNameAlreadyExists"
        ///</summary>
        [TestMethod]
        public void CheckIfDBNameAlreadyExistsTest()
        {
            ConnectionParameters connectionParams = new ConnectionParameters("TestString", "Cname", "TestString", "TestString", "TestString", "TestString", "TestString");
            string connectionName = "Cname";

            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.ConnectionParametersList.Add(connectionParams);

            bool actual;

            actual = DBConnectionHelpers.CheckIfDatabaseNameExists(connectionName);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///Ein Test für "CheckIfDBNameAlreadyExists"
        ///</summary>
        [TestMethod]
        public void CheckIfDBNameAlreadyExistsTest2()
        {
            ConnectionParameters connectionParams = new ConnectionParameters("TestString", "Cname", "TestString", "TestString", "TestString", "TestString", "TestString");
            string connectionName = "OtherName";

            DBConnectionHelpers.ConnectionParametersList = new List<ConnectionParameters>();
            DBConnectionHelpers.ConnectionParametersList.Add(connectionParams);

            bool actual = DBConnectionHelpers.CheckIfDatabaseNameExists(connectionName);
            Assert.IsTrue(actual);
        }
    }
}
