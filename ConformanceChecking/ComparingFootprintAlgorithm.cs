using System;
using System.Collections.Generic;
using System.Linq;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// The Comparing Footprints Algorithm
    /// compares two Event-Logs,
    /// an Event-Log and a Process-Model (Petri-Net) or
    /// two Process-Models (Petri-Net)
    /// and shows the Differences between in a Result Comparing-Footprint-Matrix.
    /// The Fitness can calculated for the Result matrix.
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class ComparingFootprintAlgorithm
    {
        /// <summary>
        /// The method create a Footprint for an Eventlog
        /// </summary>
        /// <param name="eventLog">EventLog</param>
        /// <returns>Returns a ComparingFootprintMatrix</returns>
        /// <autor>Andrej Albrecht</autor>
        public static ComparingFootprint CreateFootprint(EventLog eventLog)
        {
            List<ComparingFootprint> footprintList = new List<ComparingFootprint>();

            foreach (Case Case in eventLog.Cases)
                footprintList.Add(CreateFootprint(Case, eventLog.Cases));

            return MergeFootprints(footprintList);
        }

        /// <summary>
        /// create a single footprint from a list of footprints
        /// </summary>
        /// <param name="footprintList">A list of footprints</param>
        /// <returns>A Footprint</returns>
        /// <autor>Andrej Albrecht</autor>
        public static ComparingFootprint MergeFootprints(List<ComparingFootprint> footprintList)
        {
            ComparingFootprint footprintResult = new ComparingFootprint();
            footprintResult.MergeFootprints(footprintList);
            return footprintResult;
        }

        /// <summary>
        /// Create a footprint from one case
        /// </summary>
        /// <param name="c">A case</param>
        /// <param name="cases">A list of cases</param>
        /// <autor>Andrej Albrecht</autor>
        public static ComparingFootprint CreateFootprint(Case c, List<Case> cases = null)
        {
            ComparingFootprint footprint = new ComparingFootprint();

            foreach (Event Event in c.EventList)
                footprint.AddEventHeader(Event.Name);

            int numberOfEventsInCase = c.EventList.Count;
            CellType[,] resultMatrix = new CellType[numberOfEventsInCase, numberOfEventsInCase];

            for (int row = 0; row < footprint.HeaderWithEventNames.Count; row++)
            {
                for (int column = 0; column < footprint.HeaderWithEventNames.Count; column++)
                {
                    //first of all, reset the cellstate to nothing:
                    resultMatrix[row, column] = CellType.Nothing;

                    //Go through all events of the case
                    for (int eventIndex = 0; eventIndex < c.EventList.Count - 1; eventIndex++)
                    {
                        String leftEventName = c.EventList.ElementAt(eventIndex).Name;
                        String rightEventName = c.EventList.ElementAt(eventIndex + 1).Name;

                        if (footprint.HeaderWithEventNames[row].Equals(leftEventName) && footprint.HeaderWithEventNames[column].Equals(rightEventName) &&
                            leftEventName.Equals(rightEventName))
                        {
                            resultMatrix[row, column] = CellType.Loop;
                            resultMatrix[row, column] = CellType.Loop;
                        }

                        else if (footprint.HeaderWithEventNames[row].Equals(leftEventName) && footprint.HeaderWithEventNames[column].Equals(rightEventName))
                        {
                            if (resultMatrix[row, column].Equals(CellType.Left))
                                resultMatrix[row, column] = CellType.Parallel;

                            else if (resultMatrix[row, column].Equals(CellType.Nothing))
                            {
                                resultMatrix[row, column] = CellType.Right;

                                if (row != column)
                                {
                                    resultMatrix[column, row] = CellType.Left;
                                    //ResultMatrix[Row, Column] = CellType.Left;
                                }
                                    
                            }
                        }

                        else if (footprint.HeaderWithEventNames[row].Equals(rightEventName) && footprint.HeaderWithEventNames[column].Equals(leftEventName))
                        {
                            if (resultMatrix[row, column].Equals(CellType.Right))
                                resultMatrix[row, column] = CellType.Parallel;

                            else if (resultMatrix[row, column].Equals(CellType.Nothing)){
                                resultMatrix[row, column] = CellType.Left;
                            }
                            else if (resultMatrix[row, column].Equals(CellType.Parallel))
                                resultMatrix[row, column] = CellType.Parallel;
                        }
                        
                    }
                }
            }

            footprint.ResultMatrix = resultMatrix;

            return footprint;
        }

        /// <summary>
        /// The method checks a cell from a list of cases if the cell is parallel
        /// </summary>
        /// <param name="transition1">Eventname 1</param>
        /// <param name="transition2">Eventname 2</param>
        /// <param name="caseList">A list of cases</param>
        /// <returns>A boolean if the cellstate is parallel</returns>
        /// <autor>Andrej Albrecht</autor>
        // ReSharper disable once UnusedMember.Local
        private static bool CheckParallelism(string transition1, string transition2, List<Case> caseList)
        {
            //This parallelism check is redundant if the Y and X headername is the same! Therefore return false.
            if (transition1.Equals(transition2)) return false;

            // Example for the next two lines, that check the follower:
            // Case 1: Y ->    A   -> X && X ->    B   -> Y //
            // Case 2: Y -> B -> C -> X && X -> D -> E -> Y // -> X and Y are parallel!
            bool fromYtoX = CheckFollower(transition1, transition2, caseList);
            bool fromXtoY = CheckFollower(transition2, transition1, caseList);

            if (!(fromYtoX && fromXtoY))
            {
                // place for more checks:
                
            }

            return fromYtoX && fromXtoY;
        }

        /// <summary>
        /// Checks if one event is a follower from another event in a list of cases
        /// </summary>
        /// <param name="eventName">The eventname</param>
        /// <param name="possibleFollowerEventname">The eventname of the possible follower</param>
        /// <param name="caseList">A list of cases</param>
        /// <returns>Returns true if the possibleFollowerEventname is a follower the eventName</returns>
        private static bool CheckFollower(string eventName, string possibleFollowerEventname, IEnumerable<Case> caseList)
        {
            foreach (Case Case in caseList)
            {
                bool found = false;

                foreach (Event Event in Case.EventList)
                    if (Event.Name.Equals(eventName))
                        found = true;
                    else if (found && Event.Name.Equals(possibleFollowerEventname))
                        return true; // found "possibleFollowerEventname" as follower from eventName!
            }
            return false;
        }

        /// <summary>
        /// Create a footprint for a petrinet
        /// </summary>
        /// <param name="petriNet">Petrinet</param>
        /// <returns>returns a ComparingFootprint</returns>
        /// <autor>Andrej Albrecht</autor>
        public static ComparingFootprint CreateFootprint(PetriNet petriNet)
        {
            ComparingFootprint resultFootprint = new ComparingFootprint(new CellType[petriNet.Transitions.Count, petriNet.Transitions.Count]);

            foreach (Transition transition in petriNet.Transitions)
            {
                if (!transition.IsLoop)
                    resultFootprint.AddEventHeader(transition.Name);
            }

            List<String> transitionLoops = Transition.GetTransitionLoops(petriNet);

            int indexRow = 0;
            foreach (String headerNameRow in resultFootprint.HeaderWithEventNames)
            {
                int indexColumn = 0;
                foreach (String headerNameColumn in resultFootprint.HeaderWithEventNames)
                {
                    CellType transitionRelationship = GetTransitionRelationship(headerNameRow, headerNameColumn, petriNet);
                    
                    //reset the cellstate to nothing, if the cell have no state:
                    resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Nothing;

                    //save the found transition relationship to the cell and check parallelism with method checkTransitionParallel: 
                    if (headerNameRow.Equals(headerNameColumn) && transitionLoops.Contains(headerNameColumn))
                    {
                        resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Loop;
                    }
                    else if (transitionRelationship.Equals(CellType.Parallel))
                    {
                        resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Parallel;

                        if (indexRow != indexColumn)
                            resultFootprint.ResultMatrix[indexColumn, indexRow] = CellType.Parallel;
                    }
                    else if (transitionRelationship.Equals(CellType.Right))
                    {
                        if (resultFootprint.ResultMatrix[indexRow, indexColumn].Equals(CellType.Left))
                            resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Parallel;

                        resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Right;

                        if (indexRow != indexColumn)
                            resultFootprint.ResultMatrix[indexColumn, indexRow] = CellType.Left;
                    }
                    else if (transitionRelationship.Equals(CellType.Left))
                    {
                        if (resultFootprint.ResultMatrix[indexRow, indexColumn].Equals(CellType.Right))
                            resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Parallel;

                        resultFootprint.ResultMatrix[indexRow, indexColumn] = CellType.Left;
                    }

                    indexColumn++;
                }
                indexRow++;
            }
            return resultFootprint;
        }

        /// <summary>
        /// The logic for finding the relationship between two transitions in a petrinet
        /// </summary>
        /// <param name="transitionOfColumn">Eventname of column</param>
        /// <param name="transitionOfRow">Eventname of row</param>
        /// <param name="petriNet">Petrinet</param>
        /// <autor>Andrej Albrecht</autor>
        private static CellType GetTransitionRelationship(string transitionOfColumn, string transitionOfRow, PetriNet petriNet)
        {
            if (CheckTransitionIsParallel(transitionOfColumn, transitionOfRow, petriNet))
                 return CellType.Parallel;

            //Transition        ->  Place               ->  Transition
            //transition  ->  outgoingPlaceOfTransition ->  outgoingTransitionofPlace
            foreach (Transition transition in petriNet.Transitions)
                if (transition.Name.Equals(transitionOfColumn))
                {
                    foreach (Place outgoingPlaceOfTransition in transition.OutgoingPlaces)
                        foreach (Transition outgoingTransitionofPlace in outgoingPlaceOfTransition.OutgoingTransitions)
                            if (outgoingTransitionofPlace.Name.Equals(transitionOfRow))
                                return CellType.Right;
                }
                else if (transition.Name.Equals(transitionOfRow))
                {
                    foreach (Place outgoingPlaceOfTransition in transition.OutgoingPlaces)
                        foreach (Transition outgoingTransitionofPlace in outgoingPlaceOfTransition.OutgoingTransitions)
                            if (outgoingTransitionofPlace.Name.Equals(transitionOfColumn))
                                return CellType.Left;
                }

            return CellType.Nothing;
        }

        /// <summary>
        /// Checks the parallelism of two transitions in a petrinet recursively
        /// In some petrinets with a high complexity
        /// </summary>
        /// <param name="transition1">Eventname of y-direction</param>
        /// <param name="transition2">Eventname of x-direction</param>
        /// <param name="petriNet">Petrinet</param>
        /// <returns>Return true if the two transitions are parallel</returns>
        /// <autor>Andrej Albrecht</autor>
        // ReSharper disable once UnusedMember.Local
        private static bool CheckTransitionIsParallel(string transition1, string transition2, PetriNet petriNet)
        {
            Transition transitionY = petriNet.FindTransition(transition1);
            Transition transitionX = petriNet.FindTransition(transition2);
            return CheckTransitionIsParallel(transitionY, transitionX, petriNet);
        }

        /// <summary>
        /// Checks the parallelism of two transitions in a given petri net
        /// </summary>
        /// <param name="transition1">A Transition</param>
        /// <param name="transition2">Another Transition</param>
        /// <param name="petriNet">Petrinet</param>
        /// <param name="level"></param>
        /// <returns>Returns true if the two transitions are parallel</returns>
        /// <autor>Andrej Albrecht</autor>
        private static bool CheckTransitionIsParallel(Transition transition1, Transition transition2, PetriNet petriNet, int level = 0)
        {
            //return false if the level is to large and unreal to prevent a stackoverflow exception
            if (level > petriNet.Transitions.Count + petriNet.Places.Count) return false;

            //Same Transition is at the first call never parallel!
            if (level == 0 && transition1.Name.Equals(transition2.Name)) return false;

            //long runtime:
            //The method CheckPathToTransitionAvailable can be improved:
            if ((transition1.IncomingPlaces.Count > 1 || transition2.IncomingPlaces.Count > 1)
                && (CheckPathToTransitionAvailable(transition1, transition2) || CheckPathToTransitionAvailable(transition2, transition1)))
                //The method CheckPathToTransitionAvailable find more branches and perhaps that two transitions are not parallel
                return false;

            foreach (Place incomingPlaceOfTransition1 in transition1.IncomingPlaces)
            {
                foreach (Place incomingPlaceOfTransition2 in transition2.IncomingPlaces)
                {
                    if (incomingPlaceOfTransition1 == incomingPlaceOfTransition2)
                        return false; //Here is the only position to return a false state!

                    foreach (Transition incomingTransitionOfPlace1 in incomingPlaceOfTransition1.IncomingTransitions)
                    {
                        foreach (Transition incomingTransitionOfPlace2 in incomingPlaceOfTransition2.IncomingTransitions)
                            if (incomingTransitionOfPlace1.IsLoop ||
                                incomingTransitionOfPlace1.Name.Equals(incomingTransitionOfPlace2.Name) ||
                                CheckTransitionIsParallel(incomingTransitionOfPlace2, incomingTransitionOfPlace2, petriNet, level + 1) ||
                                CheckTransitionIsParallel(transition1, incomingTransitionOfPlace2, petriNet, level + 1))
                                return true;

                        if (CheckTransitionIsParallel(transition2, incomingTransitionOfPlace1, petriNet, level + 1))
                            return true;

                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a path from a transition to another is available
        /// </summary>
        /// <param name="transition1">A Transition</param>
        /// <param name="transition2">Another Transition</param>
        /// <param name="depth">The current depth (The method set this value when it is calling itself)</param>
        /// <returns>Returns true if a path from a transition to another is available</returns>
        private static bool CheckPathToTransitionAvailable(Transition transition1, Transition transition2, int depth = 0)
        {
            if (depth > 5) return false; //beware StackOverflow
            if (transition1.Equals(transition2)) return true;

            foreach (Place pOutY in transition1.OutgoingPlaces)
                foreach (Transition pOutYtOut in pOutY.OutgoingTransitions)
                    if (transition2.Equals(pOutYtOut) || CheckPathToTransitionAvailable(pOutYtOut, transition2, depth + 1))
                        return true;
            return false;
        }

        /// <summary>
        /// calculates the Fitness for a comparing-footprints-matrix
        /// </summary>
        /// <param name="numDifferences"></param>
        /// <param name="numOpportunities"></param>
        /// <returns>returns the Fitness for a Result matrix between 0.0 and 1.0</returns>
        /// <autor>Andrej Albrecht</autor>
        public static double CalculateFitness(int numDifferences, int numOpportunities)
        {
            if (numDifferences < 0 || numOpportunities < 0) throw new Exception();
            if (numOpportunities == 0) throw new DivideByZeroException();

            double fitness = 1.0 - (numDifferences / (double)numOpportunities);

            //Condition: 0.0 <= Fitness <= 1.0
            if (fitness > 1.0 || fitness < 0.0) throw new Exception("BadFitnessResult: " + fitness + " numDifferences:" + numDifferences + " numOpportunities:" + numOpportunities);

            return fitness;
        }
    }
}