using Global.SaveLoad;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerDataService
{
    private IStorageService _storageService;
    private MarketConfig _marketConfig;
    
    [Inject]
    public void Construct(IStorageService storageService, MarketConfig marketConfig)
    {
        _storageService = storageService;
        _marketConfig = marketConfig;
    }
    
    public void SavePurchasedCar(int index)
    {
        var data = LoadData();
        if (!data.purchasedCarIndexes.Contains(index))
        {
            data.purchasedCarIndexes.Add(index);
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
        }
    }
    
    public bool IsCarPurchased(int index)
    {
        var data = LoadData();
        return data.purchasedCarIndexes.Contains(index);
    }
    
    public PlayerData LoadData()
    {
        var data = _storageService.Load<PlayerData>(ProjectConstantKeys.PLAYER_SAVE_DATA);
        if (data == null)
        {
            data = new PlayerData();
            data.purchasedCarIndexes.Add(int.Parse(ProjectConstantKeys.DEFAULTCARINDEX) - 1);
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
        }
        return data;
    }

    public bool TryPurchaseCar(int index)
    {
        var data = LoadData();
        if (_marketConfig.Cars[index].Price > data.money)
        {
            Debug.Log("Try to purchase car is failed, no money bro :(");
            return false;
        }
        else
        {
            data.money -= _marketConfig.Cars[index].Price;
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
            SavePurchasedCar(index);
            Debug.Log("Try to purchase car is done :)");
            return true;
        }
    }
}