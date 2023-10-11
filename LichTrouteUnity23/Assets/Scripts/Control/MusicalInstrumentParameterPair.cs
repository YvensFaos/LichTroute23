using System;
using Utils;

namespace Control
{
    [Serializable]
    public class MusicalInstrumentParameterPair : Pair<string, float>
    {
        public MusicalInstrumentParameterPair(string one, float two) : base(one, two)
        { }
    }
}