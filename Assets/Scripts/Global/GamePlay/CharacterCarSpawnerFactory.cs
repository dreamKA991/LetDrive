using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class CharacterCarSpawnerFactory : MonoBehaviour
{
    private IStorageService _storageService;
    private DiContainer _container;
    
    [Inject]
    public void Construct(IStorageService storageService, DiContainer container)
    {
        _storageService = storageService;
        _container = container;
        transform.position = new Vector3(_container.Resolve<RoadConfig>().XCoordLines[0],0.1f ,0);
    }

    public ICharacterCar LoadAndSpawnCar(bool isBot = true)
    {
        string _carName = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARNAME);
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);
        GameObject obj;
        PrometeoCarController prometeoCarController;
        if (_carName == null)
        {
            Debug.LogWarning("Spawning default car");
            obj = Instantiate(Resources.Load<GameObject>("Cars/Prefabs/" + ProjectConstantKeys.DEFAULTCARPREFABNAME), transform.position, quaternion);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + _carName);
            obj = Instantiate(Resources.Load<GameObject>("Cars/Prefabs/" + _carName), transform.position, quaternion);
        }

        prometeoCarController = obj.GetComponent<PrometeoCarController>();
        prometeoCarController.IsBot = isBot;
        _container.Bind<ICharacterCar>().FromInstance(prometeoCarController).AsSingle();
        return prometeoCarController;
    }
}   