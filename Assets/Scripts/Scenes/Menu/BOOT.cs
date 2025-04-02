using Global.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class BOOT : MonoBehaviour
{
    private IStorageService _storageService;
    private CharacterCarSpawnerFactory _characterCarSpawnerFactory;
    private RoadSpawnerFactory _roadSpawnerFactory;
    private CameraFollow _cameraFollow;
    private DiContainer _container;

    [Inject]
    private void Construct(IStorageService storageService, CharacterCarSpawnerFactory characterCarSpawnerFactory, RoadSpawnerFactory roadSpawnerFactory)
    {
        _storageService = storageService;
        _characterCarSpawnerFactory = characterCarSpawnerFactory;
        _roadSpawnerFactory = roadSpawnerFactory;
        _cameraFollow = Camera.main.AddComponent<CameraFollow>();
        LoadSettings();
        StartGame();
    }

    private void StartGame()
    {
        var iCharacterCar = _characterCarSpawnerFactory.LoadAndSpawnCar();
        _roadSpawnerFactory.Init(iCharacterCar.GetTransform());
        _cameraFollow.Init(iCharacterCar, false);
    }
    
    private void LoadSettings()
    {
        SettingsData _settingsData = _storageService.Load<SettingsData>(ProjectConstantKeys.SETTINGSDATA);
        if (_settingsData != null)
        {
            
        }
    }
}
