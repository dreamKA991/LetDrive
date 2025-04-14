using Zenject;

public class MenuInstallers : MonoInstaller
{
    public RoadConfig RoadConfig;
    
    public override void InstallBindings()
    {
        BindServices();
        BindFactories();
    }

    private void BindServices()
    {
        Container.Bind<RoadConfig>().FromInstance(RoadConfig).AsSingle();
        
        TrafficSpawnerService trafficSpawnerService = Container.InstantiateComponentOnNewGameObject<TrafficSpawnerService>();
        Container.Bind<TrafficSpawnerService>().FromInstance(trafficSpawnerService).AsSingle();
    }

    private void BindFactories()
    {
        BindCharacterCarFactory();
        BindRoadFactory();
    }
    
    private void BindCharacterCarFactory()
    {
        CharacterCarSpawnerFactory characterCarSpawnerFactory = Container
            .InstantiateComponentOnNewGameObject<CharacterCarSpawnerFactory>();
        Container.Bind<CharacterCarSpawnerFactory>().FromInstance(characterCarSpawnerFactory).AsSingle().NonLazy();
    }
    
    private void BindRoadFactory()
    {
        RoadSpawnerFactory roadSpawnerFactory = Container
            .InstantiateComponentOnNewGameObject<RoadSpawnerFactory>();
        Container.Bind<RoadSpawnerFactory>().FromInstance(roadSpawnerFactory).AsSingle().NonLazy();
    }
}