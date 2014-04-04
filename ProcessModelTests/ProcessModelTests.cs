using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pgmpm.ModelTests
{
    /// <summary>
    /// Tests for the abstract ProcessModel class
    /// </summary>
    /// <author>Jannik Arndt</author>
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void IsOfKindTest()
        {
            var petriNet = new Model.PetriNet.PetriNet();
            var actual = petriNet.IsOfKind();

            Assert.AreEqual("PetriNet", actual);
        }

        [TestMethod]
        public void DeepCopyTest()
        {
            var petriNet1 = new Model.PetriNet.PetriNet();
            petriNet1.AddTransition("Test");

            var copy = (Model.PetriNet.PetriNet)petriNet1.DeepCopy();
            copy.Transitions[0].Name = "Test1";

            Assert.AreNotEqual(copy.Transitions[0].Name, petriNet1.Transitions[0].Name);
        }
    }
}
