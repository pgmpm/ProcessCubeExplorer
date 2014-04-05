using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Database.Model;

namespace pgmpm.DBConnectTests
{
    /// <summary>
    ///This is a test class for FactTableTest and is intended
    ///to contain all FactTableTest Unit Tests
    ///</summary>
    ///<autor>Anonymous, Andrej Albrecht</autor>
    [TestClass]
    public class MetaDataRepositoryTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        /// <summary>
        /// A test for FactTable Constructor
        /// </summary>
        [TestMethod]
        public void MetaDataRepositoryConstructorTest()
        {
            MetaDataRepository target = new MetaDataRepository("name");

            Assert.IsNotNull(target.ListOfFactDimensions);
            Assert.IsNotNull(target.ListOfEventDimensions);
            Assert.IsNotNull(target.ListOfCaseTableColumnNames);
            Assert.IsNotNull(target.ListOfEventsTableColumnNames);
        }
    }
}