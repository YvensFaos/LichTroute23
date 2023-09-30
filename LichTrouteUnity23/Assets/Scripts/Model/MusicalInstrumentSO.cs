using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "Musical Instrument", menuName = "Lich/Musical Instrument", order = 0)]
    public class MusicalInstrumentSO : ScriptableObject
    {
        public bool CheckInstrument(string instrumentName)
        {
            return name.ToLower().Equals(instrumentName.ToLower());
        }
    }
}