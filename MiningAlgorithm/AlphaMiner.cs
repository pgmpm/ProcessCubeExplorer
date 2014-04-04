using System.Diagnostics;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm.Exceptions;
using pgmpm.Model;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
using System.Linq;


namespace pgmpm.MiningAlgorithm
{
    /// <summary>
    /// A static class for the Alpha Miner
    /// </summary>
    /// <author>Krystian Zielonka, Christopher Licht</author>
    public class AlphaMiner : IMiner
    {
        private readonly Field _field;
        private PetriNet _petriNet;
        private HashSet<String> _listOfActivities;
        private HashSet<String> _listOfStartActivities;
        private HashSet<String> _listOfEndActivities;
        private List<AlphaMinerPlaces> _listOfAlphaPlaces;
        private Dictionary<String, Tuple<HashSet<String>, HashSet<String>>> _listOfAllPairs;
        private HashSet<String> _listOfAllDoubleActivities;
        private Dictionary<String, HashSet<String>> _listOfAllParallelPairs;
        private Dictionary<String, HashSet<String>> _listOfLoopsWithTheLengthOfTwo;
        private List<List<String>> _listOfActivitiesInCase;

        public AlphaMiner(Field field)
        {
            _field = field;
            _petriNet = new PetriNet(field.Infotext);
            _listOfActivities = new HashSet<String>();
            _listOfStartActivities = new HashSet<String>();
            _listOfEndActivities = new HashSet<String>();
            _listOfAlphaPlaces = new List<AlphaMinerPlaces>();
            _listOfAllPairs = new Dictionary<String, Tuple<HashSet<String>, HashSet<String>>>();
            _listOfLoopsWithTheLengthOfTwo = new Dictionary<String, HashSet<String>>();
            _listOfAllParallelPairs = new Dictionary<String, HashSet<String>>();
            _listOfAllDoubleActivities = new HashSet<String>();
            _listOfActivitiesInCase = new List<List<String>>();
        }

