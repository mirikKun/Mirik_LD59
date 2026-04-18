namespace Project.Scripts.Infrastructure.Saving
{
    public interface ISavingService
    {
        void Save<T>(string key, T data) where T : BaseSaveModel<T>;
        T Load<T>(string key) where T : BaseSaveModel<T>;
        bool HasKey(string key);
        void Delete(string key);
    }
}

