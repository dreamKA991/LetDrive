using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class GarageBOOT : MonoBehaviour
{
    private IStorageService _storageService;
    private SaveLoadPlayerSettingsService _saveLoadPlayerSettingsService;
    CarPodiumSpawnerService _carPodiumSpawnerService;

    [Inject]
    private void Construct(IStorageService storageService, SaveLoadPlayerSettingsService saveLoadPlayerSettingsService, CarPodiumSpawnerService carPodiumSpawnerService)
    {
        _storageService = storageService;
        _saveLoadPlayerSettingsService = saveLoadPlayerSettingsService;
        _carPodiumSpawnerService = carPodiumSpawnerService;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        LoadAndApplySettings();
        _carPodiumSpawnerService.SpawnFirstCar();
    }
    
    private void LoadAndApplySettings()
    {
        _saveLoadPlayerSettingsService.LoadAndApplySettings();
    }
}