        /// <summary>
        /// Goes through the steps of the AlphaMiner Algorithm:
        /// </summary>
        /// <param Name="field">A field from the DataSelection</param>
        /// <returns>A PetriNet as a ProcessModel</returns>
        /// <exception cref="ArgumentNullException">If the field-parameter is null.</exception>
        /// <author>Krystian Zielonka, Christopher Licht</author>
        public ProcessModel Mine()
        {
            if (_field == null)
                throw new ArgumentNullException("field", "The field parameter was null");

            // Get parameters from MinerSettings
            String alphaMinerSettings = MinerSettings.GetAsString("Alpha Miner");

            // Start - Statistics
            TimeSpan ProcessingTimeStart = Process.GetCurrentProcess().TotalProcessorTime;

            DetectActivitiySet();
            DetectStartAndEndActivitiesInTraces();

            DetectAllPlaces();

            switch (alphaMinerSettings)
            {
                case "AlphaMinerPlus":
                    CorrectParallelPairs();
                    break;
                case "AlphaMinerPlusPlus":
                    Preprocessing();
                    Postprocessing();
                    CorrectParallelPairs();
                    break;
                default:
                    break;
            }

            AddsPlacesTogether();
            RemoveAllDuplicatePlaces();

            BuildTheNet();

            // End - Statistics
            TimeSpan ProcessingTimeEnd = Process.GetCurrentProcess().TotalProcessorTime;

            //Information about the generated petri net
            PetriNetInformation(_field, ref ProcessingTimeStart, ref ProcessingTimeEnd);

            //Check for errors in the field
            DoErrorHandling();

            return _petriNet;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// This method gets the set of activities in the event log
        /// </summary>
        /// <param Name="field">A field from the DataSelection</param>
        /// <author>Christopher Licht</author>
        private void DetectActivitiySet()
        {
            foreach (Event CurrentEvent in _field.EventLog.ListOfUniqueEvents)
                _listOfActivities.Add(CurrentEvent.Name);
        }

        /// <summary>
        /// Detects all start activities in all traces.
        /// </summary>
        /// <param Name="field">A field from the DataSelection</param>
        /// <author>Christopher Licht</author>
        private void DetectStartAndEndActivitiesInTraces()
        {
            foreach (Case CurrentCase in _field.EventLog.Cases)
            {
                _listOfStartActivities.Add(CurrentCase.EventList.First().Name);
                _listOfEndActivities.Add(CurrentCase.EventList.Last().Name);
            }
        }

        /// <summary>
        /// Adds information of the generated petri net to the information-list
        /// </summary>
        /// <param name="field">A field from the DataSelection</param>
        /// <param name="ProcessingTimeStart">the processing time start</param>
        /// <param name="ProcessingTimeEnd">the processing time end</param>
        /// <author>Christopher Licht</author>
        private void PetriNetInformation(Field field, ref TimeSpan ProcessingTimeStart, ref TimeSpan ProcessingTimeEnd)
        {
            _field.Information.Add("Total Processor Time", ((ProcessingTimeEnd - ProcessingTimeStart).TotalMilliseconds) + " ms");
            _field.Information.Add("Number of Events", _petriNet.Transitions.Count.ToString());
            _field.Information.Add("Number of Transitions", _petriNet.Transitions.Count.ToString());
            _field.Information.Add("Number of Places", _petriNet.Places.Count.ToString());
            _field.Information.Add("Events used", _petriNet.CountTransitionsWithoutANDs() + " of " + field.EventLog.ListOfUniqueEvents.Count);
        }

        /// <summary>
        /// Improves the current field
        /// </summary>
        /// <author>Christopher Licht</author>
        private void DoErrorHandling()
        {
            if (_petriNet.Transitions.Count == 0)
                throw new NoStartingEventFoundException("For this selection no events could be found.");
        }

        /// <summary>
        /// Detects all connections: predecessors and followers.
        /// </summary>
        /// <param Name="field">A field from the DataSelection</param>
        /// <author>Krystian Zielonka</author>
        private void DetectAllPlaces()
        {
            foreach (Case CurrentCase in _field.EventLog.Cases)
            {
                List<String> _ListOfPlaces = new List<String>();

                for (int i = 0; i < CurrentCase.EventList.Count - 1; ++i)
                {
                    String PredecessorEventName = (i > 0) ? CurrentCase.EventList[i - 1].Name : "";
                    String CurrentEventName = CurrentCase.EventList[i].Name;
                    String FollowerEventName = CurrentCase.EventList[i + 1].Name;

                    if (!_listOfAllPairs.ContainsKey(CurrentEventName))
                    {
                        _listOfAllPairs.Add(CurrentEventName, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>(), new HashSet<String>() { FollowerEventName }));
                    }
                    else
                    {
                        _listOfAllPairs[CurrentEventName].Item2.Add(FollowerEventName);
                    }

                    if (!_listOfAllPairs.ContainsKey(FollowerEventName))
                    {
                        _listOfAllPairs.Add(FollowerEventName, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>() { CurrentEventName }, new HashSet<String>()));
                    }
                    else
                    {
                        _listOfAllPairs[FollowerEventName].Item1.Add(CurrentEventName);
                    }

                    if (PredecessorEventName.Equals(FollowerEventName) && !CurrentEventName.Equals(FollowerEventName))
                    {
                        if (!_listOfLoopsWithTheLengthOfTwo.ContainsKey(CurrentEventName))
                        {
                            _listOfLoopsWithTheLengthOfTwo.Add(CurrentEventName, new HashSet<String> { FollowerEventName });
                        }
                        else
                        {
                            _listOfLoopsWithTheLengthOfTwo[CurrentEventName].Add(FollowerEventName);
                        }

                        if (!_listOfLoopsWithTheLengthOfTwo.ContainsKey(FollowerEventName))
                        {
                            _listOfLoopsWithTheLengthOfTwo.Add(FollowerEventName, new HashSet<String> { CurrentEventName });
                        }
                        else
                        {
                            _listOfLoopsWithTheLengthOfTwo[FollowerEventName].Add(CurrentEventName);
                        }
                    }

                    if (CurrentEventName.Equals(FollowerEventName))
                    {
                        _listOfAllDoubleActivities.Add(CurrentEventName);
                    }

                    if (i == 0)
                    {
                        _ListOfPlaces.Add(CurrentEventName);
                    }

                    if (!_ListOfPlaces.Last().Equals(FollowerEventName))
                    {
                        _ListOfPlaces.Add(FollowerEventName);
                    }
                }

                _listOfActivitiesInCase.Add(_ListOfPlaces);
            }

