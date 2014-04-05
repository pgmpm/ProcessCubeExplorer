using System.Collections.Generic;

namespace pgmpm.Model.PetriNet
{
    /// <summary>
    /// Custom Comparer to compare two Transition-Objects by Name
    /// </summary>
    /// <author>Jannik Arndt</author>
    public class TransitionComparer : IEqualityComparer<Transition>
    {
        public int GetHashCode(Transition transition)
        {
            if (transition == null)
                return 0;
            return transition.Name.GetHashCode();
        }

        public bool Equals(Transition x1, Transition x2)
        {
            if (ReferenceEquals(x1, x2))
                return true;
            if (ReferenceEquals(x1, null) || ReferenceEquals(x2, null))
                return false;

            return x1.Name.Equals(x2.Name);
        }
    }
}
