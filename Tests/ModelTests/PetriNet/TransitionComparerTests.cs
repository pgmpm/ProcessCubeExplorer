using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Model.PetriNet;

namespace pgmpm.ModelTests.PetriNet
{
    [TestClass]
    public class TransitionComparerTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void GetHashCodeTest()
        {
            var transition = new Transition("Test");

            var comparer = new TransitionComparer();

            var expected = "Test".GetHashCode();
            var actual = comparer.GetHashCode(transition);

            Assert.AreEqual(expected, actual);

            actual = comparer.GetHashCode(null);
            Assert.AreEqual(0, actual);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void EqualsTest()
        {
            var transition1 = new Transition("Test");
            var transition2 = new Transition("Test");
            var comparer = new TransitionComparer();

            Assert.IsTrue(comparer.Equals(transition1, transition2));
            Assert.IsTrue(comparer.Equals(transition1, transition1));
            Assert.IsFalse(comparer.Equals(transition1, null));
            Assert.IsFalse(comparer.Equals(transition2, null));
        }
    }
}
