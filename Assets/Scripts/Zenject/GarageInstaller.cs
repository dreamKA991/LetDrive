using Zenject;

public class GarageInstaller : MonoInstaller
{

    public GarageUIView GarageUIView;
    public override void InstallBindings()
    {
        BindServices();
    }

    private void BindServices()
    {
        CarPodiumSpawnerService carPodiumSpawnerService = Container.InstantiateComponentOnNewGameObject<CarPodiumSpawnerService>();
        Container.Bind<CarPodiumSpawnerService>().FromInstance(carPodiumSpawnerService).AsSingle();
        
        Container.Bind<GarageUIView>().FromInstance(GarageUIView).AsSingle();
        
        Container.Bind<ShowCarCommand>().FromNew().AsSingle();
        
        Container.Bind<UpgradeService>().FromNew().AsSingle();
    }
}