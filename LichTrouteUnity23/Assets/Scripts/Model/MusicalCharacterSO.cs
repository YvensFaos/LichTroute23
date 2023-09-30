using System.Collections.Generic;
using Control;
using UnityEngine;
using Utils;

namespace Model
{
    [CreateAssetMenu(fileName = "Musical Character", menuName = "Lich/Musical Character", order = 0)]
    public class MusicalCharacterSO : ScriptableObject
    {
        [SerializeField]
        public string character;
        [SerializeField]
        private MusicalCharacterBehaviour characterPrefab;
        [SerializeField]
        private List<InstrumentAudioClipPair> instruments;

        public AudioClip GetAudioClipFromInstrument(string instrument)
        {
            var pair = instruments.Find(musicalInstrument => musicalInstrument.One.CheckInstrument(instrument));
            if (pair != null) return pair.Two;
            DebugUtils.DebugLogErrorMsg($"Could not find instrument {instrument}.");
            return null;
        }

        public MusicalCharacterBehaviour Prefab => characterPrefab;
    }
}