using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TrafficSpawnerService : MonoBehaviour
{
    private const int INITIAL_CARS = 7;
    private Queue<Transform> _cars = new Queue<Transform>();
    private const int CARS_INTENSITY = 5;
    private int _periodCount = 1;
    private float[] _xRoadCoords;
    [Inject]
    private void Construct(RoadConfig roadConfig)
    {
        _xRoadCoords = roadConfig.XCoordLines; 
        SpawnInitialCars();
    }
    
    private void SpawnInitialCars()
    {
        for (int i = 1; i < INITIAL_CARS; i++)
        {
            GameObject newCar = Instantiate(Resources.Load<GameObject>("Cars/car" + i), Vector3.zero , Quaternion.identity);
            newCar.SetActive(false);
            _cars.Enqueue(newCar.transform);
        }
    }
    
    public void TrySpawnCar(Transform roadTransform)
    {
        if (_periodCount == CARS_INTENSITY)
        {
            _periodCount = 1;
            MoveCarIntoNewPosition(roadTransform);
        }
        else _periodCount++;
    }
    
    private void MoveCarIntoNewPosition(Transform roadTransform)
    {
        Transform oldCar = _cars.Peek();
        _cars.Dequeue();
        int randomLine = Random.Range(0, 2);
        oldCar.GetComponent<PrometeoCarController>().RoadLaneTaked = randomLine;
        oldCar.position = roadTransform.position + (Vector3.right * _xRoadCoords[randomLine]);
        oldCar.gameObject.SetActive(true);
        _cars.Enqueue(oldCar);
    }
}