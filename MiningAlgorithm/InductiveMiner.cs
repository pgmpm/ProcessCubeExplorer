using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm.Exceptions;
using pgmpm.Model;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pgmpm.MiningAlgorithm
{
    //public delegate void doSomethingHandler();

    /// <summary>
    /// A static class for the Inductive Miner
    /// </summary>
    /// <author>Krystian Zielonka, Naby Sow</author>
    public static class InductiveMiner
    {
        private static List<InductiveMinerTrace> _Cases;
        private static Dictionary<String, Tuple<HashSet<String>, HashSet<String>>> _ListOfAllConnections;
        private static Dictionary<String, HashSet<String>> _ListOfSequences;

        /// <summary>
        /// Goes through the inductive miner algorithm
        /// </summary>
        /// <param name="field">A field from the data selection</param>
        /// <returns>A PetriNet as a ProcessModel</returns>
        public static ProcessModel Mine(Field field)
        {    
            if (field == null)
                throw new ArgumentNullException("field", "The field parameter was null");

            TraceFrequency(field);
            DetectAllConnections();
            DivideAndConquer();

            return AlphaMiner.Mine(field);
        }

        private static void TraceFrequency(Field field)
        {
            _Cases = new List<InductiveMinerTrace>();

            foreach (Case CurrentCase in field.EventLog.Cases)
            {
                List<String> CurrentList = new List<String>();

                foreach (Event CurrentEvent in CurrentCase.EventList)
                {
                    CurrentList.Add(CurrentEvent.Name);
                }

                int index = _Cases.FindIndex(c => c.Trace.SequenceEqual(CurrentList));

                if (index < 0)
                {
                    _Cases.Add(new InductiveMinerTrace(CurrentList));
                }
                else
                {
                    ++_Cases[index].Frequency;
                }
            }

            foreach (InductiveMinerTrace CurrentLog in _Cases)
            {
                Console.WriteLine("Log: {0}, Anzahl: {1}",
                    string.Join(" ", CurrentLog.Trace),
                    CurrentLog.Frequency);
            }
        }

        private static void DetectAllConnections()
        {
            _ListOfAllConnections = new Dictionary<String, Tuple<HashSet<String>, HashSet<String>>>();

            foreach (InductiveMinerTrace CurrentTrace in _Cases)
            {
                for (int i = 0; i < CurrentTrace.Trace.Count - 1; ++i)
                {
                    String CurrentEvent = CurrentTrace.Trace[i];
                    String FollowerEvent = CurrentTrace.Trace[i + 1];

                    if (!_ListOfAllConnections.ContainsKey(CurrentEvent))
                    {
                        _ListOfAllConnections.Add(CurrentEvent, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>(), new HashSet<String>() { FollowerEvent }));
                    }
                    else
                    {
                        _ListOfAllConnections[CurrentEvent].Item2.Add(FollowerEvent);
                    }

                    if (!_ListOfAllConnections.ContainsKey(FollowerEvent))
                    {
                        _ListOfAllConnections.Add(FollowerEvent, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>() { CurrentEvent }, new HashSet<String>()));
                    }
                    else
                    {
                        _ListOfAllConnections[FollowerEvent].Item1.Add(CurrentEvent);
                    }
                }
            }

            foreach (KeyValuePair<String, Tuple<HashSet<String>, HashSet<String>>> pair in _ListOfAllConnections)
            {
                Console.WriteLine("{0} < {1} > {2}",
                    pair.Key,
                    string.Join(",", pair.Value.Item1),
                    string.Join(",", pair.Value.Item2));
            }
        }

        private static void DivideAndConquer()
        {
            _ListOfSequences = new Dictionary<String, HashSet<String>>();


        }
    }
}