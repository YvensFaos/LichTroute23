using System;
using FMODUnity;
using Utils;

namespace Control
{
    [Serializable]
    public class InstrumentFMODPair : Pair<string, EventReference>
    {
        public InstrumentFMODPair(string one, EventReference two) : base(one, two)
        { }

        public override string ToString()
        {
            return $"{One}: {Two.Path}";
        }
    }
}