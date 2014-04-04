/*
 * Copyright 2014 Jannik Arndt, Projektgruppe MPM. All Rights Reserved.
 *
 * This file is part of Process Cube Explorer.
 *
 * Process Cube Explorer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Cube Explorer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Cube Explorer. If not, see <http://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm.Exceptions;
using pgmpm.Model;
using pgmpm.Model.PetriNet;
using EventLog = pgmpm.MatrixSelection.Fields.EventLog;

[assembly: CLSCompliant(false)]
namespace pgmpm.MiningAlgorithm
{
    /// <summary>
    /// A static class for the Heuristic Miner
    /// </summary>
    /// <author>Jannik Arndt</author>
    public class HeuristicMiner : IMiner
    {
        private readonly Field _field;

        /// <summary>
        /// A flat version of all EventNodes, mostly for .Information
        /// </summary>
        private readonly HashSet<double> _listOfNodes;

        /// <summary>
        /// If there are multiple process-models they will be split into sub-petrinets and eventually combined
        /// </summary>
        private readonly List<PetriNet> _listOfSubPetriNets;
        /// <summary>
        /// This stack stores whenever a parallelism (AND or XOR) is opened. The most recent one is at the top (use peek() and pop())
        /// </summary>
        private readonly Stack<Parallelism> _openParallelismCount;

        /// <summary>
        /// The petrinet that will be returned
        /// </summary>
        private PetriNet _finalPetriNet;

        /// <summary>
        /// Constructor for the HeuristicMiner, initializes values and sets the "field" that will be mined.
        /// </summary>
        /// <param name="field">A field from the DataSelection</param>
        public HeuristicMiner(Field field)
        {
            _field = field;
            _finalPetriNet = new PetriNet(field.Infotext);
            _listOfSubPetriNets = new List<PetriNet>();
            _openParallelismCount = new Stack<Parallelism>();
            _listOfNodes = new HashSet<double>();
        }

        /// <summary>
        /// Goes through the steps of the (improved) HeuristicMiner Algorithm:
        /// Find starting-events => build adjacency-matrix => convert into petrinet
        /// </summary>
        /// <returns>A PetriNet as a ProcessModel</returns>
        /// <exception cref="ArgumentNullException">If the field-parameter is null.</exception>
        /// <author>Jannik Arndt</author>
        public ProcessModel Mine()
        {
            if (_field == null)
                throw new Exception("The field parameter was null");

            // Get parameters from MinerSettings
            var threshold = MinerSettings.GetAsDouble("AdjacencyThresholdSlider") / 100;
            var maxDepth = MinerSettings.GetAsInt("MaximumRecursionDepthSlider");

            if (threshold < 0 || threshold > 1)
                throw new Exception("Threshold must be between 0 and 1.");
            if (maxDepth < 1)
                throw new Exception("Maximum recursion depth must be more greater than 1.");

            // Statistics
            var processingTimeStart = Process.GetCurrentProcess().TotalProcessorTime;

            // 1. Create Adjacency Matrix
            var adjacencyMatrix = CreateAdjacencyMatrix(_field);

            // 2. Calculate the indexes of the first events
            var startindexes = GetColumnsWithOnlyNegativeEntries(adjacencyMatrix);

            // for every sub-processmodel
            foreach (var startindex in startindexes)
            {
                // 3. Create the dependency-Graph
                var rootNode = CreateDependencyGraph(_field.EventLog.ListOfUniqueEvents, adjacencyMatrix, threshold, 0.0, startindex, maxDepth);

                // 4. Turn the dependency-graph into a petri-net
                _listOfSubPetriNets.Add(CreatePetriNetFromDependencyGraph(rootNode, _field.EventLog, _field.Infotext));
            }

            // 5. Connect all Petrinets by one XOR-Split
            if (_listOfSubPetriNets.Count > 1)
            {
                // 5a. Connect all subnets to one start
                var startingPlace = _finalPetriNet.AddPlace("Start");
                foreach (var petriNet in _listOfSubPetriNets)
                {
                    var xorTransition = _finalPetriNet.AddTransition(incomingPlace: startingPlace);
                    _finalPetriNet.MergeWithPetriNet(petriNet, xorTransition);
                }
                // 5b. Add following transitions to all subnet-endings
                foreach (var openEndPlace in _finalPetriNet.GetSinks())
                    if (openEndPlace.GetType() == typeof(Place))
                        _finalPetriNet.AddTransition(incomingPlace: (Place)openEndPlace);
            }
            else
                _finalPetriNet = _listOfSubPetriNets[0];

            // 6. Connect all open endings
            _finalPetriNet = FixEnding(_finalPetriNet);

            // 7. Delete empty transitions
            _finalPetriNet = PetriNetOperation.CleanUpPetriNet(_finalPetriNet);

            // 8. Save information about the mining-process
            var processingTimeEnd = Process.GetCurrentProcess().TotalProcessorTime;
            var processingTime = Math.Abs((processingTimeEnd - processingTimeStart).TotalMilliseconds) < 1 ? "< 15 ms" : (processingTimeEnd - processingTimeStart).TotalMilliseconds + " ms";
            _field.Information.Add("Processing Time", processingTime);
            _field.Information.Add("Number of Events", _finalPetriNet.Transitions.Count(s => s.IsLoop == false).ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Number of Transitions", _finalPetriNet.Transitions.Count.ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Number of Places", _finalPetriNet.Places.Count.ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Loops in the net", _finalPetriNet.CountLoops().ToString(CultureInfo.InvariantCulture));
            _field.Information.Add("Events used", _finalPetriNet.CountTransitionsWithoutANDs() - _finalPetriNet.CountLoops() + " of " + _field.EventLog.ListOfUniqueEvents.Count);
            _field.Information.Add("Parallel Models", startindexes.Count.ToString(CultureInfo.InvariantCulture));
            if (_listOfNodes.Count > 0)
            {
                _field.Information.Add("Minimal Adjacency", _listOfNodes.Min().ToString(CultureInfo.InvariantCulture));
                _field.Information.Add("Average Adjacency", Math.Round(_listOfNodes.Average(), 3).ToString(CultureInfo.InvariantCulture));
                if (Math.Abs(_listOfNodes.Min() - _listOfNodes.Average()) > 0.0001)
                    _field.Information.Add("Standard Deviation", CalculateStandardDeviation(_listOfNodes).ToString(CultureInfo.InvariantCulture));
            }

            return _finalPetriNet;
        }

        /// <summary>
        /// Goes through the dictionary and calls GetAdjacency on every combination of Events to create an adjacency matrix.
        /// </summary>
        /// <param name="field">A field of the data selection</param>
        /// <returns>Returns an adjacency matrix</returns>
        /// <author>Jannik Arndt</author>
        public double[,] CreateAdjacencyMatrix(Field field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "The given field is null");

            int count = field.EventLog.ListOfUniqueEvents.Count;

            var adjacencyMatrix = new double[count, count];

            for (var row = 0; row < count; row++)
            {
                for (var column = 0; column < count; column++)
                {
                    // The lower triangular matrix is the negative compliment of the upper one.
                    if (column < row)
                        adjacencyMatrix[row, column] = -adjacencyMatrix[column, row];
                    else
                    {
                        Event event1 = field.EventLog.ListOfUniqueEvents[row];
                        Event event2 = field.EventLog.ListOfUniqueEvents[column];
                        Double adjacency = GetAdjacency(event1, event2, field.EventLog);
                        adjacencyMatrix[row, column] = Math.Round(adjacency, 3);
                    }
                }
            }
            return adjacencyMatrix;
        }

        /// <summary>
        /// Calculates the adjacency of two events in a given EventLog. Called by CreateAdjacencyMatrix()
        /// </summary>
        /// <param name="event1">First Event</param>
        /// <param name="event2">Second Event</param>
        /// <param name="log">The EventLog to be searched</param>
        /// <returns>The adjacency of two events in an eventlog</returns>
        /// <author>Jannik Arndt</author>
        public double GetAdjacency(Event event1, Event event2, EventLog log)
        {
            if (event1 == null)
                throw new ArgumentNullException("event1", "The given arguments are not valid");
            if (event2 == null)
                throw new ArgumentNullException("event2", "The given arguments are not valid");
            if (log == null)
                throw new ArgumentNullException("log", "The given arguments are not valid");

            var event1ToEvent2 = log.EventFollowsEvent(event1, event2);
            var event2ToEvent1 = log.EventFollowsEvent(event2, event1);
            return Convert.ToDouble(event1ToEvent2 - event2ToEvent1) / Convert.ToDouble(event1ToEvent2 + event2ToEvent1 + 1);
        }

        /// <summary>
        /// Recursive method to build the dependency graph, which essentially is a bunch of EventNodes put together.
        /// </summary>
        /// <param name="eventlist">A list of all events, automatically generated when creating the dictionary.</param>
        /// <param name="adjacencyMatrix">A filled adjacency matrix.</param>
        /// <param name="threshold">A Value between 0.5 and 1</param>
        /// <param name="adjacency">The current adjacency</param>
        /// <param name="row">The index of the first event.</param>
        /// <param name="maxDepth">Since this is recursive, there is the danger of a StackOverflow, this prevents it. If you choose wisely.</param>
        /// <returns>The root-EventNode of the graph.</returns>
        /// <author>Jannik Arndt</author>
        public EventNode CreateDependencyGraph(List<Event> eventlist, double[,] adjacencyMatrix, double threshold, double adjacency, int row, int maxDepth)
        {
            // exit condition
            if (maxDepth < 1)
                return null;

            var eventNode = new EventNode(eventlist[row], adjacency);

            // build a flat list as well
            if (adjacency > threshold)
                _listOfNodes.Add(adjacency);

            // Recursively add all child-nodes
            foreach (var index in GetFollowingEventsIndexes(adjacencyMatrix, row, threshold))
                eventNode.ListOfFollowers.Add(CreateDependencyGraph(eventlist, adjacencyMatrix, threshold, adjacencyMatrix[row, index], index, maxDepth - 1));

            return eventNode;
        }

        /// <summary>
        /// Checks all columns to see which ones have only negative entries (these usually are the starting events)
        /// </summary>
        /// <param name="adjacencyMatrix">A filled adjacency matrix.</param>
        /// <returns>A List of indexes which correspondent to the indexes in the ListOfEvents.</returns>
        /// <author>Jannik Arndt</author>
        public List<int> GetColumnsWithOnlyNegativeEntries(double[,] adjacencyMatrix)
        {
            // go through all columns until you find the right one
            var result = new List<int>();
            for (var index = 0; index < adjacencyMatrix.GetLength(0); index++)
                if (AllRowEntriesAreNegative(adjacencyMatrix, index))
                    result.Add(index);

            if (result.Count == 0)
                throw new NoStartingEventFoundException("For this selection no starting event could be found.");
            return result;
        }

        /// <summary>
        /// Returns true if all entries in the given column are negative. Used to find the start-event.
        /// </summary>
        /// <param name="adjacencyMatrix">The adjacency-matrix that is searched</param>
        /// <param name="column">The column of the matrix</param>
        /// <returns>true if all entries are negative or zero</returns>
        public bool AllRowEntriesAreNegative(double[,] adjacencyMatrix, int column)
        {
            // go through all rows until there is a positive entry
            for (var index = 0; index < adjacencyMatrix.GetLength(0); index++)
                if (adjacencyMatrix[index, column] > 0)
                    return false;
            return true;
        }

        /// <summary>
        /// Looks up the indices of the events that have an adjacency higher than the threshold
        /// </summary>
        /// <param name="adjacencyMatrix">A filled adjacency matrix.</param>
        /// <param name="row">The index of the row to start with</param>
        /// <param name="threshold">A Value between 0.5 and 1</param>
        /// <returns>A list of indices, with witch events from the ListOfEvents can be looked up.</returns>
        /// <author>Jannik Arndt</author>
        public List<int> GetFollowingEventsIndexes(double[,] adjacencyMatrix, int row, double threshold)
        {
            var resultList = new List<int>();
            for (var index = 0; index < adjacencyMatrix.GetLength(0); index++)
                if (adjacencyMatrix[row, index] > threshold)
                    resultList.Add(index);
            return resultList;
        }

        /// <summary>
        /// Goes through the dependency-graph and creates a petrinet.
        /// </summary>
        /// <param name="dependencyGraph">An EventNode from CreateDependencyGraph()</param>
        /// <param name="eventLog">The EventLog to calculate AND and XOR-relations</param>
        /// <param name="name">The name of the new PetriNet</param>
        /// <returns>A complete petrinet</returns>
        /// <author>Jannik Arndt, Bernhard Bruns</author>
        public PetriNet CreatePetriNetFromDependencyGraph(EventNode dependencyGraph, EventLog eventLog, string name)
        {
            var petriNet = new PetriNet(name);

            // 1. first event
            var startingPlace = petriNet.AddPlace("", 1);
            var firstEvent = petriNet.AddTransition(dependencyGraph.InnerEvent.Name, incomingPlace: startingPlace);

            // 2. Go through the net and turn Nodes into Places and Transitions, with parallel structures
            HandleNodesAndCloseParallelisms(firstEvent, dependencyGraph, eventLog, petriNet);

            // 3. handle loops
            petriNet = HandleLoops(eventLog, petriNet);

            return petriNet;
        }

        /// <summary>
        /// Connects all open endings to one end-place. This is useful for multiple processmodels in one net or for loops at the end of the net.
        /// </summary>
        /// <param name="petriNet">A petrinet with possible open endings</param>
        /// <returns>The given PetriNet with a fixed ending</returns>
        /// <author>Jannik Arndt</author>
        public PetriNet FixEnding(PetriNet petriNet)
        {
            if (petriNet.GetTransitionsWithoutFollowersIgnoreLoops().Count > 0)
            {
                var endingPlace = petriNet.AddPlace("End");
                foreach (var transition in petriNet.GetTransitionsWithoutFollowersIgnoreLoops())
                {
                    if (transition.OutgoingPlaces.Count > 0)
                    {
                        var temporaryTransition = petriNet.AddTransition();
                        transition.OutgoingPlaces[0].AppendOutgoingTransition(temporaryTransition);
                        temporaryTransition.AddIncomingPlace(transition.OutgoingPlaces[0]);
                        temporaryTransition.AddOutgoingPlace(endingPlace);
                    }
                    else
                        transition.AddOutgoingPlace(endingPlace);
                }
            }
            return petriNet;
        }

        /// <summary>
        /// A recursive function to call the next level of HandleNode. Also closes open XORs and ANDs right away.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="node"></param>
        /// <param name="log"></param>
        /// <param name="petriNet"></param>
        /// <author>Jannik Arndt</author>
        public void HandleNodesAndCloseParallelisms(Transition trans, EventNode node, EventLog log, PetriNet petriNet)
        {
            var transitions = HandleNode(trans, node, log, petriNet);
            if (transitions == null)
                return;

            foreach (var transition in transitions)
                foreach (var eventNode in node.ListOfFollowers)
                    if (transition.Name == eventNode.InnerEvent.Name || transition.IsANDJoin)
                        HandleNodesAndCloseParallelisms(transition, eventNode, log, petriNet);
        }

        /// <summary>
        /// Depending on the count of followers this adds one (or more) places and their following transitions.
        /// </summary>
        /// <param name="event1"></param>
        /// <param name="node">An event node</param>
        /// <param name="log">The current event log</param>
        /// <param name="petriNet"></param>
        /// <returns>The newly added transitions. This is where you need to continue working.</returns>
        /// <exception cref="NotImplementedException">If there are more than 3 followers in a non-trivial relation.</exception>
        /// <author>Jannik Arndt</author>
        public List<Transition> HandleNode(Transition event1, EventNode node, EventLog log, PetriNet petriNet)
        {
            // Case: No followers
            if (node.ListOfFollowers.Count == 0)
                return new List<Transition>();

            // one or more more followers => count the AND-relations
            var andRelations = CountANDRelations(node, log);

            // Case: All nodes are AND-related
            if (andRelations == node.ListOfFollowers.Count)
                return StartAND(event1, node.ListOfFollowers, petriNet);

            // Case: All nodes are XOR-related
            if (andRelations == 0)
                return StartXOR(event1, node.ListOfFollowers, petriNet);

            // Case: 3 Followers
            if (node.ListOfFollowers.Count == 3)
            {
                var x = node;
                var a = node.ListOfFollowers[0];
                var b = node.ListOfFollowers[1];
                var c = node.ListOfFollowers[2];

                if (andRelations == 2) // XOR-Relations == 1
                {
                    // There are two and-relations and one xor-relation. Find the xor and order the parameters accordingly
                    if (IsXorRelation(x.InnerEvent, b.InnerEvent, c.InnerEvent, log))
                        return StartAand_BxorC(event1, a, b, c, petriNet);
                    if (IsXorRelation(x.InnerEvent, a.InnerEvent, c.InnerEvent, log))
                        return StartAand_BxorC(event1, b, a, c, petriNet);
                    if (IsXorRelation(x.InnerEvent, a.InnerEvent, b.InnerEvent, log))
                        return StartAand_BxorC(event1, c, a, b, petriNet);
                }
                else // XOR-Relations == 2 && AND-Relations == 1
                {
                    // There are two xor-relations and one and-relation. Find the and and order the parameters accordingly
                    if (IsAndRelation(x.InnerEvent, b.InnerEvent, c.InnerEvent, log))
                        return StartAxor_BandC(event1, a, b, c, petriNet);
                    if (IsAndRelation(x.InnerEvent, a.InnerEvent, c.InnerEvent, log))
                        return StartAxor_BandC(event1, b, a, c, petriNet);
                    if (IsAndRelation(x.InnerEvent, a.InnerEvent, b.InnerEvent, log))
                        return StartAxor_BandC(event1, c, a, b, petriNet);
                }
            }
            if (node.ListOfFollowers.Count > 3)
                return StartXOR(event1, node.ListOfFollowers, petriNet);

            // optional transition
            if (node.ListOfFollowers.Count == 2)
                if (log.EventFollowsEvent(node.ListOfFollowers[0].InnerEvent, node.ListOfFollowers[1].InnerEvent) > 0)
                    return StartOptionalTransition(event1, node.ListOfFollowers[0].InnerEvent.Name, node.ListOfFollowers[1].InnerEvent.Name, petriNet);
                else if (log.EventFollowsEvent(node.ListOfFollowers[1].InnerEvent, node.ListOfFollowers[0].InnerEvent) > 0)
                    return StartOptionalTransition(event1, node.ListOfFollowers[1].InnerEvent.Name, node.ListOfFollowers[0].InnerEvent.Name, petriNet);

            return null;
        }

        /// <summary>
        /// Calculates the standard deviation of a List of doubles
        /// http://www.developer.com/net/article.php/3794146/Adding-Standard-Deviation-to-LINQ.htm
        /// </summary>
        /// <param name="givenValues"></param>
        /// <returns>Returns the rounded deviation value</returns>
        /// <author>Jannik Arndt</author>
        public double CalculateStandardDeviation(IEnumerable<double> givenValues)
        {
            var values = givenValues as IList<double> ?? givenValues.ToList();
            if (!values.Any()) return 0;

            //Compute the Average
            var average = values.Average();

            //Perform the Sum of (value-avg)^2
            var sum = values.Sum(d => Math.Pow(d - average, 2));

            //Put it all together
            var result = Math.Sqrt((sum) / Convert.ToDouble(values.Count() - 1));
            return Math.Round(result, 3);
        }

        /// <summary>
        /// Find the first common successor of a list of EventNodes. This takes the first EventNode and checks whether its followers occur in all of the other EventNodes (Breadth-first search).
        /// If this does not work it will take all followers of these followers and runs the same method on them (technically now Depth-first).
        /// </summary>
        /// <param name="eventNodes">A List of EventNodes</param>
        /// <returns>The name of the first successor all the EventNodes have in common.</returns>
        /// <author>Jannik Arndt</author>
        public String FindFirstCommonSuccessor(List<EventNode> eventNodes)
        {
            if (eventNodes.Count == 0)
                return "";
            if (eventNodes.Count == 1)
            {
                var follower = eventNodes.First().ListOfFollowers.FirstOrDefault();
                if (follower != null)
                    return follower.InnerEvent.Name;
            }

            var possibleResult = "";

            // Look at all Followers of the first EventNode
            foreach (var eventNode in eventNodes[0].ListOfFollowers)
            {
                possibleResult = eventNode.InnerEvent.Name;
                var resultFound = false;
                // In all other EventNodes try to find the same Name. If found in all: return that name. If not found in any skip to the next EventNode (break).
                foreach (var comparedNode in eventNodes.Skip(1))
                {
                    if (comparedNode.FindNode(eventNode.InnerEvent.Name) == null)
                        break;
                    resultFound = true;
                }
                if (resultFound)
                    return possibleResult;
            }
            // If the common Successor is NOT in the ListOfFollowers of the first EventNode, try all their followers.
            foreach (var newList in eventNodes[0].ListOfFollowers.Select(eventNode => new List<EventNode>()))
            {
                newList.AddRange(eventNodes.First().ListOfFollowers.ToList());
                newList.AddRange(eventNodes.Skip(1));
                possibleResult = FindFirstCommonSuccessor(newList);
                if (possibleResult != null)
                    return possibleResult;
            }
            return possibleResult;
        }

        /// <summary>
        /// Adds an optional Transition to the net, as well as the next mandatory transition.
        /// </summary>
        /// <param name="startingTransition">The existing transition</param>
        /// <param name="optionalAction">The optional transition</param>
        /// <param name="commonAction">The mandatory transition</param>
        /// <param name="petriNet"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> StartOptionalTransition(Transition startingTransition, String optionalAction, String commonAction, PetriNet petriNet)
        {
            var place1 = petriNet.AddPlace();
            var place2 = petriNet.AddPlace();
            startingTransition.AddOutgoingPlace(place1);
            petriNet.AddTransition(optionalAction, incomingPlace: place1, outgoingPlace: place2);
            var commonTransition = petriNet.AddTransition(commonAction, new List<Place> { place1, place2 });
            return new List<Transition> { commonTransition };
        }

        #region Start and end parallelisms

        /// <summary>
        /// Adds a place and the transitions it leads to.
        /// </summary>
        /// <param name="startingTransition">>Contains the starting transition</param>
        /// <param name="followers">node.followers</param>
        /// <param name="petriNet">Contains the current petri net</param>
        /// <returns>The two transitions that are in an XOR-relation.</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> StartXOR(Transition startingTransition, List<EventNode> followers, PetriNet petriNet)
        {
            if (followers.Count > 1)
                for (var index = 0; index < followers.Count; index++)
                    _openParallelismCount.Push(Parallelism.Xor);

            var newPlace = petriNet.AddPlace();
            var listOfFollowingTransitions = new List<Transition>();
            foreach (var eventNode in followers)
            {
                if (eventNode == null)
                    break;
                /* _____________________          __________         ____________________________
                 * | startingTransition |  --->  ( NewPlace )  --->  |  NewFollowingTransition  |
                 * '''''''''''''''''''''          **********         ''''''''''''''''''''''''''''
                 */
                var newFollowingTransition = petriNet.FindTransition(eventNode.InnerEvent.Name);
                // The following transition already exists => close something
                if (newFollowingTransition != null)
                {
                    if (_openParallelismCount.Count > 0)
                        switch (_openParallelismCount.Peek())
                        {
                            // Close XOR
                            case Parallelism.Xor:
                                newFollowingTransition.IncomingPlaces.Remove(newPlace);
                                startingTransition.AddOutgoingPlace(newFollowingTransition.IncomingPlaces.First());
                                _openParallelismCount.Pop();
                                break;
                            // Close AND
                            case Parallelism.And:
                                newFollowingTransition.AddIncomingPlace(newPlace);
                                startingTransition.AddOutgoingPlace(newPlace);
                                _openParallelismCount.Pop();
                                break;
                        }
                }
                // Open XOR
                else
                {
                    newFollowingTransition = petriNet.AddTransition(eventNode.InnerEvent.Name, incomingPlace: newPlace);
                    if (newPlace.IncomingTransitions.Count == 0)
                        startingTransition.AddOutgoingPlace(newPlace);
                }

                listOfFollowingTransitions.Add(newFollowingTransition);

            }
            if (newPlace.IncomingTransitions.Count == 0 && newPlace.OutgoingTransitions.Count == 0)
                petriNet.Places.Remove(newPlace);
            return listOfFollowingTransitions;
        }

        /// <summary>
        /// Adds as many places as following events that each lead to a transition.
        /// </summary>
        /// <param name="startingTransition">Contains the starting transition</param>
        /// <param name="followers">node.followers</param>
        /// <param name="petriNet">Contains the current petri net</param>
        /// <returns>The transitions that are in an AND-relation.</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> StartAND(Transition startingTransition, List<EventNode> followers, PetriNet petriNet)
        {
            for (var index = 0; index < followers.Count; index++)
                _openParallelismCount.Push(Parallelism.And);
            var listOfFollowingTransitions = new List<Transition>();

            if (startingTransition.Name == "")
                startingTransition.Name = "AND Split";
            startingTransition.IsANDSplit = true;

            foreach (var node in followers)
            {
                var newPlace = petriNet.AddPlace();

                listOfFollowingTransitions.Add(petriNet.AddTransition(node.InnerEvent.Name, incomingPlace: newPlace));
                startingTransition.AddOutgoingPlace(newPlace);
            }
            return listOfFollowingTransitions;
        }

        /// <summary>
        /// Resolves a construct of three parallel events: a AND ( b XOR c )        
        /// </summary>
        /// <param name="event1">One event</param>
        /// <param name="a">An event node</param>
        /// <param name="b">An event node</param>
        /// <param name="c">An event node</param>
        /// <param name="petriNet">Current petri net</param>
        /// <returns>A list with final transitions</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> StartAand_BxorC(Transition event1, EventNode a, EventNode b, EventNode c, PetriNet petriNet)
        {
            var finalTransition = petriNet.AddTransition("AND Join");
            finalTransition.IsANDJoin = true;

            var andTransition = StartAND(event1, new List<EventNode> { a }, petriNet);
            EndAND(andTransition, petriNet, finalTransition);

            var xorTransition = StartXOR(event1, new List<EventNode> { b, c }, petriNet);
            EndXOR(xorTransition, petriNet, finalTransition);

            return new List<Transition> { finalTransition };
        }

        /// <summary>
        /// Resolves a construct of three parallel events: a XOR ( b AND c )
        /// </summary>
        /// <param name="event1">Event number one</param>
        /// <param name="a">An Event node</param>
        /// <param name="b">An Event node</param>
        /// <param name="c">An Event node</param>
        /// <param name="petriNet">The current petri net</param>
        /// <returns>A list with final transition</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> StartAxor_BandC(Transition event1, EventNode a, EventNode b, EventNode c, PetriNet petriNet)
        {
            var finalTransition = petriNet.AddTransition(); // empty transition, will possibly be cleaned up
            finalTransition.Name = FindFirstCommonSuccessor(new List<EventNode> { a, b, c });

            var xorTransition = StartXOR(event1, new List<EventNode> { a }, petriNet);
            EndXOR(xorTransition, petriNet, finalTransition);

            var andSplit = petriNet.AddTransition(); // no idea where this disappears...
            andSplit.AddIncomingPlace(xorTransition[0].IncomingPlaces[0]); // XOR-Split-Place

            var andTransition = StartAND(andSplit, new List<EventNode> { b, c }, petriNet);
            var andJoin = petriNet.AddTransition("AND Join");
            andJoin.IsANDJoin = true;
            var andTransition2 = EndAND(andTransition, petriNet, andJoin);

            andTransition2[0].AddOutgoingPlace(finalTransition.IncomingPlaces.FirstOrDefault());

            return new List<Transition> { finalTransition };
        }

        /// <summary>
        /// Adds a place and connects the open transitions to it. Then puts the given transition behind that whole thing.
        /// </summary>
        /// <param name="startingTransition">The starting transition</param>
        /// <param name="petriNet">The current petri net</param>
        /// <param name="endingTransition">The ending transition, if it exists already. Otherwise a new one is created.</param>
        /// <returns>The empty transition after the place that combines to XOR.</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> EndXOR(List<Transition> startingTransition, PetriNet petriNet, Transition endingTransition = null)
        {
            if (endingTransition == null)
                endingTransition = petriNet.AddTransition();

            if (_openParallelismCount.Count > 0)
                _openParallelismCount.Pop();

            var newPlace = petriNet.AddPlace();
            foreach (var transition in startingTransition)
                transition.AddOutgoingPlace(newPlace);
            endingTransition.AddIncomingPlace(newPlace);
            return new List<Transition> { endingTransition };
        }

        /// <summary>
        /// Adds a place for each transition, then combines these places in the given transition.
        /// </summary>
        /// <param name="startingTransition">The starting transition</param>
        /// <param name="petriNet">The current petri net</param>
        /// <param name="endingTransition">The ending transition, if it exists already. Otherwise a new one is created></param>
        /// <returns>The transition where the places after the AND-related-events are connected.</returns>
        /// <author>Jannik Arndt</author>
        public List<Transition> EndAND(List<Transition> startingTransition, PetriNet petriNet, Transition endingTransition = null)
        {
            if (endingTransition == null)
                endingTransition = petriNet.AddTransition();

            if (_openParallelismCount.Count > 0)
                _openParallelismCount.Pop();

            var listOfClosingPlaces = new List<Place>();
            foreach (var transition in startingTransition)
            {
                var newPlace = petriNet.AddPlace();
                transition.AddOutgoingPlace(newPlace);
                listOfClosingPlaces.Add(newPlace);
            }
            endingTransition.AddIncomingPlaces(listOfClosingPlaces);
            return new List<Transition> { endingTransition };
        }

        #endregion

        #region Check relations

        /// <summary>
        /// Calculates whether b and c (both followers of a) are in an AND-relation.
        /// </summary>
        /// <param name="a">Event a</param>
        /// <param name="b">Event b</param>
        /// <param name="c">Event c</param>
        /// <param name="log">The current event log</param>
        /// <returns>Returns a result bigger than 0.1</returns>
        /// <author>Jannik Arndt</author>
        public bool IsAndRelation(Event a, Event b, Event c, EventLog log)
        {
            var result = Convert.ToDouble(log.EventFollowsEvent(b, c) + log.EventFollowsEvent(c, b)) / Convert.ToDouble(log.EventFollowsEvent(a, b) + log.EventFollowsEvent(a, c) + 1);
            return result > 0.1;
        }


        /// <summary>
        /// Calculates whether b and c (both followers of a) are in an XOR-relation.
        /// </summary>
        /// <param name="a">First event</param>
        /// <param name="b">Second event</param>
        /// <param name="c">Third event</param>
        /// <param name="log">The current event log</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public bool IsXorRelation(Event a, Event b, Event c, EventLog log)
        {
            return !IsAndRelation(a, b, c, log);
        }

        /// <summary>
        /// Counts the amount of following AND-relations to a given node
        /// </summary>
        /// <param name="node">Current event node</param>
        /// <param name="log">Current event log</param>
        /// <returns>Returns the counted number of and-relations</returns>
        /// <author>Jannik Arndt</author>
        public int CountANDRelations(EventNode node, EventLog log)
        {
            var result = 0;
            for (var index = 0; index < node.ListOfFollowers.Count; index++)
                for (var index2 = index + 1; index2 < node.ListOfFollowers.Count; index2++)
                {
                    try
                    {
                        if (node.ListOfFollowers[index] != null && node.ListOfFollowers[index2] != null)
                            if (IsAndRelation(node.InnerEvent, node.ListOfFollowers[index].InnerEvent, node.ListOfFollowers[index2].InnerEvent, log))
                                result++;
                    }
                    catch (NullReferenceException)
                    {
                        throw new NullReferenceException("Cannot count AND-relations");
                    }
                }
            return result;
        }

        #endregion

        # region Loops

        /// <summary>
        /// Find loops in the eventlog and add them to the petrinet
        /// </summary>
        /// <param name="eventLog">The eventlog that is searched for loops</param>
        /// <param name="petriNet">The net the loops are added to</param>
        /// <returns>A petrinet with added loops</returns>
        /// <author>Jannik Arndt</author>
        public PetriNet HandleLoops(EventLog eventLog, PetriNet petriNet)
        {
            var loopedActivities = FindLoopingActivities(eventLog);

            foreach (var activity in loopedActivities)
                petriNet.AddLoop(activity);

            return petriNet;
        }

        /// <summary>
        /// Go through the eventlog and find all events that occur multiple times in a row
        /// </summary>
        /// <param name="eventLog">Any eventlog</param>
        /// <returns>A HashSet of strings (activity-names)</returns>
        /// <author>Jannik Arndt</author>
        private IEnumerable<string> FindLoopingActivities(EventLog eventLog)
        {
            var loopedActivities = new HashSet<string>();
            var currentActivity = "";

            foreach (var Case in eventLog.Cases)
                foreach (var Event in Case.EventList)
                    // if the current event and the event before have the same name, there is a loop
                    if (Event.Name == currentActivity)
                        loopedActivities.Add(Event.Name);
                    else
                        currentActivity = Event.Name;

            return loopedActivities;
        }

        #endregion

        /// <summary>
        /// This classifies parallelisms, used for the static Stack of Parallelism OpenParallelismCount
        /// </summary>
        enum Parallelism
        {
            And,
            Xor
        };
    }
}