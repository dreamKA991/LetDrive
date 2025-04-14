using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class CarPodiumSpawnerService : MonoBehaviour
{
    private MarketConfig _marketConfig; 
    private IStorageService _storageService;
    private GameObject _car;
    private Vector3 _spawnPosition = new Vector3(0f,0.22f,0);
    private Quaternion _spawnRotation = Quaternion.Euler(0, 190, 0);
    [Inject]
    private void Construct(IStorageService storageService, MarketConfig marketConfig)
    {
        _storageService = storageService;
        _marketConfig = marketConfig;
    }
    
    public void SpawnCar(int index)
    {
        if (_car != null) DestroyOldCar();
        _car = _marketConfig.Cars[index].Prefab;
        GameObject newCar = Instantiate(_car, _spawnPosition, _spawnRotation);
        _car = newCar;
        Destroy(newCar.GetComponent<PrometeoCarController>());
    }

    public void SpawnSavedSelectedCar()
    {
        string carIndex = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARINDEX);
        
        
        if (carIndex == null)
        {
            Debug.LogWarning("Spawning default car");
            _car = Instantiate(Resources.Load<GameObject>("Cars/car" + ProjectConstantKeys.DEFAULTCARINDEX), _spawnPosition, _spawnRotation, null);
            _storageService.Save(ProjectConstantKeys.SELECTEDCARINDEX, ProjectConstantKeys.DEFAULTCARINDEX);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + carIndex);
            _car = Instantiate(Resources.Load<GameObject>("Cars/car" + carIndex), _spawnPosition, _spawnRotation, null);
        }
        Destroy(_car.GetComponent<PrometeoCarController>());
    }

    private void DestroyOldCar()
    {
        Destroy(_car);
    }
}
