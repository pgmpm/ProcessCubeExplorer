using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Dimensions;

namespace pgmpm.DBConnectTests
{
    /// <summary>
    ///This is a test class for DimensionContentTest and is intended
    ///to contain all DimensionContentTest Unit Tests
    ///</summary>
    ///<autor>Anonymous, Andrej Albrecht</autor>
    [TestClass()]
    public class DimensionContentTest
    {


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
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DimensionContent Constructor
        ///</summary>
        [TestMethod()]
        public void DimensionContentConstructorTest()
        {
            string set_id = "Teststring";
            string set_content = "Teststring";
            string set_desc = "Teststring";
            DimensionContent target = new DimensionContent(set_id, set_content, set_desc);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for content
        ///</summary>
        [TestMethod()]
        public void ContentTest()
        {
            string set_id = "Teststring";
            string set_content = "Teststring";
            string set_desc = "Teststring";
            DimensionContent target = new DimensionContent(set_id, set_content, set_desc);
            string expected = "Teststring";
            string actual;
            target.Content = expected;
            actual = target.Content;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            string set_id = "Teststring";
            string set_content = "Teststring";
            string set_desc = "Teststring";
            DimensionContent target = new DimensionContent(set_id, set_content, set_desc);
            string expected = "Teststring";
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for id
        ///</summary>
        [TestMethod()]
        public void IdTest()
        {
            string set_id = "Teststring";
            string set_content = "Teststring";
            string set_desc = "Teststring";
            DimensionContent target = new DimensionContent(set_id, set_content, set_desc);
            string expected = "Teststring";
            string actual;
            target.Id = expected;
            actual = target.Id;
            Assert.AreEqual(expected, actual);
        }
    }
}
