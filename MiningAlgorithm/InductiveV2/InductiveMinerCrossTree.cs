using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MiningAlgorithm.InductiveV2
{
    public class InductiveMinerCrossTree
    {
        public InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(operation:OperationsEnum.isSequence);

        public void CreateExampleTree()
        {
            //linker Teil
            InductiveMinerTreeNode one = new InductiveMinerTreeNode(operation:OperationsEnum.isChoice);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(new Event("D")) {Operation = OperationsEnum.isLeaf};
            one.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(operation:OperationsEnum.isLoop);
            one.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(new Event("C")) {Operation = OperationsEnum.isLeaf};
            one.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(operation:OperationsEnum.isSequence);
            three.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(new Event("A")) {Operation = OperationsEnum.isLeaf};
            five.LeftLeaf = six;
            InductiveMinerTreeNode seven = new InductiveMinerTreeNode(new Event("B")) { Operation = OperationsEnum.isLeaf };
            five.RightLeaf = seven;

            //rechter Teil
            InductiveMinerTreeNode eight = new InductiveMinerTreeNode(operation:OperationsEnum.isParallel);
            ExampleTree.RightLeaf = eight;
            InductiveMinerTreeNode nine = new InductiveMinerTreeNode(new Event("E")) {Operation = OperationsEnum.isLeaf};
            eight.LeftLeaf = nine;
            InductiveMinerTreeNode ten = new InductiveMinerTreeNode(new Event("F")) {Operation = OperationsEnum.isLeaf};
            eight.RightLeaf = ten;
        }
    }
}