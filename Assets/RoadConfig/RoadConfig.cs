using UnityEngine;

[CreateAssetMenu(fileName = "RoadConfig", menuName = "Scriptable Objects/RoadConfig")]
public class RoadConfig : ScriptableObject
{
    public GameObject RoadPrefab;
    public float[] XCoordLines;
}
