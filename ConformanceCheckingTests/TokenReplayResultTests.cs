using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.ConformanceChecking;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.ConformanceCheckingTests
{
    [TestClass]
    public class TokenReplayResultTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void ToDictionaryTest()
        {
            var result = new TokenReplayResult
            {
                FailedCasesTransitionNotEnabled = new List<Case> { new Case(), new Case() },
                FailedCasesTransitionNotFound = new List<Case> { new Case() },
                NumberOfCases = 5,
                SuccessfulReplays = 2
            };

            var dict = (Dictionary<string, string>)result.ToDictionary();
            Assert.AreEqual("2", dict["Successful Replays"]);
            Assert.AreEqual("40%", dict["Success Rate"]);
            Assert.AreEqual("2", dict["Transitions not enabled"]);
            Assert.AreEqual("1", dict["Transitions not found"]);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void GetTransitionsNotEnabledAsDictionaryTest()
        {
            var case1 = new Case("case1") { EventList = new List<Event> { new Event("A"), new Event("B") } };
            var case2 = new Case("case2") { EventList = new List<Event> { new Event("C"), new Event("D") } };
            var result = new TokenReplayResult
            {
                FailedCasesTransitionNotEnabled = new List<Case> { case1, case1, case2 },
            };

            var dict = (Dictionary<string, int>)result.GetTransitionsNotEnabledAsDictionary();
            Assert.AreEqual(2, dict["A, B"]);
            Assert.AreEqual(1, dict["C, D"]);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod]
        public void GetTransitionsNotFoundAsDictionaryTest()
        {
            var case1 = new Case("case1") { EventList = new List<Event> { new Event("A"), new Event("B") } };
            var case2 = new Case("case2") { EventList = new List<Event> { new Event("C"), new Event("D") } };
            var result = new TokenReplayResult
            {
                FailedCasesTransitionNotFound = new List<Case> { case1, case1, case2 },
            };

            var dict = (Dictionary<string, int>)result.GetTransitionsNotFoundAsDictionary();
            Assert.AreEqual(2, dict["A, B"]);
            Assert.AreEqual(1, dict["C, D"]);
        }
    }
}
