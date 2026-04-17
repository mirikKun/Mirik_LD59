using System.IO;
using UnityEngine;

namespace Code.Infrastructure.Saving
{
    public class BinarySavingService : ISavingService
    {
        private readonly string _saveDirectory;

        public BinarySavingService()
        {
            _saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        public void Save<T>(string key, T data) where T : BaseSaveModel<T>
        {
            string filePath = GetFilePath(key);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }

        public T Load<T>(string key) where T : BaseSaveModel<T>
        {
            string filePath = GetFilePath(key);
            
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(json))
                {
                    return default(T);
                }
                
                return JsonUtility.FromJson<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        public bool HasKey(string key)
        {
            string filePath = GetFilePath(key);
            return File.Exists(filePath);
        }

        public void Delete(string key)
        {
            string filePath = GetFilePath(key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(_saveDirectory, $"{key}.dat");
        }
    }
}

