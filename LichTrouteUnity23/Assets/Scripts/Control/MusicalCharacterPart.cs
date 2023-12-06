using System;
using UnityEngine;
using Utils;

namespace Control
{
    [Serializable]
    public class MusicalCharacterPart : Pair<string, Sprite>
    {
        public MusicalCharacterPart(string one, Sprite two) : base(one, two)
        { }
    }
}