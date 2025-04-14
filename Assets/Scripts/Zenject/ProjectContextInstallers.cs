using Global.SaveLoad;
using Zenject;

public class ProjectContextInstallers : MonoInstaller
{
    public MarketConfig MarketConfig;
    public override void InstallBindings()
    {
        Container.Bind<MarketConfig>().FromInstance(MarketConfig).AsSingle().NonLazy();
        BindServices();
    }

    private void BindServices()
    {
        Container.Bind<IStorageService>().To<JsonToFileStorageService>().FromNew().AsSingle().NonLazy();
        
        Container.Bind<PlayerDataService>().FromNew().AsSingle().NonLazy();
        
        AudioSetterService audioSetterService = Container.InstantiateComponentOnNewGameObject<AudioSetterService>();
        Container.Bind<AudioSetterService>().FromInstance(audioSetterService).AsSingle().NonLazy();
        
        GraphicSetterService graphicSetterService = Container.InstantiateComponentOnNewGameObject<GraphicSetterService>();
        Container.Bind<GraphicSetterService>().FromInstance(graphicSetterService).AsSingle().NonLazy();
        
        Container.Bind<PlayerSettingsService>().FromNew().AsSingle().NonLazy();
    }
}
