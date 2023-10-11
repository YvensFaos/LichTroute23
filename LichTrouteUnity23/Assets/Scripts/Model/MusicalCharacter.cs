using System;
using Control;
using UnityEngine;

namespace Model
{
    [Serializable]
    struct MusicalCharacterJSON
    {
        public string character;
        public string parameter;
        public float value;
    }
    
    [Serializable]
    public struct MusicalCharacter
    {
        public string UID;
        public string character;
        public string parameter;
        public float value;
        
        public MusicalCharacter(string jsonData)
        {
            var musicalCharacterJSON = JsonUtility.FromJson<MusicalCharacterJSON>(jsonData);
            UID = MusicalCharacterGenerateUID();
            character = musicalCharacterJSON.character;
            parameter = musicalCharacterJSON.parameter;
            value = musicalCharacterJSON.value;
        }

        public MusicalCharacter(string character, string parameter, float value)
        {
            UID = MusicalCharacterGenerateUID();
            this.character = character;
            this.parameter = parameter;
            this.value = value;
        }

        public MusicalCharacter(string uid, string character, string parameter, float value)
        {
            UID = uid;
            this.character = character;
            this.parameter = parameter;
            this.value = value;
        }

        private static string MusicalCharacterGenerateUID()
        {
            return Guid.NewGuid().ToString();
        }

        public MusicalInstrumentParameterPair GetPair()
        {
            return new MusicalInstrumentParameterPair(parameter, value);
        }

        public override string ToString()
        {
            return $"[{UID}]-{character}-{parameter}";
        }
    }
}