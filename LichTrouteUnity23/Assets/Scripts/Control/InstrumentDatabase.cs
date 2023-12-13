using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Control
{
    [CreateAssetMenu(fileName = "Instrument Database", menuName = "Instrument Database", order = 0)]
    public class InstrumentDatabase : ScriptableObject
    {
        [SerializeField]
        private List<InstrumentFMODPair> instruments;
        [SerializeField]
        private InstrumentFMODPair defaultInstrument;
        
        public InstrumentFMODPair GetPairForInstrument(string instrument)
        {
            var pair = instruments.Find(instrumentPair => instrumentPair.One.Equals(instrument));
            return pair ?? defaultInstrument;
        }

        public bool IsValidInstrument(string instrument)
        {
            var pair = instruments.Find(instrumentPair => instrumentPair.One.Equals(instrument));
            return pair != null;
        }

        public string GetRandomInstrument()
        {
            return RandomHelper<InstrumentFMODPair>.GetRandomFromList(instruments).One;
        }
    }
}