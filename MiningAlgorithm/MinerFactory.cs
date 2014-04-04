using System;
using System.Collections.Generic;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm.InductiveV2;

namespace pgmpm.MiningAlgorithm
{
    public static class MinerFactory
    {
        private readonly static Dictionary<String, String> _ListOfMiners = new Dictionary<String, String>()
        {
            { "Alpha Miner", "/Content/alphaMinerConfig.xaml" },
            { "Heuristic Miner", "/Content/heuristicMinerConfig.xaml" },
            { "Inductive Miner", "/Content/InductiveMinerConfig.xaml" }
        };

        public static Dictionary<String, String> ListOfMiners
        {
            get { return _ListOfMiners; }
        }

        public static IMiner CreateMiner(String miner, Field field)
        {
            switch (miner)
            {
                case "Alpha Miner":
                    return new AlphaMiner(field);
                case "Heuristic Miner":
                    return new HeuristicMiner(field);
                case "Inductive Miner":
                    if (MinerSettings.ListOfMinerSettings["InductiveMiner"].ToString().Equals("InductiveMiner"))
                        return new InductiveMiner(field);
                    return new InductiveMinerInfrequent(field);
                default:
                    throw new ArgumentException("Invalid miner", "miner");
            }
        }
    }
}