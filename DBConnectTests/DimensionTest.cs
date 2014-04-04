using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using pgmpm.MatrixSelection.Dimensions;

namespace pgmpm.DBConnectTests
{
    /// <summary>
    ///This is a test class for DimensionTest and is intended
    ///to contain all DimensionTest Unit Tests
    ///</summary>
    ///<autor>Anonymous, Andrej Albrecht</autor>
    [TestClass()]
    public class DimensionTest
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
        ///A test for Dimension Constructor
        ///</summary>
        [TestMethod()]
        public void DimensionConstructorTest()
        {
            Dimension target = new Dimension();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Dimension Constructor
        ///</summary>
        [TestMethod()]
        public void DimensionConstructorTest1()
        {
            string setname = "Teststring";
            string setfrom_constraint = "Teststring";
            string setfrom_table = "Teststring";
            string setfrom_column = "Teststring";
            string setto_constraint = "Teststring";
            string setto_table = "Teststring";
            string setto_column = "Teststring";
            Dimension target = new Dimension(setname, setfrom_constraint, setfrom_table, setfrom_column, setto_constraint, setto_table, setto_column);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for AddDimensionLevel
        ///</summary>
        [TestMethod()]
        public void AddDimensionLevelTest()
        {
            Dimension target = new Dimension();
            Dimension d = new Dimension();
            target.AddDimensionLevel(d);
            Assert.AreEqual(d, target.GetCoarserDimensionLevel()[0]);
        }

        /// <summary>
        ///A test for AddDimensionLevel
        ///</summary>
        [TestMethod()]
        public void AddDimensionLevelTest1()
        {
            Dimension target = new Dimension();
            List<Dimension> l = new List<Dimension>() { new Dimension() };
            target.AddDimensionLevel(l);
            Assert.AreEqual(l[0], target.GetCoarserDimensionLevel()[0]);
        }

        /// <summary>
        ///A test for GetCoarserDimensionLevel
        ///</summary>
        [TestMethod()]
        public void GetCoarserDimensionLevelTest()
        {

            Dimension target = new Dimension();
            Dimension d = new Dimension();
            target.AddDimensionLevel(d);
            Assert.AreEqual(d, target.GetCoarserDimensionLevel()[0]);
        }

        /// <summary>
        ///A test for GetLevel
        ///</summary>
        [TestMethod()]
        public void GetLevelTest()
        {
            Dimension target = new Dimension();
            Dimension d1 = new Dimension();
            Dimension d2 = new Dimension();
            d1.AddDimensionLevel(d2);
            target.AddDimensionLevel(d1);

            List<Dimension> actual = target.GetLevel();
            Assert.AreEqual(actual.Count, 3);
        }

        /// <summary>
        ///A test for FromColumn
        ///</summary>
        [TestMethod()]
        public void from_columnTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.FromColumn = expected;
            actual = target.FromColumn;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FromConstraint
        ///</summary>
        [TestMethod()]
        public void from_constraintTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.FromConstraint = expected;
            actual = target.FromConstraint;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FromTable
        ///</summary>
        [TestMethod()]
        public void from_tableTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.FromTable = expected;
            actual = target.FromTable;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToColumn
        ///</summary>
        [TestMethod()]
        public void to_columnTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.ToColumn = expected;
            actual = target.ToColumn;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToConstraint
        ///</summary>
        [TestMethod()]
        public void to_constraintTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.ToConstraint = expected;
            actual = target.ToConstraint;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToTable
        ///</summary>
        [TestMethod()]
        public void to_tableTest()
        {
            Dimension target = new Dimension();
            string expected = "Teststring";
            string actual;
            target.ToTable = expected;
            actual = target.ToTable;
            Assert.AreEqual(expected, actual);
        }
    }
}
