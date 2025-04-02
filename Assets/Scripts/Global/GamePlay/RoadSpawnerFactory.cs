using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class RoadSpawnerFactory : MonoBehaviour
{
    private GameObject _prefab;
    private float _roadOffset = 10f;  
    private float roadDeltaZ = 30f;   
    private Queue<Transform> roads = new Queue<Transform>(); 
    private Transform _player; 
    private const int INITIAL_ROADS = 20;

    [Inject]
    private void Construct(RoadConfig roadConfig)
    {
        _prefab = roadConfig.RoadPrefab;
    }
    
    public void Init(Transform player)
    {
        _player = player;
        SpawnInitialRoads();
    }

    private void SpawnInitialRoads()
    {
        for (int i = 0; i < INITIAL_ROADS; i++)
        {
            Vector3 spawnPosition = Vector3.forward * (i * _roadOffset);
            GameObject newRoad = Instantiate(_prefab, spawnPosition, Quaternion.identity);
            roads.Enqueue(newRoad.transform);
        }
    }

    private void Update()
    {
        Transform firstRoad = roads.Peek();
        if (firstRoad.position.z < _player.position.z - roadDeltaZ)
        {
            MoveRoad();
        }
    }

    private void MoveRoad()
    {
        Transform road = roads.Dequeue();
        
        Transform lastRoad = GetLastRoad();
        
        road.position = lastRoad.position + Vector3.forward * _roadOffset;
        
        roads.Enqueue(road);
    }

    private Transform GetLastRoad()
    {
        Transform last = null;
        foreach (Transform road in roads)
        {
            last = road;
        }
        return last;
    }
}