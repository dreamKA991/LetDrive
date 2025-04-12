using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MarketConfig", menuName = "Scriptable Objects/MarketConfig")]
public class MarketConfig : ScriptableObject
{
    public List<CarData> Cars;
}

