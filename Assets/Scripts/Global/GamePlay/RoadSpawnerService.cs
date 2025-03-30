using UnityEngine;
using Zenject;

public class RoadSpawnerService : MonoBehaviour
{
    //[SerializeField] private DoubleRoad roadData;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int roadLength = 10; // Количество дорог на старте
    [SerializeField] private float roadOffset = 10f; // Смещение следующей дороги
    
    private Transform lastRoad;
    
    private void Start()
    {
        
        SpawnInitialRoad();
    }
    
    private void SpawnInitialRoad()
    {
        for (int i = 0; i < roadLength; i++)
        {
            SpawnRoad();
        }
    }
    
    public void SpawnRoad()
    {
        Vector3 spawnPosition = lastRoad == null ? Vector3.zero : lastRoad.position + Vector3.forward * roadOffset;
        GameObject newRoad = Instantiate(prefab, spawnPosition, Quaternion.identity);
        lastRoad = newRoad.transform;
    }
}