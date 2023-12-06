using System.Collections.Generic;
using Control;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "Musical Character Parts", menuName = "Lich/Musical Character Parts", order = 0)]
    public class MusicalCharacterPartsSO : ScriptableObject
    {
        [SerializeField]
        private Sprite defaultHead;
        [SerializeField]
        private Sprite defaultTop;
        [SerializeField]
        private Sprite defaultSkirt;
        
        [SerializeField]
        private List<MusicalCharacterPart> heads;
        [SerializeField]
        private List<MusicalCharacterPart> tops;
        [SerializeField]
        private List<MusicalCharacterPart> skirts;

        public Sprite GetHeadSprite(string head)
        {
            var pair = heads.Find(part => part.One.Equals(head));
            return pair != null ? pair.Two : defaultHead;
        }

        public Sprite GetTopSprite(string top)
        {
          var pair = tops.Find(part => part.One.Equals(top));
          return pair != null ? pair.Two : defaultTop;
        }

        public Sprite GetSkirtSprite(string skirt)
        {
            var pair = skirts.Find(part => part.One.Equals(skirt));
            return pair != null ? pair.Two : defaultSkirt;
        }
    }
}