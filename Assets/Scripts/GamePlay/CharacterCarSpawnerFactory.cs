using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class CharacterCarSpawnerFactory : MonoBehaviour
{
    private IStorageService _storageService;
    private DiContainer _container;
    private const int ROADLANESPAWN = 0;
    
    [Inject]
    public void Construct(IStorageService storageService, DiContainer container)
    {
        _storageService = storageService;
        _container = container;
        transform.position = new Vector3(_container.Resolve<RoadConfig>().XCoordLines[ROADLANESPAWN],0.1f ,0);
    }

    public ICharacterCar LoadAndSpawnCar(bool isBot = true)
    {
        string _carName = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARNAME);
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);
        GameObject obj;
        
        if (_carName == null)
        {
            Debug.LogWarning("Spawning default car");
            obj = _container.InstantiatePrefab(Resources.Load<GameObject>("Cars/" + ProjectConstantKeys.DEFAULTCARPREFABNAME), transform.position, quaternion, null);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + _carName);
            obj = _container.InstantiatePrefab(Resources.Load<GameObject>("Cars/" + _carName), transform.position, quaternion, null);
        }
        PrometeoCarController prometeoCarController;
        prometeoCarController = obj.GetComponent<PrometeoCarController>();
        prometeoCarController.IsBot = isBot;
        prometeoCarController.RoadLaneTaked = ROADLANESPAWN;
        _container.Bind<ICharacterCar>().FromInstance(prometeoCarController).AsSingle();
        if (isBot)
        {
            prometeoCarController.gameObject.AddComponent<CharacterAILogic>();
            prometeoCarController.MaxBotCarSpeed = 25f;
            prometeoCarController.MinGameCarSpeed = 20f;
        }
        return prometeoCarController;
    }
}   