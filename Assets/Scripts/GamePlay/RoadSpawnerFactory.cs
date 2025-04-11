using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class RoadSpawnerFactory : MonoBehaviour
{
    private GameObject _prefab;
    private float _roadOffset = 10f;  
    private float _roadDeltaZ = 30f;   
    private Queue<Transform> _roads = new Queue<Transform>(); 
    private Transform _playerTransform; 
    private const int INITIAL_ROADS = 20;
    private const int ROAD_DELAY_PER_FIRST_SPAWN_AI_CARS = 1;
    private TrafficSpawnerService _trafficSpawnerService;
    [Inject]
    private void Construct(RoadConfig roadConfig, TrafficSpawnerService trafficSpawnerService)
    {
        _prefab = roadConfig.RoadPrefab;
        _trafficSpawnerService = trafficSpawnerService;
    }
    
    public void Init(ICharacterCar player)
    {
        _playerTransform = player.GetTransform();
        SpawnInitialRoads();
    }

    private void SpawnInitialRoads()
    {
        for (int i = 0; i < INITIAL_ROADS; i++)
        {
            Vector3 spawnPosition = Vector3.forward * (i * _roadOffset);
            GameObject newRoad = Instantiate(_prefab, spawnPosition, Quaternion.identity);
            _roads.Enqueue(newRoad.transform);
            if (i > ROAD_DELAY_PER_FIRST_SPAWN_AI_CARS)
            {
                Transform lastRoad = GetLastRoad();
                _trafficSpawnerService.TrySpawnCar(lastRoad);
            }
        }
    }

    private void Update()
    {
        Transform firstRoad = _roads.Peek();
        if (firstRoad.position.z < _playerTransform.position.z - _roadDeltaZ)
        {
            MoveRoad();
        }
    }

    private void MoveRoad()
    {
        Transform road = _roads.Dequeue();
        Transform lastRoad = GetLastRoad();
        
        road.position = lastRoad.position + Vector3.forward * _roadOffset;
        _trafficSpawnerService.TrySpawnCar(lastRoad);
        _roads.Enqueue(road);
    }

    private Transform GetLastRoad()
    {
        Transform last = null;
        foreach (Transform road in _roads)
        {
            last = road;
        }
        return last;
    }
}