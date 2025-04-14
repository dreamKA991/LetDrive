using Zenject;

public class ShowCarCommand
{
    private CarPodiumSpawnerService _carPodiumSpawnerService;
    private MarketConfig _marketConfig;
    private GarageUIReactionService _garageUIReactionService;
    private PlayerDataService _playerDataService;
    private int _selectedCar;
    
    [Inject]
    private void Construct(CarPodiumSpawnerService carPodiumSpawnerService, MarketConfig marketConfig, GarageUIReactionService garageUIReactionService, PlayerDataService playerDataService)
    {
        _carPodiumSpawnerService = carPodiumSpawnerService;
        _marketConfig = marketConfig;
        _garageUIReactionService = garageUIReactionService;
        _playerDataService = playerDataService;
    }

    public void TryBuyCar()
    {
        if (_playerDataService.TryPurchaseCar(_selectedCar))
        {
            UpdateMoneyText();
            UpdateAndSetActiveBuyUIGroup(false);
        }
    }

    public void SpawnSavedSelectedCar()
    {
        _carPodiumSpawnerService.SpawnSavedSelectedCar();
        UpdateAndSetActiveBuyUIGroup(false);
        UpdateCarInfo();
    }

    public void ShowNextCar()
    {
        SelectAndSpawnCar(1);
    }

    public void ShowPreviousCar()
    {
        SelectAndSpawnCar(-1);
    }

    public void UpdateMoneyText()
    {
        var data = _playerDataService.LoadData();
        _garageUIReactionService.SetMoneyText(data.money);
    }

    private void SelectAndSpawnCar(int direction)
    {
        _selectedCar += direction;

        if (_selectedCar < 0)
            _selectedCar = _marketConfig.Cars.Count - 1;
        else if (_selectedCar >= _marketConfig.Cars.Count)
            _selectedCar = 0;

        _carPodiumSpawnerService.SpawnCar(_selectedCar);

        bool isCarPurchased = _playerDataService.IsCarPurchased(_selectedCar);
        UpdateAndSetActiveBuyUIGroup(!isCarPurchased);
        UpdateCarInfo();
    }
    
    private void UpdateCarInfo()
    {
        _garageUIReactionService.SetSpeedUI(_marketConfig.Cars[_selectedCar].Speed);
        _garageUIReactionService.SetHandlingUI(_marketConfig.Cars[_selectedCar].Handling);
        _garageUIReactionService.SetBrakingUI(_marketConfig.Cars[_selectedCar].Braking);
    }

    private void UpdateAndSetActiveBuyUIGroup(bool value)
    {
        _garageUIReactionService.SetCarPriceText(_marketConfig.Cars[_selectedCar].Price);
        _garageUIReactionService.SetActiveBuyButtonAndPriceText(value);
    }
}
