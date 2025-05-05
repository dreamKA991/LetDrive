using Global.SaveLoad;
using Zenject;

public class UpgradeService
{
    private const int MAX_UPGRADE_LEVEL = 2;
    private PlayerDataService _playerDataService;
    private GarageUIView _garageUIView;
    
    [Inject]
    public UpgradeService(PlayerDataService playerDataService, GarageUIView garageUIView)
    {
        _playerDataService = playerDataService;
        _garageUIView = garageUIView;
    }
    
    public void UpdateUpgradeButtons(int selectedCar)
    {
        var data = _playerDataService.LoadData();
        
        bool isPurchased = data.CarDatas[selectedCar].isPurchased;
        _garageUIView.SetUpgradeButtonsActive(isPurchased);
        
        if(data.CarDatas[selectedCar].speedLevel >= MAX_UPGRADE_LEVEL) _garageUIView.SetSpeedUpgradeButtonActive(false);
        if(data.CarDatas[selectedCar].brakingLevel >= MAX_UPGRADE_LEVEL) _garageUIView.SetBrakingUpgradeButtonActive(false);
        if(data.CarDatas[selectedCar].handlingLevel >= MAX_UPGRADE_LEVEL) _garageUIView.SetHandlingUpgradeButtonActive(false);
        
    }

    public void UpgradeSpeed(int selectedCar)
    {
        var data = _playerDataService.LoadData();
        
        if(data.CarDatas[selectedCar].speedLevel >= MAX_UPGRADE_LEVEL) return;
        
        PlayerCarData carData = data.CarDatas[selectedCar];
        carData.speedLevel += 1;
        data.CarDatas[selectedCar] = carData;
        
        _playerDataService.SavePlayerData(data);
    }
    
    public void UpgradeBrake(int selectedCar)
    {
        var data = _playerDataService.LoadData();
        
        if(data.CarDatas[selectedCar].brakingLevel >= MAX_UPGRADE_LEVEL) return;
        
        PlayerCarData carData = data.CarDatas[selectedCar];
        carData.brakingLevel += 1;
        data.CarDatas[selectedCar] = carData;
        
        _playerDataService.SavePlayerData(data);
    }
    
    public void UpgradeHandling(int selectedCar)
    {
        var data = _playerDataService.LoadData();
        
        if(data.CarDatas[selectedCar].handlingLevel >= MAX_UPGRADE_LEVEL) return;
        
        PlayerCarData carData = data.CarDatas[selectedCar];
        carData.handlingLevel += 1;
        data.CarDatas[selectedCar] = carData;
        
        _playerDataService.SavePlayerData(data);
    }
}
