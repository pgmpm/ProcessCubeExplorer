using System;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model;
using pgmpm.Model.PetriNet;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// The Token Replay Algorithm
    /// </summary>
    /// <autor>Jannik Arndt</autor>
    public static class TokenReplayAlgorithm
    {
        /// <summary>
        /// Replays the content of the EventLog on the given PetriNet
        /// </summary>
        /// <param name="petriNet"></param>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public static TokenReplayResult Replay(PetriNet petriNet, EventLog eventLog)
        {
            var result = new TokenReplayResult { NumberOfCases = eventLog.Cases.Count };

            foreach (var Case in eventLog.Cases)
            {
                var exceptionThrown = false;
                petriNet.ResetTokens();
                foreach (var Event in Case.EventList)
                {
                    try
                    {
                        petriNet.FireTransition(Event.Name);
                    }
                    catch (NullReferenceException)
                    {
                        result.FailedCasesTransitionNotFound.Add(Case);
                        exceptionThrown = true;
                        break;
                    }
                    catch (TransitionNotEnabledException)
                    {
                        result.FailedCasesTransitionNotEnabled.Add(Case);
                        exceptionThrown = true;
                        break;
                    }
                }
                if (!exceptionThrown)
                    result.SuccessfulReplays++;
            }
            return result;
        }
    }
}
