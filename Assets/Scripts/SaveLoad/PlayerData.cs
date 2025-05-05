using System;
using System.Collections.Generic;

namespace Global.SaveLoad
{
    [Serializable]
    public class PlayerData
    {
        public int Money;
        public List<PlayerCarData> CarDatas = new();
    }
}