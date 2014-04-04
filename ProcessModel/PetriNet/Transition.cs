using System;
using System.Collections.Generic;
using System.Linq;

namespace pgmpm.Model.PetriNet
{

    /// <summary>
    /// A class for transitions, see also <seealso cref="Place"/>.
    /// </summary>
    /// <author>Jannik Arndt, Andrej Albrecht</author>
    [Serializable]
    public class Transition : Node
    {
        /// <summary>
        /// List with incoming places.
        /// </summary>
        public List<Place> IncomingPlaces { get; set; }

        /// <summary>
        /// List with outgoing places.
        /// </summary>
        public List<Place> OutgoingPlaces { get; set; }

        public override List<Node> OutgoingNodes { get { return new List<Node>(OutgoingPlaces); } }
        public override List<Node> OutgoingNodesWithoutLoops { get { return new List<Node>(OutgoingPlaces); } }
        /// <summary>
        /// Returns an empty list, but needs to be implemented to call Node.OutgoingLoops
        /// </summary>
        public override List<Node> OutgoingLoops { get { return new List<Node>(); } }
        public override List<Node> IncomingNodes { get { return new List<Node>(IncomingPlaces); } }
        public override List<Node> IncomingNodesWithoutLoops { get { return new List<Node>(IncomingPlaces); } }

        public bool IsLoop = false;
        public bool IsANDSplit = false;
        public bool IsANDJoin = false;

        /// <summary>
        /// Checks if this transition is enabled, which means that all incoming places have at least one token
        /// </summary>
        public bool IsEnabled
        {
            get { return IncomingPlaces.All(place => place.Token >= 1); }
        }

        /// <summary>
        /// Get all outgoing Places without those that have ONE predecessor and ONE successor that is a Loop
        /// </summary>
        public List<Place> OutgoingPlacesWithoutLoopPlaces
        {
            get { return OutgoingPlaces.Where(place => !(place.IsAfterLoopingEvent && place.OutgoingTransitions.Count == 1 && place.IncomingTransitions.Count == 1)).ToList(); }
        }

        /// <summary>
        /// Creates a new transition that is connected in the given petriNet and to the incoming and outgoing places.
        /// </summary>
        /// <param name="name">The Name of the new transition.</param>
        /// <param name="placesIn">The Places that precede the transition.</param>
        /// <param name="placesOut">The Places that follow the transition.</param>
        /// <param name="isLoop">Whether the transition is a loop or not</param>
        public Transition(String name = "", List<Place> placesIn = null, List<Place> placesOut = null, bool isLoop = false)
            : base(name)
        {
            IncomingPlaces = placesIn ?? new List<Place>();
            OutgoingPlaces = placesOut ?? new List<Place>();
            IsLoop = isLoop;

            foreach (Place incomingPlace in IncomingPlaces)
                incomingPlace.AppendOutgoingTransition(this);

            foreach (Place outgoingPlace in OutgoingPlaces)
                outgoingPlace.AppendIncomingTransition(this);
        }

        /// <summary>
        /// Fires the transition, if it is enabled.
        /// </summary>
        /// <author>Jannik Arndt</author>
        public void Fire()
        {
            var tryAgain = false;

            if (IsEnabled)
            {
                foreach (Place place in IncomingPlaces)
                    place.Token = place.Token - 1; //cannot become less than 0

                foreach (Place place in OutgoingPlaces)
                    place.Token = place.Token + 1;
            }
            // if the previous transition is an AND-Split, fire that one, then fire this again
            else if (IncomingPlaces.Count > 0)
            {
                foreach (var incomingPlace in IncomingPlaces)
                    foreach (var transition1 in incomingPlace.IncomingTransitions.Where(transition => transition.IsANDSplit || transition.IsANDJoin || transition.IsLoop))
                    {
                        transition1.Fire();
                        tryAgain = true;
                    }
                if (tryAgain)
                    Fire();
                else
                    throw new TransitionNotEnabledException("The transition is not enabled");
            }
        }

        /// <summary>
        /// Add a connection between this transition and the given following place.
        /// </summary>
        public void AddOutgoingPlace(Place outgoing)
        {
            if (!OutgoingPlaces.Contains(outgoing))
                OutgoingPlaces.Add(outgoing);
            outgoing.AppendIncomingTransition(this);
        }

        /// <summary>
        /// Add a connection between this transition and the given following places.
        /// </summary>
        public void AddOutgoingPlaces(List<Place> outgoing)
        {
            //foreach (Place Place in outgoing)
            //    AddOutgoingPlace(Place);
            OutgoingPlaces.AddRange(outgoing);
            foreach (Place place in outgoing)
                place.AppendIncomingTransition(this);
        }

        /// <summary>
        /// Add a connection between this transition and the given preceding place.
        /// </summary>
        public void AddIncomingPlace(Place incoming)
        {
            //if (!IncomingPlaces.Contains(incoming))
            //    IncomingPlaces.Add(incoming);
            //incoming.AppendOutgoingTransition(this);
            IncomingPlaces.Add(incoming);
            incoming.AppendOutgoingTransition(this);
        }

        /// <summary>
        /// Add a connection between this transition and the given preceding place.
        /// </summary>
        public void AddIncomingPlaces(List<Place> incoming)
        {
            //foreach (Place Place in incoming)
            //    AddIncomingPlace(Place);
            IncomingPlaces.AddRange(incoming);
            foreach (Place place in incoming)
                place.AppendOutgoingTransition(this);
        }

        /// <summary>
        /// creates a list of looped events
        /// </summary>
        /// <param name="petriNet">Petrinet</param>
        /// <returns>Returns a list of Strings</returns>
        /// <autor>Andrej Albrecht</autor>
        public static List<string> GetTransitionLoops(PetriNet petriNet)
        {
            List<String> transitionLoops = new List<String>();
            foreach (Transition transitionLevel1 in petriNet.Transitions)
                if (transitionLevel1.IsLoop)
                {
                    transitionLoops.Add(transitionLevel1.Name);
                    foreach (Place incomingPlaceOfTransitionLevel1 in transitionLevel1.IncomingPlaces)
                        foreach (Transition incomingTransitionOfIncomingPlace in incomingPlaceOfTransitionLevel1.IncomingTransitions)
                            foreach (Place outgoingPlacOfTransitionLevel1 in transitionLevel1.OutgoingPlaces)
                                foreach (Transition tOutPOut in outgoingPlacOfTransitionLevel1.OutgoingTransitions)
                                    if (incomingTransitionOfIncomingPlace.Name.Equals(tOutPOut.Name))
                                        transitionLoops.Add(incomingTransitionOfIncomingPlace.Name);
                }
            return transitionLoops;
        }
    }
}