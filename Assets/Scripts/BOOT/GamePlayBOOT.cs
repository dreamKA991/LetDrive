using Global.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource), typeof(PlayerUIProviderService))]
public class GamePlayBOOT : MonoBehaviour
{
    private IStorageService _storageService;
    private CharacterCarSpawnerFactory _characterCarSpawnerFactory;
    private RoadSpawnerFactory _roadSpawnerFactory;
    private CameraFollow _cameraFollow;
    private PlayerSettingsService _playerSettingsService;
    private PlayerUIProviderService _playerUIProviderService;
    
    [Inject]
    private void Construct(IStorageService storageService, CharacterCarSpawnerFactory characterCarSpawnerFactory, RoadSpawnerFactory roadSpawnerFactory, PlayerSettingsService playerSettingsService)
    {
        _storageService = storageService;
        _characterCarSpawnerFactory = characterCarSpawnerFactory;
        _roadSpawnerFactory = roadSpawnerFactory;
        _cameraFollow = Camera.main.AddComponent<CameraFollow>();
        _playerSettingsService = playerSettingsService;
        _playerUIProviderService = GetComponent<PlayerUIProviderService>();
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        var characterCarController = _characterCarSpawnerFactory.LoadAndSpawnCar(false);
        _playerUIProviderService.ApplyGUI(characterCarController);
        _roadSpawnerFactory.Init(characterCarController);
        _cameraFollow.Init(characterCarController, true);
        LoadAndApplySettings();
    }
    
    private void LoadAndApplySettings()
    {
        _playerSettingsService.LoadAndApplySettings();
    }
}
