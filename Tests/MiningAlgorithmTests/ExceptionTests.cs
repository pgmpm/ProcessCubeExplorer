using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MiningAlgorithm.Exceptions;

namespace pgmpm.MiningAlgorithmTests
{
    [TestClass]
    public class ExceptionTests
    {

        [TestMethod]
        [ExpectedException(typeof(MultipleStartingEventsFoundException))]
        public void MultipleStartingEventsFoundException1()
        {
            throw new MultipleStartingEventsFoundException();
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleStartingEventsFoundException))]
        public void MultipleStartingEventsFoundException2()
        {
            MultipleStartingEventsFoundException ex = new MultipleStartingEventsFoundException("Text");
            Assert.IsTrue(ex.Message.Equals("Text"));
            throw ex;
        }
        [TestMethod]
        [ExpectedException(typeof(NoStartingEventFoundException))]
        public void NoStartingEventFoundException1()
        {
            throw new NoStartingEventFoundException();
        }

        [TestMethod]
        [ExpectedException(typeof(NoStartingEventFoundException))]
        public void NoStartingEventFoundException2()
        {
            NoStartingEventFoundException ex = new NoStartingEventFoundException("Text");
            Assert.IsTrue(ex.Message.Equals("Text"));
            throw ex;
        }

    }
}
