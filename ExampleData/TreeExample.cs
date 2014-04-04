using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm.InductiveV2;

namespace pgmpm.ExampleData
{

    /// <summary>
    /// Example Tree for implementation tests.
    /// </summary>
    public class TreeExample
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static InductiveMinerTreeNode CreateExampleTree(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner,operation: OperationsEnum.isSequence);

            //linker Teil
            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            one.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLoop);
            one.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            three.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            three.LeftLeaf = five;

            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            five.LeftLeaf = six;
            InductiveMinerTreeNode seven = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            five.RightLeaf = seven;

            //rechter Teil
            InductiveMinerTreeNode eight = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isParallel);
            ExampleTree.RightLeaf = eight;
            InductiveMinerTreeNode nine = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("E") };
            eight.LeftLeaf = nine;
            InductiveMinerTreeNode ten = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("F") };
            eight.RightLeaf = ten;

            return ExampleTree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static InductiveMinerTreeNode CreateExampleTreeXor(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            one.LeftLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            one.RightLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            ExampleTree.RightLeaf = four;

            return ExampleTree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static InductiveMinerTreeNode CreateExampleTreeParallel(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            one.LeftLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            one.RightLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isParallel);
            ExampleTree.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            four.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            four.RightLeaf = six;

            return ExampleTree;
        }

        public static InductiveMinerTreeNode CreateExampleSequence(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            one.LeftLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            one.RightLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            ExampleTree.RightLeaf = four;

            return ExampleTree;
        }

        public static InductiveMinerTreeNode CreateExampleXY(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            one.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLoop);
            one.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            three.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            three.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            five.LeftLeaf = six;
            InductiveMinerTreeNode seven = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            five.RightLeaf = seven;
            InductiveMinerTreeNode eigth = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);
            ExampleTree.RightLeaf = eigth;
            InductiveMinerTreeNode nine = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("E") };
            eigth.LeftLeaf = nine;
            InductiveMinerTreeNode ten = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("F") };
            eigth.RightLeaf = ten;

            return ExampleTree;
        }


        public static InductiveMinerTreeNode CreateExampleYZ(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            one.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLoop);
            one.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            three.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isParallel);
            three.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("A") };
            five.LeftLeaf = six;
            InductiveMinerTreeNode seven = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            five.RightLeaf = seven;
            InductiveMinerTreeNode eigth = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.RightLeaf = eigth;
            InductiveMinerTreeNode nine = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("E") };
            eigth.LeftLeaf = nine;
            InductiveMinerTreeNode ten = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("F") };
            eigth.RightLeaf = ten;

            return ExampleTree;
        }

        public static InductiveMinerTreeNode CreateExampleParallelXOR(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) {Event =  new Event("A")};
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isParallel);
            ExampleTree.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("B") };
            two.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);
            two.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("C") };
            four.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("D") };
            four.RightLeaf = six;
            
            return ExampleTree;
        }


        public static InductiveMinerTreeNode CreateExampleKnochenbruch(InductiveMiner miner)
        {
            InductiveMinerTreeNode ExampleTree = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);

            InductiveMinerTreeNode one = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.LeftLeaf = one;
            InductiveMinerTreeNode two = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Befundung") };
            one.RightLeaf = two;
            InductiveMinerTreeNode three = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            one.LeftLeaf = three;
            InductiveMinerTreeNode four = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isParallel);
            three.RightLeaf = four;
            InductiveMinerTreeNode five = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Blutentnahme") };
            four.LeftLeaf = five;
            InductiveMinerTreeNode six = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isXOR);
            four.RightLeaf = six;
            InductiveMinerTreeNode seven = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Röntgen") };
            six.LeftLeaf = seven;
            InductiveMinerTreeNode eight = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("MRT") };
            six.RightLeaf = eight;
            InductiveMinerTreeNode nine = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            three.LeftLeaf = nine;
            InductiveMinerTreeNode ten = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Aufnahme") };
            nine.LeftLeaf = ten;
            InductiveMinerTreeNode eleven = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Anamnese") };
            nine.RightLeaf = eleven;
            InductiveMinerTreeNode twelve = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            ExampleTree.RightLeaf = twelve;
            InductiveMinerTreeNode thirteen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Operation") };
            twelve.LeftLeaf = thirteen;
            InductiveMinerTreeNode fourteen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isSequence);
            twelve.RightLeaf = fourteen;
            InductiveMinerTreeNode fiveteen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLoop);
            fourteen.LeftLeaf = fiveteen;
            InductiveMinerTreeNode sixteen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Entlassen") };
            fourteen.RightLeaf = sixteen;
            InductiveMinerTreeNode seventeen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Verband anlegen") };
            fiveteen.LeftLeaf = seventeen;
            InductiveMinerTreeNode eigthteen = new InductiveMinerTreeNode(miner, operation: OperationsEnum.isLeaf) { Event = new Event("Verband entfernen") };
            fiveteen.RightLeaf = eigthteen;

            return ExampleTree;
        }

    }
}
