using System;

namespace Global.SaveLoad
{
    [Serializable]
    public struct PlayerCarData
    {
        public bool isPurchased;
        public int speedLevel, handlingLevel, brakingLevel;
    }
}