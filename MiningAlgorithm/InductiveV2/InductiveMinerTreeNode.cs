using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pgmpm.MiningAlgorithm.InductiveV2
{
    /// <summary>
    /// Inductive miner tree
    /// </summary>
    /// <author>Thomas Meents, Bernd Nottbeck</author>
    public class InductiveMinerTreeNode
    {
        /// <summary>
        /// HashSet recursion helper.
        /// </summary>
        private HashSet<InductiveMinerGraphNode> visitedNodes;

        /// <summary>
        /// HashSet recursion helper.
        /// </summary>
        private HashSet<InductiveMinerGraphNode> visitedLoopCheckNodes;

        /// <summary>
        /// Node operation
        /// </summary>
        public OperationsEnum Operation { get; set; }

        /// <summary>
        /// Left leaf node
        /// </summary>
        public InductiveMinerTreeNode LeftLeaf { get; set; }

        /// <summary>
        /// Right leaf node
        /// </summary>
        public InductiveMinerTreeNode RightLeaf { get; set; }

        /// <summary>
        /// Associated event
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Associated GraphNode
        /// </summary>
        public InductiveMinerGraphNode GraphNode;

        /// <summary>
        /// Associated StartEvent
        /// </summary>
        public Event startEvent;

        /// <summary>
        /// new Start rof the right branch
        /// </summary>
        public InductiveMinerGraphNode newStart;

        /// <summary>
        /// Partial petrinet for the node
        /// </summary>
        public PetriNet petriNet;

        /// <summary>
        /// Constructor for a tree node
        /// </summary>
        /// <param name="inductiveMiner"></param>
        /// <param name="operation">node operation</param>
        /// <param name="associatedEvent"></param>
        /// <param name="right"></param>
        /// <param name="left"></param>
        /// <author>Thomas Meents, Bernd Nottbeck</author>
        public InductiveMinerTreeNode(InductiveMiner inductiveMiner, OperationsEnum operation, Event associatedEvent = null, InductiveMinerTreeNode right = null, InductiveMinerTreeNode left = null)
        {
            Operation = operation;
            LeftLeaf = left;
            RightLeaf = right;
            Event = associatedEvent;
            petriNet = inductiveMiner.IMPetriNet;
        }

        /// <summary>
        /// Constructor for a tree node´used duringt the cutting process.
        /// </summary>
        /// <param name="petriNet"></param>
        /// <param name="graphNode"></param>
        /// <param name="startEvent"></param>
        public InductiveMinerTreeNode(PetriNet petriNet, InductiveMinerGraphNode graphNode, Event startEvent)
        {
            GraphNode = graphNode;
            Operation = GraphNode.FollowerList.Count == 0 ? OperationsEnum.isLeaf : OperationsEnum.isUnkown;
            Event = GraphNode.Name;
            LeftLeaf = null;
            RightLeaf = null;
            this.startEvent = startEvent;
            visitedNodes = new HashSet<InductiveMinerGraphNode>();
            visitedLoopCheckNodes = new HashSet<InductiveMinerGraphNode>();
            this.petriNet = petriNet;
            newStart = new InductiveMinerGraphNode(new Event("NewStart"));
            DivideAndConquer();
        }

        /// <summary>
        /// Identifies possible cuts and recursively generates the tree.
        /// </summary>
        public void DivideAndConquer()
        {
            if (Operation == OperationsEnum.isUnkown)
            {
                GraphNode.ReBuildeEventualFollower(null);
                GraphNode.CleanUpHelperList(null);
                foreach (InductiveMinerGraphNode nody in GraphNode.GetMyEventualNodes())
                {
                    nody.ReBuildeEventualFollower(null);
                    nody.CleanUpHelperList(null);
                }
                if (GraphNode.EventualFollowerList.Count <= 1)
                {
                    Operation = OperationsEnum.isLeaf;
                }
            }
            if (Operation != OperationsEnum.isLeaf)
            {
                if (CheckSequenceCut())
                {
                    newStart.ReBuildeEventualFollower(null);
                    newStart.CleanUpHelperList(null);

                    foreach (InductiveMinerGraphNode nody in newStart.GetMyEventualNodes())
                    {
                        nody.ReBuildeEventualFollower(null);
                        nody.CleanUpHelperList(null);
                    }

                    LeftLeaf = new InductiveMinerTreeNode(petriNet, GraphNode, startEvent);
                    RightLeaf = new InductiveMinerTreeNode(petriNet, newStart, newStart.Name);
                    Operation = OperationsEnum.isSequence;

                }
                else if (CheckXorCut(GraphNode))
                {
                    newStart.ReBuildeEventualFollower(null);
                    newStart.CleanUpHelperList(null);

                    foreach (InductiveMinerGraphNode nody in newStart.GetMyEventualNodes())
                    {
                        nody.ReBuildeEventualFollower(null);
                        nody.CleanUpHelperList(null);
                    }

                    LeftLeaf = new InductiveMinerTreeNode(petriNet, GraphNode, startEvent);
                    RightLeaf = new InductiveMinerTreeNode(petriNet, newStart, newStart.Name);
                    Operation = OperationsEnum.isXOR;
                }
                else if (CheckLoopCut(GraphNode))
                {
                    newStart.ReBuildeEventualFollower(null);
                    newStart.CleanUpHelperList(null);
                    foreach (InductiveMinerGraphNode nody in newStart.GetMyEventualNodes())
                    {
                        nody.ReBuildeEventualFollower(null);
                        nody.CleanUpHelperList(null);
                    }

                    LeftLeaf = new InductiveMinerTreeNode(petriNet, GraphNode, startEvent );
                    RightLeaf = new InductiveMinerTreeNode(petriNet, newStart, newStart.Name );
                    Operation = OperationsEnum.isLoop;
                }
                else if (CheckAndCut(GraphNode))
                {
                    newStart.ReBuildeEventualFollower(null);
                    newStart.CleanUpHelperList(null);
                    foreach (InductiveMinerGraphNode nody in newStart.GetMyEventualNodes())
                    {
                        nody.ReBuildeEventualFollower(null);
                        nody.CleanUpHelperList(null);
                    }

                    LeftLeaf = new InductiveMinerTreeNode(petriNet, GraphNode, startEvent);
                    RightLeaf = new InductiveMinerTreeNode(petriNet, newStart, newStart.Name);
                    Operation = OperationsEnum.isParallel;
                }
            }
            else
            {
                if (GraphNode.FollowerList.Count > 0)
                    Event = GraphNode.FollowerList[0].ToNode.Name;
            }
        }

        /// <summary>
        /// Identifies a Sequence cut
        /// </summary>
        /// <returns>True if cut was made</returns>
        private bool CheckSequenceCut()
        {
            HashSet<InductiveMinerRow> sequenceList = SequenceCutHelper(GraphNode);

            bool isSequence = sequenceList.Any();

            if (isSequence)
            {
                int middle = IsEven(sequenceList.Count) ? (sequenceList.Count - 1) / 2 : sequenceList.Count / 2;

                InductiveMinerRow middleRow = sequenceList.ElementAt(middle);

                List<InductiveMinerRow> sequenceNodes = sequenceList.Where(k => k.FromNode.Equals(middleRow.FromNode) ||
                                                                                k.ToNode.Equals(middleRow.ToNode)).ToList();

                var fromNodeQuery = from row in sequenceNodes
                                    where row.FromNode != middleRow.FromNode
                                    select row.FromNode;

                var toNodeQuery = from row in sequenceNodes
                                  where row.ToNode != middleRow.ToNode
                                  select row.ToNode;

                List<InductiveMinerGraphNode> fromNodes = fromNodeQuery.ToList();
                List<InductiveMinerGraphNode> toNodes = toNodeQuery.ToList();

                sequenceNodes.AddRange(sequenceList.Where(k => fromNodes.Contains(k.FromNode) && toNodes.Contains(k.ToNode)).ToList());

                sequenceList = new HashSet<InductiveMinerRow>(sequenceNodes);

                foreach (InductiveMinerRow sequenceRow in sequenceList)
                {
                    newStart.AddDirectFollower(sequenceRow.ToNode);

                    foreach (InductiveMinerGraphNode eventllyNode in sequenceRow.ToNode.GetMyEventualNodes())
                    {
                        newStart.AddEventualFollower(eventllyNode);
                    }

                    sequenceRow.FromNode.WasCut = true;
                    sequenceRow.FromNode.DeleteFollower(sequenceRow);

                    sequenceRow.FromNode.ReBuildeEventualFollower(null);
                    sequenceRow.FromNode.CleanUpHelperList(null);
                }

                newStart.ReBuildeEventualFollower(null);
                newStart.CleanUpHelperList(null);
            }

            return isSequence;
        }

        /// <summary>
        /// Recursive procedure to identify sequence cuts.
        /// </summary>
        /// <param name="graphNode">Analyzed node</param>
        /// <author>Krystian Zielonka, Bernd Nottbeck</author>
        private HashSet<InductiveMinerRow> SequenceCutHelper(InductiveMinerGraphNode graphNode)
        {
            HashSet<InductiveMinerRow> sequenceList = new HashSet<InductiveMinerRow>();

            if (!visitedNodes.Contains(graphNode))
            {
                visitedNodes.Add(graphNode);

                List<InductiveMinerRow> followerList = graphNode.FollowerList;

                foreach (InductiveMinerRow row in followerList)
                {
                    var query = from SearchRow in row.ToNode.EventualFollowerList
                                where SearchRow.ToNode == row.FromNode
                                select SearchRow;

                    InductiveMinerRow currentRow = query.FirstOrDefault();

                    if (currentRow == null && !graphNode.Name.Equals(startEvent) && !graphNode.Name.Equals(newStart))
                    {
                        sequenceList.Add(row);
                    }

                    sequenceList.UnionWith(SequenceCutHelper(row.ToNode));
                }
            }

            return sequenceList;
        }

        /// <summary>
        /// Recursive procedure to identify sequence cuts 
        /// </summary>
        /// <param name="graphNode">Analyzed node</param>
        /// <author>Krystian Zielonka, Bernd Nottbeck</author>
        private bool CheckXorCut(InductiveMinerGraphNode graphNode)
        {
            List<InductiveMinerGraphNode> followerList = graphNode.GetMyDirectNodes();
            List<InductiveMinerRow> deleteList = new List<InductiveMinerRow>();

            bool bo = false;
            bool foundone = false;
            bool goOn = true;
            if (graphNode.FollowerList.Count > 1)
            {
                List<InductiveMinerRow>.Enumerator e = graphNode.FollowerList.GetEnumerator();
                e.MoveNext();

                do
                {
                    if (!e.Current.ToNode.FollowerContains(followerList))
                    {
                        if (foundone)
                        {
                            goOn = false;
                            deleteList.Add(e.Current);
                        }
                        foundone = true;
                    }

                    if (goOn)
                    {
                        goOn = e.MoveNext();
                    }

                } while (goOn);
                e.Dispose();

                foreach (InductiveMinerRow deleteRow in deleteList)
                {
                    newStart.AddDirectFollower(deleteRow.ToNode);

                    foreach (InductiveMinerRow row in deleteRow.ToNode.EventualFollowerList)
                    {
                        if (!newStart.GetMyEventualNodes().Contains(row.ToNode))
                            newStart.EventualFollowerList.Add(new InductiveMinerRow(newStart, row.ToNode));

                    }

                    graphNode.WasCut = true;
                    graphNode.DeleteFollower(deleteRow);


                    bo = true;
                }

                if (bo)
                {
                    newStart.ReBuildeEventualFollower(null);
                    newStart.CleanUpHelperList(null);


                    graphNode.ReBuildeEventualFollower(null);
                    graphNode.CleanUpHelperList(null);
                }
                return bo;
            }
            else return false;
        }

        /// <summary>
        /// Recursive procedure to identify loop cuts
        /// </summary>
        /// <param name="graphNode">Analyzed node</param>
        /// <author>Krystian Zielonka, Bernd Nottbeck</author>
        private bool CheckLoopCut(InductiveMinerGraphNode graphNode)
        {
            bool foundSth = false;

            if (!visitedLoopCheckNodes.Contains(graphNode))
            {
                visitedLoopCheckNodes.Add(graphNode);

                List<InductiveMinerRow> followerList = graphNode.FollowerList;

                foreach (InductiveMinerRow row in followerList)
                {
                    var query = from SearchRow in row.ToNode.EventualFollowerList
                                where SearchRow.ToNode == row.FromNode
                                select SearchRow;

                    InductiveMinerRow currentRow = query.FirstOrDefault();
                    bool and = false;
                    if (currentRow != null)
                    {
                        foreach (InductiveMinerGraphNode andCheckNode in currentRow.FromNode.GetMyDirectNodes())
                        {
                            if (andCheckNode.GetMyDirectNodes().Contains(currentRow.FromNode)) and = true;
                        }


                    }

                    if (currentRow != null && !and)
                    {
                        foundSth = true;

                        bool cutFound = executeLastCutInLoop(row.FromNode, row.ToNode);

                        if (cutFound)
                        {
                            executeFirstCutInLoop(row.FromNode, row.ToNode);

                            return true;
                        }
                    }
                    if (foundSth) { }
                    else
                    {
                        if (CheckLoopCut(row.ToNode)) return true;
                    }
                }
            }

            return foundSth;
        }

        /// <summary>
        /// Executes the first cut for a loop
        /// </summary>
        /// <param name="LoopStart">Loop start</param>
        /// <param name="currentNode">current node</param>
        /// <returns>True if cut was made</returns>
        private bool executeFirstCutInLoop(InductiveMinerGraphNode LoopStart, InductiveMinerGraphNode currentNode)
        {
            Boolean bo = false;
            if (currentNode.WasCut)
            {

                foreach (InductiveMinerRow row in currentNode.FollowerList)
                {
                    if (row.ToNode.GetMyEventualNodes().Contains(LoopStart))
                    {
                        newStart.AddDirectFollower(row.ToNode);
                        foreach (InductiveMinerGraphNode eventuallyRow in row.ToNode.GetMyEventualNodes())
                        {
                            if (!newStart.GetMyEventualNodes().Contains(eventuallyRow))
                            {
                                newStart.EventualFollowerList.Add(new InductiveMinerRow(newStart, eventuallyRow));
                            }
                        }
                        currentNode.FollowerList.Remove(row);
                        return true;
                    }
                }
                bo = true;
            }
            else
            {
                foreach (InductiveMinerRow row in currentNode.EventualFollowerList)
                {
                    if (row.ToNode.GetMyEventualNodes().Contains(LoopStart))
                    {
                        executeFirstCutInLoop(LoopStart, row.ToNode);
                    }
                }
            }
            return bo;
        }

        /// <summary>
        /// Executes the last cut in a loop
        /// </summary>
        /// <param name="LoopStart">Loop start node</param>
        /// <param name="currentNode">Analysed node</param>
        /// <returns>true if cut was made</returns>
        private bool executeLastCutInLoop(InductiveMinerGraphNode LoopStart, InductiveMinerGraphNode currentNode)
        {
            if (currentNode.FollowerList.Count > 0)
            {
                if (currentNode.GetMyDirectNodes().Contains(LoopStart))
                {
                    var query = from search in currentNode.FollowerList
                                where search.ToNode == LoopStart
                                select search;

                    InductiveMinerRow deleteRow = query.FirstOrDefault();
                    currentNode.FollowerList.Remove(deleteRow);

                    return true;
                }
                else
                {
                    foreach (InductiveMinerRow row in currentNode.FollowerList)
                    {
                        if (executeLastCutInLoop(LoopStart, row.ToNode))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Identifies parallels in the graph
        /// </summary>
        /// <param name="graphNode">Analyzed node</param>
        /// <author>Krystian Zielonka, Thomas Meents, Bernd Nottbeck</author>
        private bool CheckAndCut(InductiveMinerGraphNode graphNode)
        {
            bool wasSplit = false;

            if (graphNode.FollowerList.Count > 1)
            {
                List<InductiveMinerRow> followerList = graphNode.FollowerList;
                List<InductiveMinerRow> deleteList = new List<InductiveMinerRow>();

                foreach (InductiveMinerRow row in followerList)
                {
                    InductiveMinerRow currentRow = row.ToNode.FollowerList.FirstOrDefault();

                    if (currentRow.ToNode.GetMyDirectNodes().Contains(currentRow.FromNode))
                    {
                        deleteList.Add(currentRow);
                    }
                }

                wasSplit = deleteList.Any();

                if (wasSplit)
                {
                    foreach (InductiveMinerRow row in deleteList)
                    {
                        row.FromNode.DeleteFollower(row);
                    }

                    InductiveMinerRow lastRow = deleteList.Last();
                    newStart.AddDirectFollower(lastRow.FromNode);
                    newStart.AddEventualFollower(lastRow.FromNode);
                    graphNode.DeleteFollower(graphNode.GetRowWithFollower(lastRow.FromNode));

                    graphNode.ReBuildeEventualFollower(null);
                    graphNode.CleanUpHelperList(null);
                }
            }

            return wasSplit;
        }

        /// <summary>
        /// Test print method
        /// </summary>
        /// <param name="graphNode"></param>
        public void TestTheTree(InductiveMinerGraphNode graphNode)
        {
            if (!visitedNodes.Contains(graphNode))
            {
                visitedNodes.Add(graphNode);
                List<InductiveMinerRow> followerList = graphNode.FollowerList;

                foreach (InductiveMinerRow row in followerList)
                {
                    Console.WriteLine(row.FromNode.Name + " -> " + row.ToNode.Name);
                    TestTheTree(row.ToNode);
                }
            }
        }
    }
}