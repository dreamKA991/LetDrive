using Global.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class MenuBOOT : MonoBehaviour
{
    private IStorageService _storageService;
    private CharacterCarSpawnerFactory _characterCarSpawnerFactory;
    private RoadSpawnerFactory _roadSpawnerFactory;
    private CameraFollow _cameraFollow;
    private SaveLoadPlayerSettingsService _saveLoadPlayerSettingsService;

    [Inject]
    private void Construct(IStorageService storageService, CharacterCarSpawnerFactory characterCarSpawnerFactory, RoadSpawnerFactory roadSpawnerFactory, SaveLoadPlayerSettingsService saveLoadPlayerSettingsService)
    {
        _storageService = storageService;
        _characterCarSpawnerFactory = characterCarSpawnerFactory;
        _roadSpawnerFactory = roadSpawnerFactory;
        _cameraFollow = Camera.main.AddComponent<CameraFollow>();
        _saveLoadPlayerSettingsService = saveLoadPlayerSettingsService;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        var iCharacterCar = _characterCarSpawnerFactory.LoadAndSpawnCar();
        _roadSpawnerFactory.Init(iCharacterCar);
        _cameraFollow.Init(iCharacterCar, false);
        LoadAndApplySettings();
    }
    
    private void LoadAndApplySettings()
    {
        _saveLoadPlayerSettingsService.LoadAndApplySettings();
    }
}
