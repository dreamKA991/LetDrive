using System;
using System.Collections.Generic;

namespace Global.SaveLoad
{
    [Serializable]
    public class PlayerData
    {
        public int money;
        public List<int> purchasedCarIndexes = new();
    }
}