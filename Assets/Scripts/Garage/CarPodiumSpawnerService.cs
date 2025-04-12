using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class CarPodiumSpawnerService : MonoBehaviour
{
    private MarketConfig _marketConfig; 
    private IStorageService _storageService;
    private GameObject _car;
    private int _selectedCar;
    private Vector3 _spawnPosition = new Vector3(0f,0.22f,0);
    
    [Inject]
    private void Construct(MarketConfig marketConfig, IStorageService storageService)
    {
        _marketConfig = marketConfig;
        _storageService = storageService;
    }

    public void SpawnNextCar()
    {
        if (_selectedCar < _marketConfig.Cars.Count)
        {
            _selectedCar++;
        }
        else
        {
            _selectedCar = 0;
        }
        SpawnCar(_selectedCar);
    }
    
    public void SpawnPreviousCar() {}

    private void SpawnCar(int index)
    {
        if (_car != null) Destroy(_car);
        _car = _marketConfig.Cars[index].Prefab;
        GameObject newCar = Instantiate(_car, _spawnPosition, transform.rotation);
        Destroy(newCar.GetComponent<PrometeoCarController>());
    }

    public void SpawnFirstCar()
    {
        string carName = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARNAME);
        Quaternion quaternion = Quaternion.Euler(0, 190, 0);
        
        if (carName == null)
        {
            Debug.LogWarning("Spawning default car");
            _car = Instantiate(Resources.Load<GameObject>("Cars/" + ProjectConstantKeys.DEFAULTCARPREFABNAME), _spawnPosition, quaternion, null);
            _storageService.Save(ProjectConstantKeys.SELECTEDCARNAME, ProjectConstantKeys.DEFAULTCARPREFABNAME);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + carName);
            _car = Instantiate(Resources.Load<GameObject>("Cars/" + carName), _spawnPosition, quaternion, null);
        }
        Destroy(_car.GetComponent<PrometeoCarController>());
    }
}
