using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.Model.PetriNet;

namespace pgmpm.ModelTests.PetriNet
{
    [TestClass]
    public class PetriNetOperationTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void CleanUpPetriNetTest()
        {
            var petriNet = new Model.PetriNet.PetriNet();
            var place1 = petriNet.AddPlace("1");
            var place2 = petriNet.AddPlace("2");
            var place3 = petriNet.AddPlace("3");
            var place4 = petriNet.AddPlace("4");
            petriNet.AddTransition("eins", incomingPlace: place1, outgoingPlace: place2);
            petriNet.AddTransition("", incomingPlace: place2, outgoingPlace: place3);
            petriNet.AddTransition("drei", incomingPlace: place3, outgoingPlace: place4);

            petriNet = PetriNetOperation.CleanUpPetriNet(petriNet);
            Assert.IsTrue(petriNet.Transitions.Count == 2 && petriNet.Places.Count == 3, "Transitions =" + petriNet.Transitions.Count + " Places =" + petriNet.Places.Count);
        }
    }
}
