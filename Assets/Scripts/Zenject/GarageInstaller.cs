using Zenject;

public class GarageInstaller : MonoInstaller
{

    public GarageUIReactionService GarageUIReactionService;
    public override void InstallBindings()
    {
        
        BindServices();
    }

    private void BindServices()
    {
        CarPodiumSpawnerService carPodiumSpawnerService = Container.InstantiateComponentOnNewGameObject<CarPodiumSpawnerService>();
        Container.Bind<CarPodiumSpawnerService>().FromInstance(carPodiumSpawnerService).AsSingle();
        
        Container.Bind<GarageUIReactionService>().FromInstance(GarageUIReactionService).AsSingle();
        
        Container.Bind<ShowCarCommand>().FromNew().AsSingle();
    }
}