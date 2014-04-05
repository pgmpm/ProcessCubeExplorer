using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MatrixSelectionTests
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void EventConstructorTest1()
        {
            Event e = new Event();
            Assert.AreEqual("", e.Name);

            e.Name = "1234Test";
            Assert.AreEqual("1234Test", e.Name);

            if (e.Information.Count != 0) Assert.Fail("Event Information.Count != 0");
        }


        [TestMethod]
        public void EventConstructorTest2()
        {
            Event e = new Event("Test");
            Assert.AreEqual("Test", e.Name);
        }

        [TestMethod]
        public void EventEqualsTest()
        {
            Event e = new Event("Test");
            Event e2 = new Event("Test2");
            Event e3 = new Event("Test2");

            if (e.Equals(e2))
            {
                Assert.Fail("Event.equals check failed!");
            }

            if (!e2.Equals(e3))
            {
                Assert.Fail("Event.equals check failed!");
            }
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            Event ev = new Event { Name = "Event" };
            Assert.IsTrue(ev.Name.GetHashCode() == ev.GetHashCode());
        }

        /// <author>Thomas Meents</author>
        [TestMethod()]
        public void ToStringTest()
        {
            Event ev = new Event { Name = "Event" };
            Assert.IsTrue(ev.Name == ev.ToString());
        }
    }
}
