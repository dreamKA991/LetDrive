using System;
namespace Global.SaveLoad
{
    public interface IStorageService
    {
        void Save(string key, object data, Action<bool> callback = null);
        T Load<T>(string key);
    }
}