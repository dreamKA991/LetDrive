using UnityEngine;
using Zenject;

public class GarageBOOT : MonoBehaviour
{
    private PlayerSettingsService _playerSettingsService;
    private ShowCarCommand _showCarCommand;

    [Inject]
    private void Construct(PlayerSettingsService playerSettingsService, ShowCarCommand showCarCommand)
    {
        _playerSettingsService = playerSettingsService;
        _showCarCommand = showCarCommand;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        LoadAndApplySettings();
        _showCarCommand.SpawnSavedSelectedCar();
        _showCarCommand.UpdateMoneyText();
    }
    
    private void LoadAndApplySettings()
    {
        _playerSettingsService.LoadAndApplySettings();
    }
}
