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

    public PrometeoCarController LoadAndSpawnCar(bool isBot = true)
    {
        string carIndex = _storageService.Load<string>(ProjectConstantKeys.SELECTEDCARINDEX);
        Quaternion quaternion = Quaternion.Euler(0, 0, 0);
        GameObject obj;
        
        if (carIndex == null)
        {
            Debug.LogWarning("Spawning default car");
            obj = _container.InstantiatePrefab(Resources.Load<GameObject>("Cars/car" + ProjectConstantKeys.DEFAULTCARINDEX), transform.position, quaternion, null);
            _storageService.Save(ProjectConstantKeys.SELECTEDCARINDEX, ProjectConstantKeys.DEFAULTCARINDEX);
        }
        else
        {
            Debug.LogWarning("Spawning car: " + carIndex);
            obj = _container.InstantiatePrefab(Resources.Load<GameObject>("Cars/car" + carIndex), transform.position, quaternion, null);
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