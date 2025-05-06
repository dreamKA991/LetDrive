using Global.SaveLoad;
using Zenject;

public class ShowCarCommand
{
    private CarPodiumSpawnerService _carPodiumSpawnerService;
    private MarketConfig _marketConfig;
    private GarageUIView _garageUIView;
    private PlayerDataService _playerDataService;
    private UpgradeService _upgradeService;
    private int _selectedCar;

    [Inject]
    private void Construct(CarPodiumSpawnerService carPodiumSpawnerService, MarketConfig marketConfig,
        GarageUIView garageUIReactionService, PlayerDataService playerDataService, UpgradeService upgradeService, IStorageService storageService)
    {
        _carPodiumSpawnerService = carPodiumSpawnerService;
        _marketConfig = marketConfig;
        _garageUIView = garageUIReactionService;
        _playerDataService = playerDataService;
        _upgradeService = upgradeService;
        _selectedCar = int.Parse(storageService.Load<string>(ProjectConstantKeys.SELECTEDCARINDEX)) - 1;
    }

    public void TryBuyCar()
    {
        if (_playerDataService.TryPurchaseCar(_selectedCar))
        {
            UpdateMoneyText();
            UpdateAndSetActiveBuyUIGroup(false);
        }
    }

    private void TryBuyUpgrade(System.Action<int> upgradeAction)
    {
        if (_playerDataService.TryPurchaseUpgrade())
        {
            UpdateMoneyText();
            upgradeAction(_selectedCar);
            _upgradeService.UpdateUpgradeButtons(_selectedCar);
        }
    }

    public void TryBuySpeedUpgrade()
    {
        TryBuyUpgrade(_upgradeService.UpgradeSpeed);
        UpdateCarInfo();
    }

    public void TryBuyBrakeUpgrade()
    {
        TryBuyUpgrade(_upgradeService.UpgradeBrake);
        UpdateCarInfo();
    }

    public void TryBuyHandlingUpgrade()
    {
        TryBuyUpgrade(_upgradeService.UpgradeHandling);
        UpdateCarInfo();
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
        _garageUIView.SetMoneyText(data.Money);
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
        if(isCarPurchased) _playerDataService.SaveSelectedCar(_selectedCar);
        UpdateAndSetActiveBuyUIGroup(!isCarPurchased);
        UpdateCarInfo();
    }

    private void UpdateAndSetActiveBuyUIGroup(bool value)
    {
        _garageUIView.SetCarPriceText(_marketConfig.Cars[_selectedCar].Price);
        _garageUIView.SetActiveBuyButtonAndPriceText(value);

        _upgradeService.UpdateUpgradeButtons(_selectedCar);
    }
    
    private void UpdateCarInfo()
    {
        var data = _playerDataService.LoadData();
        _garageUIView.SetSpeedUI(_marketConfig.Cars[_selectedCar].Speed + data.CarDatas[_selectedCar].speedLevel * 5f);
        _garageUIView.SetHandlingUI(_marketConfig.Cars[_selectedCar].Handling + data.CarDatas[_selectedCar].handlingLevel * 5f);
        _garageUIView.SetBrakingUI(_marketConfig.Cars[_selectedCar].Braking + data.CarDatas[_selectedCar].brakingLevel * 5f);
    }
}
