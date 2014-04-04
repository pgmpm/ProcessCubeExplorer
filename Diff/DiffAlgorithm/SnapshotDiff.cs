using System.Collections.Generic;
using System.Linq;
using pgmpm.Model;
using pgmpm.Model.PetriNet;


namespace pgmpm.Diff.DiffAlgorithm
{
    ﻿///<author>Christopher Licht, Thomas Meents, Jannik Arndt</author>
    public class SnapshotDiff : IDifference
    {
        public static readonly List<PetriNet> ListOfChoosenProcessModels = new List<PetriNet>();

        public SnapshotDiff()
        {}

        /// <summary>
        /// Finds the difference in two process models and combines them into one.
        /// </summary>
        /// <param name="listOfChoosenProcessModels">A list with exactly two petrinets in it.</param>
        /// <returns>A new petrinet that combines the two given ones</returns>
        /// <author>Christopher Licht, Jannik Arndt</author>
        public ProcessModel CompareProcessModels(List<ProcessModel> listOfChoosenProcessModels)
        {
            if (listOfChoosenProcessModels.Count != 2)
                return null;

            // Reset & fill list of PetriNets, also cast ProcessModels to PetriNets
            ListOfChoosenProcessModels.Clear();
            foreach (ProcessModel processModel in listOfChoosenProcessModels)
                ListOfChoosenProcessModels.Add((PetriNet)processModel);

            // 1. Mark all Transitions as "added", "deleted", "changed" or "unchanged"          
            FindDifferences(ListOfChoosenProcessModels[0].Transitions, ListOfChoosenProcessModels[1].Transitions);

            // Copy the first PetriNet
            PetriNet resultingNet = (PetriNet)ListOfChoosenProcessModels[0].DeepCopy();

            // 2. Add the transitions with status "added"
            AddAddedTransitions(resultingNet);

            // Force all nodes to be redrawn
            resultingNet.Nodes.ForEach(node => node.IsDrawn = false);

            FixEnding(resultingNet);

            return resultingNet;
        }

        /// <summary>
        /// Calculates the difference between two lists of transitions and marks the transitions as deleted, added, changed or unchanged.
        /// </summary>
        /// <param name="listofTransitionsInFirstProcessModel">Contains the transitions of the first net.</param>
        /// <param name="listofTransitionsInSecondProcessModel">Contains the transitions of the second net.</param>
        /// <author>Jannik Arndt (Originally by Christopher Licht & Thomas Meents)</author>
        public void FindDifferences(List<Transition> listofTransitionsInFirstProcessModel, List<Transition> listofTransitionsInSecondProcessModel)
        {
            // 1. Find nodes that are in one list but not the other
            var deleted = listofTransitionsInFirstProcessModel.Except(listofTransitionsInSecondProcessModel, new TransitionComparer()).ToList();
            var added = listofTransitionsInSecondProcessModel.Except(listofTransitionsInFirstProcessModel, new TransitionComparer()).ToList();

            // 2. Mark them
            deleted.ForEach(transition => transition.DiffStatus = DiffState.Deleted);
            added.ForEach(transition => transition.DiffStatus = DiffState.Added);

            // 3. Make a list for the remaining Transitions
            var restOfFirstModel = listofTransitionsInFirstProcessModel.Except(deleted, new TransitionComparer()).ToList();
            var restOfSecondModel = listofTransitionsInSecondProcessModel.Except(added, new TransitionComparer()).ToList();

            // 4. Go through the remaining Transitions and compare their names. Update the name if it differs (different order)
            for (var index = 0; index < restOfFirstModel.Count(); index++)
                if (!restOfFirstModel[index].Name.Equals(restOfSecondModel[index].Name))
                {
                    restOfFirstModel[index].DiffStatus = DiffState.Changed;
                    restOfFirstModel[index].Name = "(" + restOfFirstModel[index].Name + ") " + restOfSecondModel[index].Name;
                }
                else
                    restOfFirstModel[index].DiffStatus = DiffState.Unchanged;
        }

        /// <summary>
        /// Adds all added Transition to the NetWithDifference
        /// </summary>
        /// <author>Jannik Arndt,Thomas Meents</author>
        public void AddAddedTransitions(PetriNet netWithDifference)
        {
            // Find all Transitions that have the DiffStatus "added"
            List<Transition> addedTransitions = ListOfChoosenProcessModels[1].Transitions.Where(transition => transition.DiffStatus == DiffState.Added).ToList();

            foreach (Transition addedTransition in addedTransitions)
            {
                netWithDifference.Places.AddRange(addedTransition.OutgoingPlaces);
                addedTransition.DiffStatus = DiffState.Added;

                // For non-starting transitions
                if (addedTransition.IncomingPlaces[0].IncomingTransitions != null && addedTransition.IncomingPlaces[0].IncomingTransitions.Count > 0)
                {
                    List<Transition> listOfFollower = new List<Transition>();
                    if (addedTransition.OutgoingPlaces[0].OutgoingTransitions.Count > 0)
                    {
                        foreach (Place t in addedTransition.OutgoingPlaces)
                            foreach (Transition newfollower in t.OutgoingTransitions)
                                if (!listOfFollower.Contains(newfollower))
                                    listOfFollower.Add(newfollower);
                    }
                    else
                    {
                        Transition follower = new Transition();
                        listOfFollower.Add(follower);
                    }

                    if (listOfFollower.Count > 0)
                    {
                        listOfFollower[0].IncomingPlaces.Add(addedTransition.OutgoingPlaces[0]);
                    }
                    else
                    {
                        Place endPlace = new Place("End");
                        addedTransition.OutgoingPlaces.Add(endPlace);
                    }
                }

                // For starting Transitions
                else
                {
                    foreach (Place t in addedTransition.OutgoingPlaces)
                    {
                        if (t.OutgoingTransitions.Count <= 0) continue;
                        addedTransition.IncomingPlaces[0].Name = "Start";
                        netWithDifference.Places.Add(addedTransition.IncomingPlaces[0]);

                        if (addedTransition.IncomingPlaces[0].OutgoingTransitions.Count > 1)
                            addedTransition.IncomingPlaces[0].OutgoingTransitions.RemoveAt(1);

                        if (addedTransition.OutgoingPlaces[0].IncomingTransitions.Count > 1)
                            addedTransition.OutgoingPlaces[0].IncomingTransitions.RemoveAt(1);

                        netWithDifference.Places.RemoveAt(0);
                    }
                }
                netWithDifference.Transitions.Add(addedTransition);
            }
        }

        /// <summary>
        /// Remove old "End"-Places, combine sinks and add new "End"-Place
        /// </summary>
        /// <param name="petriNet">The petrinet to work on</param>
        /// <author>Jannik Arndt</author>
        public void FixEnding(PetriNet petriNet)
        {
            foreach (Place endPlace in petriNet.Places.Where(place => place.Name.Equals("End")))
                endPlace.Name = "";

            List<Place> endPlaces = (from sink in petriNet.GetSinks() where sink.GetType() == typeof(Place) select sink as Place).ToList();

            if (endPlaces.Count > 1)
                foreach (Place endPlace in endPlaces.Skip(1))
                {
                    foreach (Transition incomingTransition in endPlace.IncomingTransitions)
                    {
                        endPlaces[0].IncomingTransitions.Add(incomingTransition);
                        incomingTransition.OutgoingPlaces.Clear();
                        incomingTransition.OutgoingPlaces.Add(endPlaces[0]);
                    }
                    petriNet.Places.Remove(endPlace);
                }
            endPlaces[0].Name = "End";
        }
    }
}