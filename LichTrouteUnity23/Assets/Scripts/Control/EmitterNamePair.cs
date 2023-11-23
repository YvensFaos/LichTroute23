using System;
using FMODUnity;
using Utils;

namespace Control
{
    [Serializable]
    public class EmitterNamePair : Pair<string, StudioEventEmitter>
    {
        public EmitterNamePair(string one, StudioEventEmitter two) : base(one, two)
        { }
    }
}