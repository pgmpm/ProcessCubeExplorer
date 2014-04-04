using pgmpm.MatrixSelection.Fields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pgmpm.MiningAlgorithm.InductiveV2
{
    /// <summary>
    /// An inductive miner graph node is used to analyse cutting options.
    /// </summary>
    ///  <author>Krystian Zielonka, Bernd Nottbeck</author>
    public class InductiveMinerGraphNode
    {
        public bool WasCut;


        /// <summary>
        /// Infrequency threshold
        /// </summary>

        private readonly double threshHold;

        /// <summary>
        /// A helper list to save which nodes are still reachable
        /// </summary>
        public List<InductiveMinerGraphNode> StillReachableHelperList;

        /// <summary>
        /// A list containing all directly follower
        /// </summary>
        public List<InductiveMinerRow> FollowerList;

        /// <summary>
        /// A list containing all eventually follower
        /// </summary>
        public List<InductiveMinerRow> EventualFollowerList;

        /// <summary>
        /// The event associated with the node
        /// </summary>
        public Event Name { get; set; }

        /// <summary>
        /// Constructor for an graph node object
        /// </summary>
        /// <param name="name">associated event</param>
        ///  <author>Bernd Nottbeck</author>
        public InductiveMinerGraphNode(Event name)
        {
            threshHold = MinerSettings.GetAsDouble("InductiveThresholdSlider");

            Name = name;
            FollowerList = new List<InductiveMinerRow>();
            EventualFollowerList = new List<InductiveMinerRow>();
        }

        /// <summary>
        /// Checks if the eventually follower list contains an node from the provided list
        /// </summary>
        /// <param name="list">Nodes to check</param>
        /// <returns>true if eventually an item of list is in eventually follower</returns>
        /// <author>Krystian Zielonka, Bernd Nottbeck</author>
        public Boolean FollowerContains(List<InductiveMinerGraphNode> list)
        {
            list.Remove(this);

            return list.Intersect(GetMyEventualNodes()).Any();
        }

        /// <summary>
        /// Gets a list of directly follower nodes
        /// </summary>
        /// <returns>node list</returns>
        /// <author>Bernd Nottbeck</author>
        public List<InductiveMinerGraphNode> GetMyDirectNodes()
        {
            var query = from row in FollowerList
                        select row.ToNode;

            List<InductiveMinerGraphNode> tempList = query.ToList();
            return tempList;
        }

        /// <summary>
        /// Gets a list of eventually follower nodes
        /// </summary>
        /// <returns>node list</returns>
        /// <author>Bernd Nottbeck</author>
        public List<InductiveMinerGraphNode> GetMyEventualNodes()
        {
            var query = from row in EventualFollowerList
                        select row.ToNode;

            List<InductiveMinerGraphNode> tempList = query.ToList();
            return tempList;
        }

        public void AddDirectFollower(InductiveMinerGraphNode node)
        {
            if (!GetMyDirectNodes().Contains(node))
            {
                FollowerList.Add(new InductiveMinerRow(this, node));
            }
        }

        public void AddEventualFollower(InductiveMinerGraphNode node)
        {
            if (!GetMyEventualNodes().Contains(node))
            {
                EventualFollowerList.Add(new InductiveMinerRow(this, node));
            }
        }

        public InductiveMinerRow GetRowWithFollower(InductiveMinerGraphNode node)
        {
            var query = from row in FollowerList
                        where row.ToNode == node
                        select row;

            return query.FirstOrDefault();
        }

        public void DeleteFollower(InductiveMinerRow rowToDelete)
        {
            FollowerList.Remove(rowToDelete);
        }

        /// <summary>
        /// Orders  the directly and eventually follower list by occurrence
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        public void OrderFollowerLists()
        {
            FollowerList = FollowerList.OrderBy(o => o.Count).ToList();
            EventualFollowerList = EventualFollowerList.OrderBy(o => o.Count).ToList();
        }

        /// <summary>
        /// Eliminates infrequent directly follower
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        public void EliminateInfrequent()
        {
            if (FollowerList.Count > 1)
            {
                InductiveMinerRow lastRow = FollowerList.Last<InductiveMinerRow>();
                int ThreshHoldValue = (int)Math.Round(lastRow.Count * threshHold);
                bool SomethingWasRemoved = true;
                List<InductiveMinerRow>.Enumerator e = FollowerList.GetEnumerator();
                List<InductiveMinerRow> deleteList = new List<InductiveMinerRow>();
                e.MoveNext();
                do
                {
                    if (e.Current.Count < ThreshHoldValue)
                    {
                        deleteList.Add(e.Current);
                    }
                    else
                    {
                        SomethingWasRemoved = false;
                    }

                    if (!e.MoveNext())
                    {
                        SomethingWasRemoved = false;
                    }

                } while (SomethingWasRemoved);
                e.Dispose();
                foreach (InductiveMinerRow deleteRow in deleteList)
                {
                    FollowerList.Remove(deleteRow);
                }
            }
        }

        /// <summary>
        /// Recursively rebuilds the eventually follower list based on the current node.
        /// </summary>
        /// <param name="ComingFromList"></param>
        /// <returns>A list of nodes that are still reachable</returns>
        /// <author>Bernd Nottbeck</author>
        public List<InductiveMinerGraphNode> ReBuildeEventualFollower(List<InductiveMinerGraphNode> ComingFromList)
        {
            if (ComingFromList == null)
            {
                ComingFromList = new List<InductiveMinerGraphNode>();
                StillReachableHelperList = new List<InductiveMinerGraphNode>();
            }

            List<InductiveMinerGraphNode> reachableNodeList = new List<InductiveMinerGraphNode>();

            reachableNodeList.Add(this);
            ComingFromList.Add(this);

            if (FollowerList.Count != 0)
            {
                foreach (InductiveMinerRow row in FollowerList)
                {
                    // abfrage ob überprüft werden soll also EventuallyFollowerList contains
                    if (!ComingFromList.Contains(row.ToNode))
                    {
                        //Diese Abfrage muss mindestens den Follower Knoten selbs zurück geben
                        List<InductiveMinerGraphNode> temp = row.ToNode.ReBuildeEventualFollower(ComingFromList);
                        List<InductiveMinerGraphNode> eventually = GetMyEventualNodes();

                        var query = from row1 in temp
                                    where eventually.Contains(row1) && !reachableNodeList.Contains(row1)
                                    select row1;

                        List<InductiveMinerGraphNode> querReturn = query.ToList();

                        reachableNodeList.AddRange(querReturn);
                    }
                    else
                    {
                        reachableNodeList.Add(row.ToNode);
                    }
                    reachableNodeList = reachableNodeList.Distinct().ToList();
                }
            }
            StillReachableHelperList = reachableNodeList;
            return reachableNodeList;
        }

        /// <summary>
        /// Removes the Eventually follower rows that are no longer reachable.
        /// </summary>
        /// <author>Bernd Nottbeck</author>
        public void CleanUpHelperList(List<InductiveMinerGraphNode> ComingFromList)
        {
            var query = from row in EventualFollowerList
                        where StillReachableHelperList.Contains(row.ToNode)
                        select row;

            List<InductiveMinerRow> queryList = query.ToList();

            if (queryList.Count != 0)
            {
                EventualFollowerList = query.ToList();
            }
            else
            {
                EventualFollowerList.Clear();
            }
        }
    }
}