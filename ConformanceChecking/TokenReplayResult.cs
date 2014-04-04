using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// Stores all information on the Token Replay-Process
    /// </summary>
    /// <autor>Jannik Arndt</autor>
    public class TokenReplayResult
    {
        public List<Case> FailedCasesTransitionNotEnabled = new List<Case>();
        public List<Case> FailedCasesTransitionNotFound = new List<Case>();
        public int SuccessfulReplays { get; set; }
        public int NumberOfCases { get; set; }

        /// <summary>
        /// Returns a dictionary to show the stored data in a ListView
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public IEnumerable ToDictionary()
        {
            return new Dictionary<string, string>
            {
                {"Successful Replays", SuccessfulReplays.ToString(CultureInfo.InvariantCulture)},
                {"Success Rate", (( 1.0 * SuccessfulReplays) / NumberOfCases * 100).ToString(CultureInfo.InvariantCulture) + "%"},
                {"Transitions not enabled", FailedCasesTransitionNotEnabled.Count.ToString(CultureInfo.InvariantCulture)},
                {"Transitions not found", FailedCasesTransitionNotFound.Count.ToString(CultureInfo.InvariantCulture)},
            };
        }

        /// <summary>
        /// Returns a dictionary with the strings of all the cases, that could not be replayed because a transition was not enabled, and the number of occurrences.
        /// </summary>
        /// <returns>A dictionary of the case and number of occurrences</returns>
        /// <author>Jannik Arndt</author>
        public IEnumerable GetTransitionsNotEnabledAsDictionary()
        {
            var result = new Dictionary<string, int>();
            foreach (var failedCase in FailedCasesTransitionNotEnabled)
                if (!result.ContainsKey(failedCase.ToString()))
                    result.Add(failedCase.ToString(), 1);
                else
                    result[failedCase.ToString()]++;
            return result;
        }

        /// <summary>
        /// Returns a dictionary with the strings of all the cases, that could not be replayed because a transition was not found, and the number of occurrences.
        /// </summary>
        /// <returns>A dictionary of the case and number of occurrences</returns>
        /// <author>Jannik Arndt</author>
        public IEnumerable GetTransitionsNotFoundAsDictionary()
        {
            var result = new Dictionary<string, int>();
            foreach (Case failedCase in FailedCasesTransitionNotFound)
                if (!result.ContainsKey(failedCase.ToString()))
                    result.Add(failedCase.ToString(), 1);
                else
                    result[failedCase.ToString()]++;
            return result;
        }
    }
}