            foreach (KeyValuePair<String, Tuple<HashSet<String>, HashSet<String>>> pair in _listOfAllPairs)
            {
                if (pair.Value.Item1.Overlaps(pair.Value.Item2))
                {
                    _listOfAllParallelPairs.Add(pair.Key, new HashSet<String>(pair.Value.Item1.Intersect(pair.Value.Item2)));
                }
            }
        }

        /// <summary>
        /// Correct Parallel Pairs for the AlphaMinerPlus and AlphaMinerPlusPlus.
        /// </summary>
        /// <param Name="field">A field from the DataSelection</param>
        /// <author>Krystian Zielonka</author>
        private void CorrectParallelPairs()
        {
            foreach (KeyValuePair<String, HashSet<String>> pair in _listOfLoopsWithTheLengthOfTwo)
            {
                if (_listOfAllParallelPairs.ContainsKey(pair.Key) && _listOfAllParallelPairs[pair.Key].Overlaps(pair.Value))
                {
                    _listOfAllParallelPairs[pair.Key].ExceptWith(pair.Value);

                    if (!_listOfAllParallelPairs[pair.Key].Any())
                    {
                        _listOfAllParallelPairs.Remove(pair.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Remove duplicate activities if they occur several times in succession.
        /// </summary>
        /// <author>Krystian Zielonka</author>
        private void Preprocessing()
        {
            foreach (String Activity in _listOfAllDoubleActivities)
            {
                _listOfAllPairs[Activity].Item1.ExceptWith(_listOfAllDoubleActivities);
                _listOfAllPairs[Activity].Item2.ExceptWith(_listOfAllDoubleActivities);
            }

            foreach (String DoubleActivitie in _listOfAllDoubleActivities)
            {
                if (_listOfLoopsWithTheLengthOfTwo.Keys.Contains(DoubleActivitie))
                {
                    _listOfLoopsWithTheLengthOfTwo[DoubleActivitie].ExceptWith(_listOfAllDoubleActivities);

                    if (!_listOfLoopsWithTheLengthOfTwo[DoubleActivitie].Any())
                    {
                        _listOfLoopsWithTheLengthOfTwo.Remove(DoubleActivitie);
                    }
                }
            }

            foreach (String DoubleActivitie in _listOfAllDoubleActivities)
            {
                if (_listOfAllParallelPairs.Keys.Contains(DoubleActivitie))
                {
                    _listOfAllParallelPairs[DoubleActivitie].ExceptWith(_listOfAllDoubleActivities);

                    if (!_listOfAllParallelPairs[DoubleActivitie].Any())
                    {
                        _listOfAllParallelPairs.Remove(DoubleActivitie);
                    }
                }
            }
        }

        /// <summary>
        /// Adds activities at the right place again which were removed in the preprocessing method.
        /// </summary>
        /// <author>Krystian Zielonka</author>
        private void Postprocessing()
        {
            foreach (String DoubleActivity in _listOfAllDoubleActivities)
            {
                List<String> _ListOfActivities = new List<String>(_listOfAllDoubleActivities.Except(new List<String>() { DoubleActivity }));

                foreach (List<String> _ListOfCurrentActivityInCase in _listOfActivitiesInCase)
                {
                    if (_ListOfCurrentActivityInCase.Contains(DoubleActivity))
                    {
                        List<String> _ListOfExceptedActivityInCase = new List<String>(_ListOfCurrentActivityInCase.Except(_ListOfActivities));

                        for (int i = 0; i < _ListOfExceptedActivityInCase.Count - 1; ++i)
                        {
                            String CurrentEventName = _ListOfExceptedActivityInCase[i];
                            String FollowerEventName = _ListOfExceptedActivityInCase[i + 1];

                            if (!_listOfAllPairs.ContainsKey(CurrentEventName))
                            {
                                _listOfAllPairs.Add(CurrentEventName, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>(), new HashSet<String>() { FollowerEventName }));
                            }
                            else
                            {
                                _listOfAllPairs[CurrentEventName].Item2.Add(FollowerEventName);
                            }

                            if (!_listOfAllPairs.ContainsKey(FollowerEventName))
                            {
                                _listOfAllPairs.Add(FollowerEventName, new Tuple<HashSet<String>, HashSet<String>>(new HashSet<String>() { CurrentEventName }, new HashSet<String>()));
                            }
                            else
                            {
                                _listOfAllPairs[FollowerEventName].Item1.Add(CurrentEventName);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// link places which have a lot of predecessors and followers
        /// </summary>
        /// <author>Krystian Zielonka</author>
        private void AddsPlacesTogether()
        {
            foreach (KeyValuePair<String, HashSet<String>> pair in _listOfAllParallelPairs)
            {
                if (_listOfAllPairs.ContainsKey(pair.Key) && _listOfAllPairs[pair.Key].Item2.Overlaps(pair.Value))
                {
                    _listOfAllPairs[pair.Key].Item2.ExceptWith(pair.Value);
                    _listOfAllPairs[pair.Key].Item1.ExceptWith(pair.Value);
                }
            }

            foreach (KeyValuePair<String, Tuple<HashSet<String>, HashSet<String>>> Pair in _listOfAllPairs)
            {
                HashSet<String> _ListOfInComingSet = Pair.Value.Item1;
                HashSet<String> _ListOfOutComingSet = Pair.Value.Item2;

                for (int i = 0; i < _ListOfInComingSet.Count; ++i)
                {
                    String InComingEvent_1 = _ListOfInComingSet.ElementAt(i);

                    HashSet<String> _ListForCombinePlaces = new HashSet<String>() { InComingEvent_1 };

                    for (int j = i + 1; j < _ListOfInComingSet.Count; ++j)
                    {
                        String InComingEvent_2 = _ListOfInComingSet.ElementAt(j);

                        if (!_listOfAllParallelPairs.ContainsKey(InComingEvent_1))
                        {
                            _ListForCombinePlaces.Add(InComingEvent_2);
                        }
                        else if (!_listOfAllParallelPairs[InComingEvent_1].Contains(InComingEvent_2))
                        {
                            _ListForCombinePlaces.Add(InComingEvent_2);
                        }
                    }

                    if (_ListForCombinePlaces.Count > 0)
                    {
                        AlphaMinerPlaces NewAlphaPlace = new AlphaMinerPlaces(_ListForCombinePlaces, new HashSet<String>() { Pair.Key });

                        int Count = _listOfAlphaPlaces
                                            .Count(k => k.PredecessorHashSet.SetEquals(NewAlphaPlace.PredecessorHashSet) &&
                                                        k.FollowerHashSet.SetEquals(NewAlphaPlace.FollowerHashSet));
                        if (Count == 0)
                        {
                            _listOfAlphaPlaces.Add(NewAlphaPlace);
                        }
                    }
                }

                for (int i = 0; i < _ListOfOutComingSet.Count; ++i)
                {
                    String OutComingEvent_1 = _ListOfOutComingSet.ElementAt(i);

                    HashSet<String> _ListForCombinePlaces = new HashSet<String>() { OutComingEvent_1 };

                    for (int j = i + 1; j < _ListOfOutComingSet.Count; ++j)
                    {
                        String OutComingEvent_2 = _ListOfOutComingSet.ElementAt(j);

                        if (!_listOfAllParallelPairs.ContainsKey(OutComingEvent_1))
                        {
                            _ListForCombinePlaces.Add(OutComingEvent_2);
                        }
                        else if (!_listOfAllParallelPairs[OutComingEvent_1].Contains(OutComingEvent_2))
                        {
                            _ListForCombinePlaces.Add(OutComingEvent_2);
                        }
                    }

                    if (_ListForCombinePlaces.Count > 0)
                    {
                        AlphaMinerPlaces NewAlphaPlace = new AlphaMinerPlaces(new HashSet<String>() { Pair.Key }, _ListForCombinePlaces);

                        int Count = _listOfAlphaPlaces
                                            .Count(k => k.PredecessorHashSet.SetEquals(NewAlphaPlace.PredecessorHashSet) &&
                                                        k.FollowerHashSet.SetEquals(NewAlphaPlace.FollowerHashSet));
                        if (Count == 0)
                        {
                            _listOfAlphaPlaces.Add(NewAlphaPlace);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes duplicate alphaPlaces out of the _ListOfAlphaPlaces
        /// </summary>
        /// <author>Krystian Zielonka</author>
        private void RemoveAllDuplicatePlaces()
        {
            foreach (AlphaMinerPlaces AlphaPlace in _listOfAlphaPlaces)
            {
                HashSet<String> _ListOfPredecessors = AlphaPlace.PredecessorHashSet;
                HashSet<String> _ListOfFollowers = AlphaPlace.FollowerHashSet;

                List<int> _ListOfActivitiesToDelete = Enumerable.Range(0, _listOfAlphaPlaces.Count).Where(k => !_listOfAlphaPlaces[k].Equals(AlphaPlace) &&
                    (_listOfAlphaPlaces[k].PredecessorHashSet.IsSubsetOf(_ListOfPredecessors) || _ListOfPredecessors.IsSubsetOf(_listOfAlphaPlaces[k].PredecessorHashSet)) &&
                    (_ListOfFollowers.IsSubsetOf(_listOfAlphaPlaces[k].FollowerHashSet) || _listOfAlphaPlaces[k].FollowerHashSet.IsSubsetOf(_ListOfFollowers)) &&
                    (_listOfAlphaPlaces[k].PredecessorHashSet.Any() && _listOfAlphaPlaces[k].FollowerHashSet.Any())).ToList();

                foreach (int Index in _ListOfActivitiesToDelete)
                {
                    _listOfAlphaPlaces[Index].MergePredecessorSet(_ListOfPredecessors);
                    _listOfAlphaPlaces[Index].MergeFollowerSet(_ListOfFollowers);
                }

                if (_ListOfActivitiesToDelete.Count > 0)
                {
                    AlphaPlace.ClearAlphaPlace();
                }
            }

            List<AlphaMinerPlaces> NewAlphaPlace = new List<AlphaMinerPlaces>();

            foreach (AlphaMinerPlaces AlphaPlace in _listOfAlphaPlaces)
            {
                if (AlphaPlace.PredecessorHashSet.Any() && AlphaPlace.FollowerHashSet.Any())
                {
                    NewAlphaPlace.Add(AlphaPlace);
                }
            }

            _listOfAlphaPlaces = NewAlphaPlace;
        }

        /// <summary>
        /// Creates and connects all places and transitions to a complete petri net
        /// </summary>
        /// <author>Christopher Licht</author>
        public void BuildTheNet()
        {
            Place Place;
            int PlaceCounter = 0;

            //Create one place for all startActvities and one place for all endActvities
            Place StartPlace = _petriNet.AddPlace("start", 0);
            Place EndPlace = _petriNet.AddPlace("end", 1);
            PlaceCounter = 1;

            //Create the transitions with the detected activities of step 1
            foreach (String CurrentActivity in _listOfActivities)
                _petriNet.AddTransition(CurrentActivity);

            //Connects all startActvities with the startPlace
            foreach (String CurrentStartActivity in _listOfStartActivities)
                foreach (Transition CurrentTransition in _petriNet.Transitions)
                    if (CurrentTransition.Name.Equals(CurrentStartActivity))
                        CurrentTransition.AddIncomingPlace(StartPlace);

            //Connects all endActvities with the endPlace
            foreach (String CurrentEndActivity in _listOfEndActivities)
                foreach (Transition CurrentTransition in _petriNet.Transitions)
                    if (CurrentTransition.Name.Equals(CurrentEndActivity))
                        CurrentTransition.AddOutgoingPlace(EndPlace);

            //All transitions get their incoming and outgoing places
            foreach (AlphaMinerPlaces CurrentAlphaMinerPlaces in _listOfAlphaPlaces)
            {
                PlaceCounter++;
                Place = _petriNet.AddPlace("", PlaceCounter);

                //Splits the predecessor and followers of the current place
                HashSet<String> _ListOfPredecessor = CurrentAlphaMinerPlaces.PredecessorHashSet;
                HashSet<String> _ListOfFollower = CurrentAlphaMinerPlaces.FollowerHashSet;

                //The current predecessor-transition will be connected to the current place 
                for (int i = 0; i < _ListOfPredecessor.Count; i++)
                    foreach (Transition CurrentTransition in _petriNet.Transitions)
                        if (CurrentTransition.Name.Equals(_ListOfPredecessor.ElementAt(i)))
                            CurrentTransition.AddOutgoingPlace(Place);

                //The current follower-transition will be connected to the current place
                for (int i = 0; i < _ListOfFollower.Count; i++)
                    foreach (Transition CurrentTransition in _petriNet.Transitions)
                        if (CurrentTransition.Name.Equals(_ListOfFollower.ElementAt(i)))
                            CurrentTransition.AddIncomingPlace(Place);
            }
        }
    }
}