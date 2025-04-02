using Global.SaveLoad;
using Zenject;

public class MenuInstallers : MonoInstaller
{
    public RoadConfig _roadConfig;
    public override void InstallBindings()
    {
        BindServices();
        BindFactories();
    }

    private void BindServices()
    {
        Container.Bind<RoadConfig>().FromInstance(_roadConfig).AsSingle();
        Container.Bind<IStorageService>().To<JsonToFileStorageService>().FromNew().AsSingle().NonLazy();
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