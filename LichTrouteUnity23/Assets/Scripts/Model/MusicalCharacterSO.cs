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

        public MusicalCharacterBehaviour Prefab => characterPrefab;
    }
}