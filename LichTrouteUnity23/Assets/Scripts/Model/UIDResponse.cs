using System;

namespace Model
{
    [Serializable]
    public struct UIDResponse
    {
        public string UID;
        public int queueSize;
    }
}