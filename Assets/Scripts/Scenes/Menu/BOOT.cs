using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class BOOT : MonoBehaviour
{
    private IStorageService _storageService;
    private CharacterCarSpawnerService _characterCarSpawnerService;
    
    [Inject]
    private void Construct(IStorageService storageService, CharacterCarSpawnerService characterCarSpawneService)
    {
        _storageService = storageService;
        _characterCarSpawnerService = characterCarSpawneService;
        LoadSettings();
        //storageService.Save(ProjectConstantKeys.SELECTEDCARNAME, "car18");
        StartGame();
    }

    private void StartGame()
    {
        _characterCarSpawnerService.LoadAndSpawnCar();
    }
    private void LoadSettings()
    {
        
    }
}
