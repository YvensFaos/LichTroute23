using System;

namespace Model
{
    [Serializable]
    public struct MusicalCharacterInfo
    {
        public string uid;
        public int queuePosition;
        public bool onStage;
        public int queueSize;
    }
}