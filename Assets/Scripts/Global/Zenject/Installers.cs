using Global.SaveLoad;
using UnityEngine;
using Zenject;

public class Installers : MonoInstaller
{
    public override void InstallBindings()
    {
        BindServices();
        BindFactories();
    }

    private void BindServices()
    {
        Container.Bind<IStorageService>().To<JsonToFileStorageService>().FromNew().AsSingle();
        Container.Bind<CharacterCarSpawnerService>().FromComponentInHierarchy().AsSingle();
    }

    private void BindFactories()
    {
        
    }
}