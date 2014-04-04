using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace pgmpm.Model.PetriNet
{
    /// <summary>
    /// Provides the base-class for Petri Nets, which consist of <see cref="Place"/>s and <see cref="Transition"/>s.
    /// Also implements the <see cref="ProcessModel"/>-Interface
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable]
    public class PetriNet : ProcessModel
    {
        /// <summary>
        /// The constructor sets the Name and initializes the list of places and transitions.
        /// </summary>
        /// <param name="name">The Name of the net.</param>
        public PetriNet(String name = "")
        {
            Name = name;
            Places = new List<Place>();
            Transitions = new List<Transition>();
        }

        public List<Node> Nodes
        {
            get
            {
                List<Node> result = new List<Node>();
                result.AddRange(Places);
                result.AddRange(Transitions);
                return result;
            }
        }

        public List<Place> Places { get; set; }

        public List<Transition> Transitions { get; set; }

        public List<Place> SourcePlaces
        {
            get
            {
                return
                    Places.Where(
                        place =>
                            place.IncomingTransitions.Count == 0 || (place.IsBeforeLoopingEvent && place.IncomingTransitions.Count == 1)).ToList();
            }
        }

        public List<Place> SinkPlaces
        {
            get
            {
                return
                    Places.Where(
                        place =>
                            place.OutgoingTransitions.Count == 0 || AllTransitionsInvisible(place.OutgoingTransitions)).ToList();
            }
        }

        #region Serialization

        /// <summary>
        /// Constructor to deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <author>Jannik Arndt</author>
        public PetriNet(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Places = (List<Place>)info.GetValue("Places", typeof(List<Place>));
            Transitions = (List<Transition>)info.GetValue("Transitions", typeof(List<Transition>));
        }

        /// <summary>
        /// Gets the objects for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <author>Jannik Arndt</author>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Places", Places);
            info.AddValue("Transitions", Transitions);
        }

        #endregion Serialization

        /// <summary>
        /// Implements the interface-method to find out of what kind an object is.
        /// </summary>
        /// <returns>Returns the String PetriNet.</returns>
        public override String IsOfKind()
        {
            return "PetriNet";
        }

        #region Places

        /// <summary>
        /// Creates a new <see cref="Place"/> and adds it to the net.
        /// </summary>
        /// <param name="name">The Name of the new <see cref="Place"/>.</param>
        /// <param name="tokenNumber">The amount of tokens lying on the new <see cref="Place"/>.</param>
        /// <returns>Returns the new place for future reference.</returns>
        public Place AddPlace(String name = "", int tokenNumber = 0)
        {
            Place newPlace = new Place(name, tokenNumber);
            Places.Add(newPlace);
            return newPlace;
        }

        /// <summary>
        /// Looks up a <see cref="Place"/> by its Name and returns the object.
        /// </summary>
        /// <param name="name">Name of the place you are looking for.</param>
        /// <returns>The first object in the list of places with that Name.</returns>
        public Place GetPlaceByName(String name)
        {
            return Places.Find(place => place.Name == name);
        }

        /// <summary>
        /// Returns all places that have no or only invisible transitions following
        /// </summary>
        /// <returns></returns>
        /// <author>Roman Bauer</author>
        public List<Place> GetPlacesWithoutFollowers()
        {
            //iterate over the places and search sink nodes
            return Places.Where(place => place.OutgoingTransitions.Count == 0 || AllTransitionsInvisible(place.OutgoingTransitions)).ToList();
        }

        /// <summary>
        /// Return true if all places are visible or false if at least one place is invisible. This method helps in drawing petrinets.
        /// </summary>
        /// <param name="places"></param>
        /// <autorh>Roman Bauer</autorh>
        /// <returns>Returns true or false</returns>
        private Boolean AllPlacesVisible(IEnumerable<Place> places)
        {
            return places.All(place => place.Visibility);
        }

        /// <summary>
        /// Resets all tokens to zero and puts one token on all source places
        /// </summary>
        public void ResetTokens()
        {
            Places.ForEach(place => place.Token = 0);
            SourcePlaces.ForEach(place => place.Token = 1);
        }

        #endregion Places

        #region Transitions

        private int _transitionsCountCache = -1;

        private int _transitionsWithoutANDCountCache = -1;

        /// <summary>
        /// Creates a new <see cref="Transition"/> and adds it to the net.
        /// </summary>
        /// <param name="setname">The name of the new transition.</param>
        /// <param name="incomingPlaces">The places that proceeds the transition.</param>
        /// <param name="outgoingPlaces">The places that follow the transition.</param>
        /// <param name="incomingPlace">If only one place proceeds the transition.</param>
        /// <param name="outgoingPlace">If only one place follows the transition.</param>
        /// <param name="isLoop">Whether the transition is a loop.</param>
        /// <returns>A new transition that is connected to the net and the places before and after it.</returns>
        /// <author>Jannik Arndt</author>
        public Transition AddTransition(String setname = "", List<Place> incomingPlaces = null, List<Place> outgoingPlaces = null, Place incomingPlace = null, Place outgoingPlace = null, bool isLoop = false)
        {
            incomingPlaces = incomingPlaces ?? new List<Place>();
            outgoingPlaces = outgoingPlaces ?? new List<Place>();
            if (incomingPlace != null)
                incomingPlaces.Add(incomingPlace);
            if (outgoingPlace != null)
                outgoingPlaces.Add(outgoingPlace);

            Transition newTransition = new Transition(setname, incomingPlaces, outgoingPlaces, isLoop);
            Transitions.Add(newTransition);
            return newTransition;
        }

        /// <summary>
        /// Returns the number of transitions that are not AND-Transitions. Note that this method uses a cache for performance.
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public int CountTransitionsWithoutANDs()
        {
            // Only go through all transitions if the overall number of transitions has changed
            // This has a HUGE performance impact!
            if (Transitions.Count() != _transitionsCountCache)
            {
                int counter = Transitions.Count(transition => !transition.Name.StartsWith("AND") && !String.IsNullOrEmpty(transition.Name) && !transition.IsANDJoin && !transition.IsANDSplit);
                _transitionsCountCache = Transitions.Count();
                _transitionsWithoutANDCountCache = counter;
            }
            return _transitionsWithoutANDCountCache;
        }

        /// <summary>
        /// Returns the first element that matches the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public Transition FindTransition(String name)
        {
            return Transitions.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Returns all transitions that have no or only invisible places following
        /// </summary>
        /// <returns></returns>
        /// <author>Roman Bauer</author>
        public List<Transition> GetTransitionsWithoutFollowers()
        {
            //iterate over the transitions and search sink nodes
            return Transitions.Where(transition => transition.OutgoingPlaces.Count == 0 || !AllPlacesVisible(transition.OutgoingPlaces)).ToList();
        }

        /// <summary>
        /// Returns all transitions that have no or only invisible places following, except for loops (so no LOGICAL followers)
        /// </summary>
        /// <returns></returns>
        /// <author>Roman Bauer, Jannik Arndt</author>
        public List<Transition> GetTransitionsWithoutFollowersIgnoreLoops()
        {
            //iterate over the transitions and search sink nodes
            return Transitions.Where(transition => transition.OutgoingPlacesWithoutLoopPlaces.Count == 0 || !AllPlacesVisible(transition.OutgoingPlaces)).ToList();
        }

        /// <summary>
        /// Return if all given transition are visible or not.
        /// </summary>
        /// <param name="transitions"></param>
        /// <autorh>Roman Bauer</autorh>
        /// <returns>Returns true or false</returns>
        private Boolean AllTransitionsInvisible(IEnumerable<Transition> transitions)
        {
            return transitions.All(transition => !transition.Visibility);
        }

        /// <summary>
        /// Fires a transition with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <author>Jannik Arndt</author>
        public void FireTransition(String name)
        {
            Transition transition = FindTransition(name);
            if (transition == null)
                throw new NullReferenceException();
            transition.Fire();
        }

        #endregion Transitions

        #region Nodes

        /// <summary>
        /// Return all sink nodes of the petri net. A sink node is a node without successors or with only invisible successors.
        /// Places and Transitions be regarded as nodes.
        /// </summary>
        /// <returns>Return a list of all nodes that are sink nodes, regardless of whether it is a transition or a place.</returns>
        /// <author>Roman Bauer</author>
        public List<Node> GetSinks()
        {
            List<Node> sinks = new List<Node>();

            sinks.AddRange(GetPlacesWithoutFollowers());
            sinks.AddRange(GetTransitionsWithoutFollowers());

            return sinks;
        }

        /// <summary>
        /// Return all source nodes of the petri net. A source node is a node without predecessors or with only invisible predecessors.
        /// Places and Transitions be regarded as nodes.
        /// </summary>
        /// <returns>Return a list of all nodes that are source nodes, regardless of whether it is a transition or a place.</returns>
        /// <author>Roman Bauer</author>
        public List<Node> GetSources()
        {
            //iterate over the places and search source nodes
            List<Node> sources = new List<Node>(SourcePlaces);

            //iterate over the transitions and search source nodes
            sources.AddRange(Transitions.Where(transition => transition.IncomingPlaces.Count == 0 || !AllPlacesVisible(transition.IncomingPlaces)));

            return sources;
        }

        /// <summary>
        /// Remove a given node. A node can be a place or a transition.
        /// </summary>
        /// <param name="nodeToRemove"></param>
        /// <author>Roman Bauer</author>
        public void RemoveNode(Node nodeToRemove)
        {
            if (nodeToRemove.GetType() == typeof(Place))
                Places.Remove((Place)nodeToRemove);

            if (nodeToRemove.GetType() == typeof(Transition))
                Transitions.Remove((Transition)nodeToRemove);
        }

        /// <summary>
        /// Remove a given node. A node can be a place or a transition.
        /// Version 2: Now also removes the connections in the nodes before and after.
        /// </summary>
        /// <param name="nodeToRemove"></param>
        /// <author>Roman Bauer, Jannik Arndt</author>
        public void RemoveNodeAndConnections(Node nodeToRemove)
        {
            if (nodeToRemove.GetType() == typeof(Place))
            {
                Place placeToRemove = nodeToRemove as Place;
                if (placeToRemove != null)
                {
                    foreach (Transition transition in placeToRemove.IncomingTransitions)
                        transition.OutgoingPlaces.Remove(placeToRemove);
                    foreach (Transition transition in placeToRemove.OutgoingTransitions)
                        transition.IncomingPlaces.Remove(placeToRemove);
                    Places.Remove(placeToRemove);
                }
            }

            if (nodeToRemove.GetType() == typeof(Transition))
            {
                Transition transitionToRemove = nodeToRemove as Transition;
                if (transitionToRemove != null)
                {
                    foreach (Place place in transitionToRemove.IncomingPlaces)
                        place.OutgoingTransitions.Remove(transitionToRemove);
                    foreach (Place place in transitionToRemove.OutgoingPlaces)
                        place.IncomingTransitions.Remove(transitionToRemove);
                    Transitions.Remove(transitionToRemove);
                }
            }
        }

        /// <summary>
        /// Removes all nodes (transitions and places) in the list from the net.
        /// </summary>
        /// <param name="listOfNodes"></param>
        /// <author>Jannik Arndt</author>
        public void RemoveNodes(List<Node> listOfNodes)
        {
            foreach (Node node in listOfNodes)
                RemoveNodeAndConnections(node);
        }

        #endregion Nodes

        #region Loops

        /// <summary>
        /// Finds the Transition with the given name and adds a loop to it. This also handles looping transitions right after multiple places (AND-join) or right before multiple places (AND-split)
        /// </summary>
        /// <param name="transitionName">The name of the transition the is looped</param>
        /// <param name="transition"></param>
        public void AddLoop(String transitionName = "", Transition transition = null)
        {
            Transition item = transition ?? FindTransition(transitionName);
            // if the activity is in the eventlog but NOT in the net
            if (item == null)
                return;

            Transition loop = AddTransition("(Loop)", isLoop: true);

            // before the Transition
            if (item.IncomingPlaces.Count == 1)
            {
                loop.AddOutgoingPlace(item.IncomingPlaces[0]);
            }
            else
            {
                Place newPlace = AddPlace();
                Transition newTransition = AddTransition();

                // redirect incoming connections
                newTransition.AddIncomingPlaces(item.IncomingPlaces);
                foreach (Place place in item.IncomingPlaces)
                    place.OutgoingTransitions.Remove(item);
                item.IncomingPlaces.Clear();
                // add new connections
                loop.AddOutgoingPlace(newPlace);
                newTransition.AddOutgoingPlace(newPlace);
                item.AddIncomingPlace(newPlace);
            }

            // after the Transition
            if (item.OutgoingPlaces.Count == 1)
                loop.AddIncomingPlace(item.OutgoingPlaces[0]);
            else
            {
                Place newPlace = AddPlace();
                Transition newTransition = AddTransition();

                // redirect outgoing connections
                newTransition.AddOutgoingPlaces(item.OutgoingPlaces);
                foreach (Place place in item.OutgoingPlaces)
                    place.IncomingTransitions.Remove(item);
                item.OutgoingPlaces.Clear();
                // add new connections
                item.AddOutgoingPlace(newPlace);
                loop.AddIncomingPlace(newPlace);
                newTransition.AddIncomingPlace(newPlace);
            }
        }

        /// <summary>
        /// Counts all Transitions that are marked as a loop (.IsLoop == true)
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public int CountLoops()
        {
            return Transitions.Count(transition => transition.IsLoop);
        }

        #endregion Loops

        #region Export

        /// <summary>
        /// Converts the net more or less reliable to a string which can be read as a *.dot-File by Graphviz (http://www.graphviz.org/)
        /// </summary>
        /// <returns>Returns said string, which you have to put into a file.</returns>
        public String ConvertToDot()
        {
            StringBuilder content = new StringBuilder();
            content.Append("digraph TC {\n");
            content.Append("node[shape=circle];\nrankdir=LR\n");
            // draw all places with their respective labels and amount of tokens (p0 [label="A (1)"])
            for (int index = 0; index <= Places.Count() - 1; index++)
                content.Append("p" + index + " [label=\"" + Places[index].Name + " (" + Places[index].Token + ")\"]\n");
            content.Append("node[shape=rect];\n");
            // draw all transitions with their respective labels (t0 [label="AtoB"])
            for (int index = 0; index <= Transitions.Count() - 1; index++)
            {
                content.Append("t" + index + " [label=\"" + Transitions[index].Name + "\"]\n");
                // and for each transition draw the incoming (p0 -> t0)...
                foreach (Place incomingPlace in Transitions[index].IncomingPlaces)
                    content.Append("p" + Places.IndexOf(incomingPlace) + " -> " + "t" + index + "\n");
                // ... and outgoing (t0 -> p1) connections
                foreach (Place outgoingPlace in Transitions[index].OutgoingPlaces)
                    content.Append("t" + index + " -> " + "p" + Places.IndexOf(outgoingPlace) + "\n");
            }
            content.Append("}");

            return content.ToString();
        }

        /// <summary>
        /// Converts the net more or less reliable to a string which can be read as a *.pnml by (http://woped.dhbw-karlsruhe.de/woped/)
        /// </summary>
        /// /// <autor>Naby M. Sow</autor>
        /// <returns>Returns said string, which you have to put into a file.</returns>
        public String ConvertToPNML()
        {
            const string cID = "cId-0";

            StringBuilder content = new StringBuilder();
            content.Append("<pnml xmlns=\"http://www.pnml.org/version-2009/grammar/pnml\">\n");
            content.Append("  <net id=\"" + cID + "\" type=\"http://www.pnml.org/version-2009/grammar/ptnet\">\n");
            content.Append("    <page id=\"page0\">\n");

            for (int index = 0; index < Places.Count(); index++)
            {
                string placeId = "p" + index;
                string placeName = Places[index].Name;
                int positionX = Places[index].PositionX;
                int positionY = Places[index].PositionY;

                content.Append("  <place id=\"" + placeId + "\">\n");
                content.Append("    <name>\n");
                content.Append("      <text>" + placeName + "</text>\n");
                content.Append("      <graphics>\n");
                content.Append("         <offset x=\"" + (positionX + 6) + "\" y=\"" + (positionY + 6) + " \"/>\n");
                content.Append("      </graphics>\n");
                content.Append("    </name>\n");
                content.Append("    <graphics>\n");
                content.Append("      <position x=\"" + positionX + "\" y=\"" + positionY + "\"/>\n");
                content.Append("    </graphics>\n");
                content.Append("   </place>\n");
            }

            for (int index = 0; index < Transitions.Count(); index++)
            {
                string transitionId = "t" + index;
                string transitionName = Transitions[index].Name;
                int positionX = Transitions[index].PositionX;
                int positionY = Transitions[index].PositionY;

                content.Append("  <transition id=\"" + transitionId + "\">\n");
                content.Append("    <name>\n");
                content.Append("      <text>" + transitionName + "</text>\n");
                content.Append("      <graphics>\n");
                content.Append("         <offset x=\"" + (positionX + 9) + "\" y=\"" + (positionY + 9) + "\"/>\n");
                content.Append("      </graphics>\n");
                content.Append("    </name>\n");
                content.Append("    <graphics>\n");
                content.Append("      <position x=\"" + positionX + "\" y=\"" + positionY + "\"/>\n");
                content.Append("    </graphics>\n");
                content.Append("  </transition>\n");
            }

            int sourcePlaceId = 0;
            foreach (Place sourcePlace in Places)
            {
                int targetTransitionId = 0;
                foreach (Transition targetTransition in sourcePlace.OutgoingTransitions)
                {
                    string arcId = "p2t" + ((sourcePlaceId + 1) + (sourcePlaceId + 1) * (targetTransitionId + 1)); //calculation for unique id
                    string arcSource = GetPlaceID(sourcePlace);
                    string arcTarget = GetTransitionID(targetTransition);
                    content.Append("  <arc id=\"" + arcId + "\" " + "source=\"" + arcSource + "\" " + "target=\"" + arcTarget + "\">\n");
                    content.Append("    <graphics/>\n");
                    content.Append("  </arc>\n");

                    targetTransitionId++;
                }
                sourcePlaceId++;
            }

            int sourceTransitionId = 0;
            foreach (Transition sourceTransition in Transitions)
            {
                int targetPlaceId = 0;
                foreach (Place targetPlace in sourceTransition.OutgoingPlaces)
                {
                    string arcId = "t2p" + ((sourceTransitionId + 1) + (sourceTransitionId + 1) * (targetPlaceId + 1)); //calculation for unique id
                    string arcSource = GetTransitionID(sourceTransition);
                    string arcTarget = GetPlaceID(targetPlace);
                    content.Append("  <arc id=\"" + arcId + "\" " + "source=\"" + arcSource + "\" " + "target=\"" + arcTarget + "\">\n");
                    content.Append("    <graphics/>\n");
                    content.Append("  </arc>\n");

                    targetPlaceId++;
                }

                sourceTransitionId++;
            }

            content.Append("    </page>\n");
            content.Append("    <name>\n");
            content.Append("      <text></text>\n");
            content.Append("    </name>\n");
            content.Append("  </net>\n");
            content.Append("</pnml>\n");

            return content.ToString();
        }

        /// <summary>
        /// Returns a generated PlaceID
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        public String GetPlaceID(Place place)
        {
            return "p" + Places.IndexOf(place);
        }

        /// <summary>
        /// Returns a generated TransitionID
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        public String GetTransitionID(Transition transition)
        {
            return "t" + Transitions.IndexOf(transition);
        }

        #endregion Export

        #region Misc

        /// <summary>
        /// Adds another PetriNet to this at the given Transition (since the other PetriNet should start with a place).
        /// Keep in mind to close all open sinks afterwards!
        /// </summary>
        /// <param name="petriNet">The other PetriNet</param>
        /// <param name="atTransition">A transition in THIS PetriNet, where the added net will be connected</param>
        /// <author>Jannik Arndt</author>
        public void MergeWithPetriNet(PetriNet petriNet, Transition atTransition)
        {
            List<Node> sources = petriNet.GetSources();
            if (sources.Count > 0)
            {
                atTransition.OutgoingPlaces.Add((sources[0] as Place));
                atTransition.OutgoingPlaces[0].AppendIncomingTransition(atTransition);
                Transitions.AddRange(petriNet.Transitions);
                Places.AddRange(petriNet.Places);
            }
        }

        #endregion Misc
    }
}