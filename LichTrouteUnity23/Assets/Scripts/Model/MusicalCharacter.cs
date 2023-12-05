using System;
using Control;
using UnityEngine;

namespace Model
{
    [Serializable]
    struct MusicalCharacterJSON
    {
        public string head;
        public string body;
        public string parameter;
    }
    
    [Serializable]
    public struct MusicalCharacter
    {
        public string UID;
        public string head;
        public string body;
        public string parameter;
        
        public MusicalCharacter(string jsonData)
        {
            var musicalCharacterJSON = JsonUtility.FromJson<MusicalCharacterJSON>(jsonData);
            UID = MusicalCharacterGenerateUID();
            head = musicalCharacterJSON.head;
            body = musicalCharacterJSON.body;
            parameter = musicalCharacterJSON.parameter;
        }

        public MusicalCharacter(string head, string body, string parameter)
        {
            UID = MusicalCharacterGenerateUID();
            this.head = head;
            this.body = body;
            this.parameter = parameter;
        }

        public MusicalCharacter(string uid, string head, string body, string parameter)
        {
            UID = uid;
            this.head = head;
            this.body = body;
            this.parameter = parameter;
        }

        private static string MusicalCharacterGenerateUID()
        {
            return Guid.NewGuid().ToString();
        }

        public MusicalInstrumentParameterPair GetPair()
        {
            return new MusicalInstrumentParameterPair(parameter, 0.0f);
        }

        public string GetInstrument() => parameter;

        public override string ToString()
        {
            return $"[{DateTime.Now}] [{UID}]-HEAD {head} BODY {body} - INSTRUMENT {parameter}";
        }
    }
}