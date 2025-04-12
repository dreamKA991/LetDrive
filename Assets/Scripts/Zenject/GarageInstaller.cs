using Zenject;

public class GarageInstaller : MonoInstaller
{
    public MarketConfig MarketConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<MarketConfig>().FromInstance(MarketConfig).AsSingle();
        BindServices();
    }

    private void BindServices()
    {
        CarPodiumSpawnerService carPodiumSpawnerService = Container.InstantiateComponentOnNewGameObject<CarPodiumSpawnerService>();
        Container.Bind<CarPodiumSpawnerService>().FromInstance(carPodiumSpawnerService).AsSingle();
    }
}