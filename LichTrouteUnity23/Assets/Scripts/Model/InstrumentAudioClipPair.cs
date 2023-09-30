using System;
using UnityEngine;
using Utils;

namespace Model
{
    [Serializable]
    public class InstrumentAudioClipPair : Pair<MusicalInstrumentSO, AudioClip>
    {
        public InstrumentAudioClipPair(MusicalInstrumentSO one, AudioClip two) : base(one, two)
        { }
    }
}