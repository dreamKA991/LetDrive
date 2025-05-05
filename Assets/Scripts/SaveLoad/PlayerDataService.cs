using Global.SaveLoad;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerDataService
{
    private IStorageService _storageService;
    private MarketConfig _marketConfig;
    private const int UPGRADECOST = 500;
    
    [Inject]
    public void Construct(IStorageService storageService, MarketConfig marketConfig)
    {
        _storageService = storageService;
        _marketConfig = marketConfig;
    }
    
    public void SavePurchasedCar(int index)
    {
        var data = LoadData();
        
        if (!data.CarDatas[index].isPurchased)
        {
            PlayerCarData carData = data.CarDatas[index];
            carData.isPurchased = true;
            data.CarDatas[index] = carData;
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
        }
    }
    
    public bool IsCarPurchased(int index)
    {
        var data = LoadData();
        return data.CarDatas[index].isPurchased;
    }
    
    public PlayerData LoadData()
    {
        var data = _storageService.Load<PlayerData>(ProjectConstantKeys.PLAYER_SAVE_DATA);
    
        if (data == null)
        {
            data = new PlayerData();
            
            for (int i = 0; i < _marketConfig.Cars.Count; i++)
            {
                data.CarDatas.Add(new PlayerCarData { isPurchased = false });
            }
            
            int defaultIndex = int.Parse(ProjectConstantKeys.DEFAULTCARINDEX) - 1;
            if (defaultIndex >= 0 && defaultIndex < data.CarDatas.Count)
            {
                var carData = data.CarDatas[defaultIndex];
                carData.isPurchased = true;
                data.CarDatas[defaultIndex] = carData;
            }

            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
        }

        return data;
    }

    public void SavePlayerData(PlayerData data) => _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
    
    public bool TryPurchaseCar(int index)
    {
        if (TryPurchase(_marketConfig.Cars[index].Price))
        {
            SavePurchasedCar(index);
            return true;
        } else return false;
    }

    public bool TryPurchaseUpgrade()
    {
        if(TryPurchase(UPGRADECOST)) 
        {
            return true;
        } else return false;
    }
    
    private bool TryPurchase(int cost)
    {
        var data = LoadData();
        if (cost > data.Money)
        {
            Debug.Log("Try to purchase is failed, no money bro :(");
            return false;
        }
        else
        {
            data.Money -= cost;
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
            Debug.Log("Try to purchase is done :)");
            return true;
        }
    }
}