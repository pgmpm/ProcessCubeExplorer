using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pgmpm.MiningAlgorithm
{
    public class InductiveMinerTrace
    {
        private List<String> _Trace;
        private int _Frequency;

        public InductiveMinerTrace(List<String> list)
        {
            _Trace = list;
            _Frequency = 1;
        }

        public List<string> Trace
        {
            get { return _Trace; }
            set { _Trace = value; }
        }

        public int Frequency
        {
            get { return _Frequency; }
            set { _Frequency = value; }
        }
    }
}