using Global.SaveLoad;
using Zenject;

public class PlayerSaveLoadService
{
    private IStorageService _storageService;

    [Inject]
    public void Construct(IStorageService storageService)
    {
        _storageService = storageService;
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
            _storageService.Save(ProjectConstantKeys.PLAYER_SAVE_DATA, data);
        }
        return data;
    }
}