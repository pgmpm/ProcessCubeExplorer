using pgmpm.MatrixSelection.Fields;
using pgmpm.Model;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace pgmpm.MiningAlgorithm.InductiveV2
{

    /// <summary>
    /// Basic implementation of the b' algorithm(Inductive Miner)
    /// </summary>
    /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
    public class InductiveMiner : IMiner
    {
        /// <summary>
        /// Corresponding field for this Miner instance.
        /// </summary>
        protected readonly Field _field;

        /// <summary>
        /// The eventDictionary helps to identify GraphNodes via the corresponding event.
        /// </summary>
        protected Dictionary<Event, InductiveMinerGraphNode> EventDictionary;

        /// <summary>
        /// List of all directly follow relations. 
        /// </summary>
        protected List<InductiveMinerRow> DirectRowList;

        /// <summary>
        /// List of all eventually follow relations.
        /// </summary>
        protected List<InductiveMinerRow> EventuallyRowList;

        /// <summary>
        /// Resulting process model
        /// </summary>
        public PetriNet IMPetriNet;


        /// <summary>
        /// Logic start event for all traces
        /// </summary>
        protected Event StartEvent;

        /// <summary>
        /// Logic end event for all places
        /// </summary>
        protected Event EndEvent;

       
        /// <summary>
        /// The inductive miner tree for this instance.
        /// </summary>
        protected InductiveMinerTreeNode IMTree;

        /// <summary>
        /// Initialize the InductiveMiner instance.
        /// </summary>
        /// <param name="field">Corresponding field</param>
        /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
        public InductiveMiner(Field field)
        {
            _field = field;
            DirectRowList = new List<InductiveMinerRow>();
            EventDictionary = new Dictionary<Event, InductiveMinerGraphNode>();
            IMPetriNet = new PetriNet(_field.Infotext);
            StartEvent = new Event("StartEvent");
            EndEvent = new Event("EndEvent");
            EventuallyRowList = new List<InductiveMinerRow>();
        }

        /// <summary>
        /// Starts the mining process
        /// </summary>
        /// <returns>Process model</returns>
        /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
        public virtual ProcessModel Mine()
        {
            if (_field == null)
                throw new ArgumentNullException("field", "The field parameter was null");

            // Statistics
            var processingTimeStart = Process.GetCurrentProcess().TotalProcessorTime;

            DirectFrequency();
            EventualFrequency();
            BuildInitialDirectFollowGraph();
            BuildInitialEventualFollowGraph();

            IMTree = new InductiveMinerTreeNode(IMPetriNet, EventDictionary[StartEvent], StartEvent);

            GeneratePetriNet();

            FinishPetriNet();
            _field.ProcessModel = IMPetriNet;

            EndLog(processingTimeStart);
            return _field.ProcessModel;
        }
     
        /// <summary>
        /// Informations about the mining process
        /// </summary>
        /// <param name="processingTimeStart"></param>
        protected void EndLog(TimeSpan processingTimeStart)
        {
            TimeSpan processingTimeEnd = Process.GetCurrentProcess().TotalProcessorTime;
            var processingTime = Math.Abs((processingTimeEnd - processingTimeStart).TotalMilliseconds) < 1 ? "< 15 ms" : (processingTimeEnd - processingTimeStart).TotalMilliseconds + " ms";
            _field.Information.Add("Processing Time", processingTime);
            _field.Information.Add("Number of Events", IMPetriNet.Transitions.Count(s => s.IsLoop == false).ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Number of Transitions", IMPetriNet.Transitions.Count.ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Number of Places", IMPetriNet.Places.Count.ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Loops in the net", IMPetriNet.CountLoops().ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Events used", IMPetriNet.CountTransitionsWithoutANDs() - IMPetriNet.CountLoops() + " of " + _field.EventLog.ListOfUniqueEvents.Count);
        }

        /// <summary>
        /// Checks each case and each event and saves the directly follower information in preparation for building the eventually follow graph.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        protected void DirectFrequency() 
        {
            EventDictionary.Add(StartEvent, new InductiveMinerGraphNode(StartEvent));
            EventDictionary.Add(EndEvent, new InductiveMinerGraphNode(EndEvent));

            foreach (Case currentCase in _field.EventLog.Cases)
            {
                Event tempEvent = StartEvent;

                foreach (Event currentEvent in currentCase.EventList)
                {
                    CheckAddRowEventDic(tempEvent, currentEvent);
                    tempEvent = currentEvent;
                }
            }
        }

        /// <summary>
        /// Checks the DirectRowList if a row from event A to event B exists. If not a new row is created, else the directly follow count is increased by one.
        /// </summary>
        /// <param name="fromEvent">From event A</param>
        /// <param name="toEvent">To event B</param>
        /// <author>Bernd Nottbeck</author>
        protected void CheckAddRowEventDic(Event fromEvent, Event toEvent)  
        {
            if (!EventDictionary.ContainsKey(toEvent))
                EventDictionary.Add(toEvent, new InductiveMinerGraphNode(toEvent));

            InductiveMinerRow currentRow;

            var query = from SearchRow in DirectRowList
                        where SearchRow.FromNode == EventDictionary[fromEvent] && SearchRow.ToNode == EventDictionary[toEvent]
                        select SearchRow;

            currentRow = query.FirstOrDefault();

            if (currentRow != null)
            {
                currentRow.Count++;
            }
            else
            {
                DirectRowList.Add(new InductiveMinerRow(EventDictionary[fromEvent], EventDictionary[toEvent]));
            }
        }

        /// <summary>
        /// Builds the initial directly follow graph.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        protected void BuildInitialDirectFollowGraph()
        {
            foreach (InductiveMinerRow currentRow in DirectRowList)
            {
                EventDictionary[currentRow.FromNode.Name].FollowerList.Add(currentRow);
            }

            foreach (KeyValuePair<Event, InductiveMinerGraphNode> pair in EventDictionary)
            {
                pair.Value.OrderFollowerLists();
            }
        }

        /// <summary>
        /// Checks each case and each event and saves the eventually follower information in preparation for building the eventually follow graph.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        protected void EventualFrequency()
        {
            foreach (Case currentCase in _field.EventLog.Cases)
            {
                Event RootEvent = StartEvent;

                for (int i = 0; i < currentCase.EventList.Count(); i++)
                {
                    for (int m = i; m < currentCase.EventList.Count(); m++)
                    {
                        Event currentEvent = currentCase.EventList[m];
                        CheckAddRowEventualFollowsDic(RootEvent, currentEvent);
                    }
                    RootEvent = currentCase.EventList[i];
                }

            }
        }

        /// <summary>
        /// Checks the eventuallyRowList if a row from event A to event B exists. If not a new row is created, else the directly follow count is increased by one.
        /// </summary>
        /// <param name="fromEvent">From event A</param>
        /// <param name="toEvent">To event B</param>
        /// <author>Bernd Nottbeck, Krystian Zielonka </author>
        protected void CheckAddRowEventualFollowsDic(Event fromEvent, Event toEvent)
        {
            if (!EventDictionary.ContainsKey(toEvent))
                throw new Exception("Something is wrong! Key Missing in event dictionary");

            InductiveMinerRow currentRow;

            var query = from SearchRow in EventuallyRowList
                        where SearchRow.FromNode == EventDictionary[fromEvent] && SearchRow.ToNode == EventDictionary[toEvent]
                        select SearchRow;

            currentRow = query.FirstOrDefault();

            if (currentRow != null)
            {
                currentRow.Count++;
            }
            else
            {
                EventuallyRowList.Add(new InductiveMinerRow(EventDictionary[fromEvent], EventDictionary[toEvent]));
            }
        }

        /// <summary>
        ///  Builds the initial eventually follow graph.
        /// </summary>
        /// <author> Bernd Nottbeck, Krystian Zielonka</author>
        protected void BuildInitialEventualFollowGraph()
        {
            foreach (InductiveMinerRow currentRow in EventuallyRowList)
            {
                EventDictionary[currentRow.FromNode.Name].EventualFollowerList.Add(currentRow);
            }

            foreach (KeyValuePair<Event, InductiveMinerGraphNode> pair in EventDictionary)
            {
                pair.Value.OrderFollowerLists();
            }

        }

        #region Draw PetriNet

        /// <summary>
        /// Draws an initial place and starts the draw tree cycle. 
        /// </summary>
        /// <returns>true if the tree was successfully drawn</returns>
        /// <author>Thomas Meents, Bernd Nottbeck</author>
        public bool GeneratePetriNet()
        {
            Place myPlace = new Place("Start");
            IMPetriNet.Places.Add(myPlace);
            Place endPlace = TraverseTreePetrinet(IMTree, myPlace);
            endPlace.Name = "End";
            return true;
        }

        /// <summary>
        /// Recursively moves threw the tree and generates the IMPetrinet
        /// </summary>
        /// <param name="node">actual node</param>
        /// <param name="relayedPlace">Place that the process operator get</param>
        /// <param name="relayedOutgoingPlace">optional place that will be overwrite the outgoingplace</param>
        /// <param name="relayedIncomingPlace">optional place that will be overwrite the incomingplace</param>
        /// <author>Thomas Meents, Bernd Nottbeck</author>
        private Place TraverseTreePetrinet(InductiveMinerTreeNode node, Place relayedPlace, Place relayedOutgoingPlace = null, Place relayedIncomingPlace = null)
        {
            if (node.Operation.Equals(OperationsEnum.isLeaf))
            {
                return DrawLeafPetrinet(node, relayedPlace, overwriteIncomingPlace: relayedIncomingPlace, overwriteOutgoingPlace: relayedOutgoingPlace);
            }

            if (node.Operation.Equals(OperationsEnum.isSequence))
            {
                if (node.LeftLeaf != null)
                {
                    Place temp = TraverseTreePetrinet(node.LeftLeaf, relayedPlace);
                    if (node.RightLeaf != null)
                        temp = TraverseTreePetrinet(node.RightLeaf, temp);
                    return temp;
                }
            }
            else if (node.Operation.Equals(OperationsEnum.isXOR))
            {
                if (node.LeftLeaf != null)
                {
                    Place tempXORExitPlace = TraverseTreePetrinet(node.LeftLeaf, relayedPlace);

                    if (node.RightLeaf == null) 
                        return null;

                    Place newPlace = TraverseTreePetrinet(node.RightLeaf, tempXORExitPlace, relayedIncomingPlace: relayedPlace, relayedOutgoingPlace: tempXORExitPlace);

                        if (tempXORExitPlace != null)
                        return tempXORExitPlace;
                    return newPlace;
                }
            }
            else if (node.Operation.Equals(OperationsEnum.isLoop))
            {
                Place tempLoopEntrancePlace = relayedPlace;

                if (node.LeftLeaf != null)
                {
                    Place temp = TraverseTreePetrinet(node.LeftLeaf, relayedPlace);
                    

                    Place tempLoopExitPlace = temp;

                    if (node.RightLeaf != null)
                    {
                        //prevents a Nullpointer-Exception if the first Place is a Loop.
                        if (tempLoopEntrancePlace != IMPetriNet.Places[0])
                        {
                            temp = TraverseTreePetrinet(node.RightLeaf, temp);
                            IMPetriNet.AddTransition("Loop", incomingPlace: temp, outgoingPlace: tempLoopEntrancePlace,
                                isLoop: true);
                        }
                        else
                        {
                            temp = TraverseTreePetrinet(node.RightLeaf, temp, relayedOutgoingPlace: tempLoopEntrancePlace);
                            if (tempLoopExitPlace != null)
                                temp = tempLoopExitPlace; 
                        }
                    }
                    return temp;
                }
            }
            else if (node.Operation.Equals(OperationsEnum.isParallel))
            {
                Transition ANDSplit = new Transition("AND-Split");
                ANDSplit.AddIncomingPlace(relayedPlace);
                IMPetriNet.Transitions.Add(ANDSplit);

                Place NewPlaceLeft = new Place();
                Place NewPlaceRight = new Place();

                IMPetriNet.Places.Add(NewPlaceLeft);
                IMPetriNet.Places.Add(NewPlaceRight);

                ANDSplit.AddOutgoingPlace(NewPlaceLeft);
                ANDSplit.AddOutgoingPlace(NewPlaceRight);

                Transition ANDJoin = new Transition("AND-Join");
                Place AndJoinOutgoingPlace = new Place();
                ANDJoin.AddOutgoingPlace(AndJoinOutgoingPlace);
                IMPetriNet.Places.Add(AndJoinOutgoingPlace);
                IMPetriNet.Transitions.Add(ANDJoin);

                if (node.LeftLeaf != null)
                {
                    Place temp = TraverseTreePetrinet(node.LeftLeaf, relayedPlace, relayedIncomingPlace: NewPlaceLeft);
                    ANDJoin.AddIncomingPlace(temp);

                    if (node.RightLeaf != null)
                    {
                        temp = TraverseTreePetrinet(node.RightLeaf, NewPlaceRight);
                        ANDJoin.AddIncomingPlace(temp);
                    }
                    return AndJoinOutgoingPlace;
                }
            }
            else
            {
                throw new Exception("Something in the process tree is wrong.");
            }

            return relayedPlace;
        }

        /// <summary>
        /// Draws the transition with the incoming and outgoing places.
        /// </summary>
        /// <param name="node">Node that will be drawn</param>
        /// <param name="relayed">Relayed place. It will be the incoming place of the transition</param>
        /// <param name="overwriteOutgoingPlace">optional place that will be overwrite the outgoingplace</param>
        /// <param name="overwriteIncomingPlace">optional place that will be overwrite the Incomingplace</param>
        /// <returns>Outgoing Place of the drawn Transition</returns>
        /// <author>Thomas Meents</author>
        private Place DrawLeafPetrinet(InductiveMinerTreeNode node, Place relayed, Place overwriteOutgoingPlace = null, Place overwriteIncomingPlace = null)
        {
            if (!node.Operation.Equals(OperationsEnum.isLeaf))
                throw new Exception("Only leafs can be drawn.");

            Place outgoing = new Place();
            IMPetriNet.Places.Add(outgoing);

            if (overwriteIncomingPlace != null)
                relayed = overwriteIncomingPlace;

            Transition transition = new Transition(node.Event.Name) { IsDrawn = false };
            transition.AddIncomingPlace(relayed);
            transition.AddOutgoingPlace(outgoing);
            if (overwriteOutgoingPlace != null)
            {
                transition.OutgoingPlaces.Remove(outgoing);
                transition.OutgoingPlaces.Add(overwriteOutgoingPlace);
            }
            IMPetriNet.Transitions.Add(transition);

            return outgoing;
        }

        private void FinishPetriNet()
        {
            List<Place> places = IMPetriNet.SinkPlaces.Where(p => p.Name != "End").ToList();
            IMPetriNet.Places.RemoveAll(p => places.Contains(p));        
        }

        #endregion Draw PetriNet
    }
}
