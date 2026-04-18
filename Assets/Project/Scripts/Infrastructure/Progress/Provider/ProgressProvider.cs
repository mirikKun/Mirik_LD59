using Project.Scripts.Infrastructure.Progress.Data;
using Project.Scripts.Infrastructure.Saving;
using Project.Scripts.Infrastructure.StaticData;

namespace Project.Scripts.Infrastructure.Progress.Provider
{
    public class ProgressProvider : IProgressProvider
    {
        private const string ProgressKey = "ProgressData";
        private readonly ISavingService _savingService;
        private readonly IStaticDataService _staticDataService;

        public ProgressData ProgressData { get; private set; }

        public ProgressProvider(ISavingService savingService, IStaticDataService staticDataService)
        {
            _savingService = savingService;
            _staticDataService = staticDataService;
        }

        public void SetProgressData(ProgressData data)
        {
            ProgressData = data;
            SaveProgress();
        }

        public void SaveProgress()
        {
            if (ProgressData != null)
            {
                _savingService.Save(ProgressKey, ProgressData);
            }
        }

        public void LoadProgress()
        {
            ProgressData = _savingService.Load<ProgressData>(ProgressKey);
        }

        public bool HasProgress()
        {
            return _savingService.HasKey(ProgressKey);
        }


        public void CreateDefaultProgress()
        {
            ProgressData = new ProgressData();
            SaveProgress();
        }
    }
}