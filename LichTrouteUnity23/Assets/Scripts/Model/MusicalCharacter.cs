using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    struct MusicalCharacterJSON
    {
        public string character;
        public string instrument;        
    }
    
    [Serializable]
    public struct MusicalCharacter
    {
        public string UID;
        public string character;
        public string instrument;
        
        public MusicalCharacter(string jsonData)
        {
            var musicalCharacterJSON = JsonUtility.FromJson<MusicalCharacterJSON>(jsonData);
            UID = MusicalCharacterGenerateUID();
            character = musicalCharacterJSON.character;
            instrument = musicalCharacterJSON.instrument;
        }

        public MusicalCharacter(string character, string instrument)
        {
            UID = MusicalCharacterGenerateUID();
            this.character = character;
            this.instrument = instrument;
        }

        public MusicalCharacter(string uid, string character, string instrument)
        {
            UID = uid;
            this.character = character;
            this.instrument = instrument;
        }

        private static string MusicalCharacterGenerateUID()
        {
            return Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"[{UID}]-{character}-{instrument}";
        }
    }
}