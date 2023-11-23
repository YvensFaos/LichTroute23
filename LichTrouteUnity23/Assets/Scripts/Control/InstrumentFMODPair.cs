using System;
using FMODUnity;
using Utils;

namespace Control
{
    [Serializable]
    public class InstrumentFMODPair : Pair<string, string>
    {
        public InstrumentFMODPair(string one, string two) : base(one, two)
        { }

        public override string ToString()
        {
            return $"{One}: {Two}";
        }
    }
}