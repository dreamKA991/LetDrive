using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Global.SaveLoad
{
    public class JsonToFileStorageService : IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null)
        {
            string path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }
            
            callback?.Invoke(true);
        }

        public T Load<T>(string key)
        {
            string path = BuildPath(key);
            
            if (!File.Exists(path))
            {
                return default; // Вернёт null для ссылочных типов или 0 / false для значимых типов
            }

            using (var fileStream = new StreamReader(path))
            {
                var json = fileStream.ReadToEnd();
                var data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
        }
        
        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}