using System.Collections.Generic;

namespace pgmpm.MiningAlgorithm
{
    /// <summary>
    /// This class contains the followers and predecessors transitions of a place
    /// </summary>
    /// <author>Christopher Licht</author>
    public class AlphaMinerPlaces
    {
        private HashSet<string> _ListOfFollower;
        private HashSet<string> _ListOfPredecessor;

        /// <summary>
        /// Constructor to set the lists of predecessor and followers
        /// </summary>
        /// <param name="predecessors"></param>
        /// <param name="followers"></param>
        /// <author>Christopher Licht</author>
        public AlphaMinerPlaces(HashSet<string> predecessors, HashSet<string> followers)
        {
            _ListOfPredecessor = predecessors;
            _ListOfFollower = followers;;
        }

        /// <summary>
        /// Default-Constructor
        /// </summary>
        /// <author>Christopher Licht</author>
        public AlphaMinerPlaces() { }

        /// <summary>
        /// To get and set the list of follower places.
        /// </summary>
        /// <author>Christopher Licht</author>
        public HashSet<string> FollowerHashSet
        {
            get { return _ListOfFollower; }
            set { _ListOfFollower = value; }
        }

        /// <summary>
        /// To get and set the list of predecessor places.
        /// </summary>
        /// <author>Christopher Licht</author>
        public HashSet<string> PredecessorHashSet
        {
            get { return _ListOfPredecessor; }
            set { _ListOfPredecessor = value; }
        }

        /// <summary>
        /// Merges the current predecessor-set with another set
        /// </summary>
        /// <param name="PredecessorSet"></param>
        /// <author>Christopher Licht</author>
        public void MergePredecessorSet(HashSet<string> PredecessorSet)
        {
            _ListOfPredecessor.UnionWith(PredecessorSet);
        }

        /// <summary>
        /// Merges the current Follower-set with another set
        /// </summary>
        /// <param name="FollowerSet"></param>
        /// <author>Christopher Licht</author>
        public void MergeFollowerSet(HashSet<string> FollowerSet)
        {
            _ListOfFollower.UnionWith(FollowerSet);
        }

        /// <summary>
        /// Clears the lists of predecessors and followers.
        /// </summary>
        /// <author>Christopher Licht</author>
        public void ClearAlphaPlace()
        {
            _ListOfPredecessor.Clear();
            _ListOfFollower.Clear();
        }
    }
}
