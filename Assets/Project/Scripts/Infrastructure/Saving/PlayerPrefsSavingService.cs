using UnityEngine;

namespace Project.Scripts.Infrastructure.Saving
{
    public class PlayerPrefsSavingService : ISavingService
    {
        public void Save<T>(string key, T data) where T : BaseSaveModel<T>
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key) where T : BaseSaveModel<T>
        {
            if (!HasKey(key))
            {
                return default(T);
            }

            try
            {
                string json = PlayerPrefs.GetString(key);
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
            return PlayerPrefs.HasKey(key);
        }

        public void Delete(string key)
        {
            if (HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
    }
}

