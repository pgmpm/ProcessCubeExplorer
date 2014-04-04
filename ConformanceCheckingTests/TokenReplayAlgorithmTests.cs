using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ConformanceChecking;
using pgmpm.ExampleData;

namespace pgmpm.ConformanceCheckingTests
{
    /// <summary>
    /// Tests for the token replay algorithm
    /// </summary>
    [TestClass]
    public class TokenReplayAlgorithmTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void ReplayTest()
        {
            var petriNet = PetriNetExample.OneTwoThreeFourFive();
            var eventLog = EventLogExample.OneTwoThreeFourFive();
            var actual = TokenReplayAlgorithm.Replay(petriNet, eventLog);

            Assert.AreEqual(5, actual.NumberOfCases);
            Assert.AreEqual(5, actual.SuccessfulReplays);
            Assert.AreEqual(0, actual.FailedCasesTransitionNotEnabled.Count);
            Assert.AreEqual(0, actual.FailedCasesTransitionNotFound.Count);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void ReplayTestFail()
        {
            var petriNet = PetriNetExample.OneTwoThreeFourFive();
            var eventLog = EventLogExample.OneTwoThreeFourFiveWithErrors();
            var actual = TokenReplayAlgorithm.Replay(petriNet, eventLog);

            Assert.AreEqual(3, actual.NumberOfCases);
            Assert.AreEqual(1, actual.SuccessfulReplays);
            Assert.AreEqual(1, actual.FailedCasesTransitionNotEnabled.Count);
            Assert.AreEqual(1, actual.FailedCasesTransitionNotFound.Count);
        }
    }
}
