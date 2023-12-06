using System.Collections.Generic;
using Control;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "Musical Character Parts", menuName = "Lich/Musical Character Parts", order = 0)]
    public class MusicalCharacterPartsSO : ScriptableObject
    {
        [SerializeField]
        private List<MusicalCharacterPart> heads;
        [SerializeField]
        private List<MusicalCharacterPart> tops;
        [SerializeField]
        private List<MusicalCharacterPart> skirts;

        public Sprite GetHeadSprite(string head) => heads.Find(part => part.One.Equals(head)).Two;
        public Sprite GetTopSprite(string top) => tops.Find(part => part.One.Equals(top)).Two;
        public Sprite GetSkirtSprite(string skirt) => skirts.Find(part => part.One.Equals(skirt)).Two;
    }
}