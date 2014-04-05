using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm;
using pgmpm.MiningAlgorithm.InductiveV2;

namespace pgmpm.MiningAlgorithmTests
{
    [TestClass()]
    public class MinerFactoryTests
    {
        /// <author>Jannik Arndt</author>
        [TestMethod()]
        public void CreateMinerTest()
        {
            var field = new Field();

            var miner = MinerFactory.CreateMiner("Alpha Miner", field);
            Assert.IsInstanceOfType(miner, typeof(AlphaMiner));

            miner = MinerFactory.CreateMiner("Heuristic Miner", field);
            Assert.IsInstanceOfType(miner, typeof(HeuristicMiner));

            MinerSettings.ListOfMinerSettings["InductiveMiner"] = "InductiveMiner";
            miner = MinerFactory.CreateMiner("Inductive Miner", field);
            Assert.IsInstanceOfType(miner, typeof(InductiveMiner));

            MinerSettings.ListOfMinerSettings["InductiveMiner"] = "InductiveMinerInfrequent";
            miner = MinerFactory.CreateMiner("Inductive Miner", field);
            Assert.IsInstanceOfType(miner, typeof(InductiveMinerInfrequent));
        }

        /// <author>Jannik Arndt</author>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMinerTestFail()
        {
            var field = new Field();
            MinerFactory.CreateMiner("Not existing Miner", field);
        }

        /// <author>Jannik Arndt</author>
        [TestMethod()]
        public void ListOfMinersTest()
        {
            var list = MinerFactory.ListOfMiners;
            Assert.IsNotNull(list["Alpha Miner"]);
        }
    }
}
