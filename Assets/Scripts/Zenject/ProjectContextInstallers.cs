using Global.SaveLoad;
using Zenject;

public class ProjectContextInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
        BindServices();
    }

    private void BindServices()
    {
        Container.Bind<IStorageService>().To<JsonToFileStorageService>().FromNew().AsSingle().NonLazy();
        
        Container.Bind<PlayerSaveLoadService>().To<PlayerSaveLoadService>().FromNew().AsSingle().NonLazy();
        
        AudioSetterService audioSetterService = Container.InstantiateComponentOnNewGameObject<AudioSetterService>();
        Container.Bind<AudioSetterService>().To<AudioSetterService>().FromInstance(audioSetterService).AsSingle().NonLazy();
        
        GraphicSetterService graphicSetterService = Container.InstantiateComponentOnNewGameObject<GraphicSetterService>();
        Container.Bind<GraphicSetterService>().To<GraphicSetterService>().FromInstance(graphicSetterService).AsSingle().NonLazy();
        
        Container.Bind<SaveLoadPlayerSettingsService>().To<SaveLoadPlayerSettingsService>().FromNew().AsSingle().NonLazy();
    }
}
