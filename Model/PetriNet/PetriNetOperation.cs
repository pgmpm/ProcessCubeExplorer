using System.Collections.Generic;
using System.Linq;

namespace pgmpm.Model.PetriNet
{
    /// <summary>
    /// This class offers functions that can enhance a process model
    /// </summary>
    /// <author>Jannik Arndt</author>
    public static class PetriNetOperation
    {
        /// <summary>
        /// Deletes all empty transitions from a Petri Net and redirects the connections
        /// </summary>
        /// <param name="petriNet"></param>
        /// <author>Jannik Arndt</author>
        public static PetriNet CleanUpPetriNet(PetriNet petriNet)
        {
            List<Node> nodesToBeDeleted = new List<Node>();
            foreach (Place place in petriNet.Places.Reverse<Place>())
            {
                if (place.IncomingTransitions.Count == 1
                    && place.IncomingTransitions[0].OutgoingPlaces.Count == 1
                    && place.OutgoingTransitions.Count == 1
                    && place.OutgoingTransitions[0].IncomingPlaces.Count == 1)
                {
                    if (place.IncomingTransitions[0].Name == "")
                    {
                        // Redirect connections around the empty transition
                        place.OutgoingTransitions[0].AddIncomingPlaces(place.IncomingTransitions[0].IncomingPlaces);
                        nodesToBeDeleted.Add(place);
                        nodesToBeDeleted.Add(place.IncomingTransitions[0]);
                    }
                    if (place.OutgoingTransitions[0].Name == "")
                    {
                        // Redirect connections around the empty transition
                        place.IncomingTransitions[0].AddOutgoingPlaces(place.OutgoingTransitions[0].OutgoingPlaces);
                        nodesToBeDeleted.Add(place);
                        nodesToBeDeleted.Add(place.OutgoingTransitions[0]);
                    }
                }
            }
            petriNet.RemoveNodes(nodesToBeDeleted);
            return petriNet;
        }
    }
}
