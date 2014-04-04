using System;
using System.Collections.Generic;
using System.Linq;

namespace pgmpm.Model.PetriNet
{
    /// <summary>
    /// A class for places, see also <seealso cref="Transition"/>.
    /// </summary>
    /// <author>Jannik Arndt</author>
    [Serializable]
    public class Place : Node
    {
        #region Properties
        /// <summary>
        /// List of incoming transitions to add an incoming transition to the place.
        /// </summary>
        public List<Transition> IncomingTransitions { get; set; }

        /// <summary>
        /// List of outgoing transitions to add an incoming transition to the place.
        /// </summary>
        public List<Transition> OutgoingTransitions { get; set; }

        public override List<Node> OutgoingNodes { get { return new List<Node>(OutgoingTransitions); } }
        public override List<Node> OutgoingNodesWithoutLoops { get { return new List<Node>(OutgoingTransitionsWithoutLoops); } }
        public override List<Node> OutgoingLoops { get { return new List<Node>(OutgoingTransitionsThatAreLoops); } }
        public override List<Node> IncomingNodes { get { return new List<Node>(IncomingTransitions); } }
        public override List<Node> IncomingNodesWithoutLoops { get { return new List<Node>(IncomingTransitionsWithoutLoops); } }

        private int _token;
        /// <summary>
        /// The (positive) amount of tokens in this place
        /// </summary>
        public int Token
        {
            get { return _token; }
            set
            {
                if (value >= 0)
                    _token = value;
            }
        }

        /// <summary>
        /// Checks all incoming transitions if at least one of them is a Loop
        /// </summary>
        public bool IsBeforeLoopingEvent
        {
            get { return IncomingTransitions.Any(transition => transition.IsLoop); }
        }

        /// <summary>
        /// Checks all outgoing transitions if at least one of them is a Loop
        /// </summary>
        public bool IsAfterLoopingEvent
        {
            get { return OutgoingTransitions.Any(transition => transition.IsLoop); }
        }

        /// <summary>
        /// Gets only transitions that are marked as AND-Split or AND-Join
        /// </summary>
        public List<Transition> OutgoingANDTransitions
        {
            get
            {
                var query = from trans in OutgoingTransitions where (trans.IsANDSplit || trans.IsANDJoin) select trans;
                return query.ToList();
            }
        }

        /// <summary>
        /// Gets only transitions that are NOT marked as AND-Split or AND-Join
        /// </summary>
        public List<Transition> OutgoingTransitionsWithoutANDs
        {
            get
            {
                var query = from trans in OutgoingTransitions where (trans.IsANDSplit == false && trans.IsANDJoin == false) select trans;
                return query.ToList();
            }
        }

        public List<Transition> OutgoingTransitionsWithoutLoops
        {
            get
            {
                var query = from trans in OutgoingTransitions where trans.IsLoop == false select trans;
                return query.ToList();
            }
        }
        public List<Transition> OutgoingTransitionsThatAreLoops
        {
            get
            {
                var query = from trans in OutgoingTransitions where trans.IsLoop select trans;
                return query.ToList();
            }
        }
        public List<Transition> IncomingTransitionsWithoutLoops
        {
            get
            {
                var query = from trans in IncomingTransitions where trans.IsLoop == false select trans;
                return query.ToList();
            }
        }

        #endregion

        /// <summary>
        /// Constructor to create a new place
        /// </summary>
        /// <param Name="name">The Name of the place, which might lateron be displayed in the circle.</param>
        /// <param Name="token">The amount of tokens the new place gets at the beginning.</param>
        public Place(String name = "", int token = 0)
            : base(name)
        {
            Token = token;
            IncomingTransitions = new List<Transition>();
            OutgoingTransitions = new List<Transition>();
        }

        /// <summary>
        /// Returns the Name of the Place if it is NOT empty, otherwise it makes up a name from the incoming and outgoing transitions
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
                return "(" + String.Join(", ", IncomingTransitions) + ") => (" + String.Join(", ", OutgoingTransitions) + ")";
            return Name;
        }

        /// <summary>
        /// Adds an incoming transition to the place. This is usually called when you add a <see cref="Transition"/> to the net.
        /// </summary>
        /// <param Name="transition">The incoming transition.</param>
        public void AppendIncomingTransition(Transition transition)
        {
            if (!IncomingTransitions.Contains(transition))
                IncomingTransitions.Add(transition);
        }

        /// <summary>
        /// Adds an outgoing transition to the place. This is usually called when you add a <see cref="Transition"/> to the net.
        /// </summary>
        /// <param Name="transition">The outgoing transition.</param>
        public void AppendOutgoingTransition(Transition transition)
        {
            if (!OutgoingTransitions.Contains(transition))
                OutgoingTransitions.Add(transition);
        }
    }
}