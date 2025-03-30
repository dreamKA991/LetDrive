using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class CharacterCarSpawnerService : MonoBehaviour
{
    private IStorageService _storageService;
    [SerializeField] private GameObject _testCharacterCarPrefab;
    [Inject]
    public void Construct(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public void LoadAndSpawnCar()
    {
        string _carName = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARNAME);
        Quaternion quaternion = Quaternion.Euler(0, -90, 0);
        GameObject obj;
        if (_carName == null)
        {
            Debug.LogWarning("Spawning default car");
            obj = Instantiate(_testCharacterCarPrefab, transform.position, quaternion);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + _carName);
            obj = Instantiate(Resources.Load<GameObject>("Cars/Prefabs/" + _carName), transform.position, quaternion);
        }
        //obj.AddComponent<>()
    }
}   