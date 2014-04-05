using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Dimensions;

namespace MainV2Tests.FunctionsTests
{
    /// <summary>
    ///This is a test class for CipherUtilityTest and is intended
    ///to contain all CipherUtilityTest Unit Tests
    ///</summary>
    //////<author>Moritz Eversmann, Bernhard Bruns</author>
    [TestClass]
    public class XMLHelperTest
    {
        public static ConnectionParameters ConParams;
        public static string Path;
        public static MetaDataRepository XmlMetadata;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

            String dbDatabaseWithoutPathName = ConParams.Database.Substring(ConParams.Database.LastIndexOf(("\\"), StringComparison.Ordinal) + 1);
            Path = AppDomain.CurrentDomain.BaseDirectory + @"\Metadata_" + dbDatabaseWithoutPathName + "@" + ConParams.Host + ".xml";

            DBWorker.ConfigureDBConnection(ConParams);
            DBWorker.OpenConnection();

            DBWorker.BuildMetadataRepository();
            XMLHelper.SerializeObjectToXML(Path, DBWorker.MetaData);

            XmlMetadata = XMLHelper.DeserializeObjectFromXML<MetaDataRepository>(Path);
        }

        #endregion

        /// <summary>
        ///Test for "SerializeObjectToXML"
        ///</summary>
        [TestMethod]
        public void SerializeObjectToXMLTest1()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\Testdata_Facttable.xml";


            XMLHelper.SerializeObjectToXML(filepath, DBWorker.MetaData);
            Assert.IsTrue(File.Exists(filepath));
        }

        /// <summary>
        ///Test for "SynchronizeFactTableWithXML"
        ///</summary>
        [TestMethod]
        public void SynchronizeFactTableWithXMLTest()
        {
            Assert.IsTrue(XMLHelper.SynchronizeFactTableWithXML(ConParams));
        }

        /// <summary>
        ///Test for "SynchronizeFactTableWithXML"
        ///</summary>
        [TestMethod]
        public void SynchronizeFactTableWithXMLTest2()
        {
            ConnectionParameters c = new ConnectionParameters();
            Assert.IsFalse(XMLHelper.SynchronizeFactTableWithXML(c));
        }

        /// <summary>
        ///Test for "SynchronizeFactTableWithXML"
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void SynchronizeFactTableWithXMLTest3()
        {
            XMLHelper.SynchronizeFactTableWithXML(null);
        }


        /// <summary>
        ///Test for "SynchronizeFactTableWithXML"
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeserializeObjectFromXML()
        {
            XMLHelper.DeserializeObjectFromXML<MetaDataRepository>("");
        }


        /// <summary>
        ///Test for "CompareDimension"
        ///Dimensions with different count of the Dimensionlevellist
        ///</summary>
        [TestMethod]
        public void CompareDimensionTest()
        {
            Dimension d1 = new Dimension();
            Dimension d2 = new Dimension();

            d1.DimensionLevelsList = new List<Dimension> {new Dimension()};

            d2.DimensionLevelsList = new List<Dimension>();

            PrivateType pt = new PrivateType(typeof(XMLHelper));
            bool result = (bool)pt.InvokeStatic("CompareDimensions", new Object[] { d1, d2 });

            Assert.IsFalse(result);
        }

        /// <summary>
        ///Test for "CompareDimension"
        ///Dimensions with different count of the Dimensionlevellist
        ///</summary>
        [TestMethod]
        public void CompareDimensionTest2()
        {
            Dimension d1 = new Dimension();
            Dimension d2 = new Dimension();

            d1.DimensionLevelsList = new List<Dimension>();
            d1.DimensionLevelsList.Add(new Dimension());
            d1.DimensionLevelsList[0].DimensionLevelsList.Add(new Dimension());
            d1.DimensionLevelsList.Add(new Dimension());

            d2.DimensionLevelsList = new List<Dimension>();
            d2.DimensionLevelsList.Add(new Dimension());

            PrivateType pt = new PrivateType(typeof(XMLHelper));
            bool result = (bool)pt.InvokeStatic("CompareDimensions", new Object[] { d1, d2 });

            Assert.IsFalse(result);
        }

        /// <summary>
        ///Test for "CompareXMLWithMetadata"
        ///If there is no classifier in the classifierList
        ///</summary>
        [TestMethod]
        public void CompareXMLWithMetadata()
        {
            DBWorker.MetaData.EventClassifier = "NotAvailable";

            PrivateType pt = new PrivateType(typeof(XMLHelper));
            bool result = (bool)pt.InvokeStatic("CompareXMLWithMetadata", new Object[] { DBWorker.MetaData, XmlMetadata });
            DBWorker.MetaData.EventClassifier = "";

            Assert.IsFalse(result);
        }

        /// <summary>
        ///Test for "CompareXMLWithMetadata"
        ///If the count of the events is not equal
        ///</summary>
        [TestMethod]
        public void CompareXMLWithMetadata1()
        {
            XmlMetadata.ListOfFactDimensions.Add(new Dimension());

            PrivateType pt = new PrivateType(typeof(XMLHelper));
            bool result = (bool)pt.InvokeStatic("CompareXMLWithMetadata", new Object[] { DBWorker.MetaData, XmlMetadata });

            Assert.IsFalse(result);
        }

    }
}
