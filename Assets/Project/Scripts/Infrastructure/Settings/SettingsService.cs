using Project.Scripts.Infrastructure.Saving;
using Project.Scripts.Infrastructure.Settings.Configs;
using Project.Scripts.Infrastructure.Settings.Data;
using Project.Scripts.Infrastructure.Sounds;
using Project.Scripts.Infrastructure.StaticData;

namespace Project.Scripts.Infrastructure.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsKey = "SettingsData";
        private readonly ISavingService _savingService;
        private readonly ISoundsSystem _soundsSystem;
        private readonly IStaticDataService _staticDataService;

        public SettingsData SettingsData { get; private set; }
        

        public SettingsConfig SettingsConfig => _staticDataService.GetSettingsConfig();

        public SettingsService(ISavingService savingService, ISoundsSystem soundsSystem,
            IStaticDataService staticDataService)
        {
            _soundsSystem = soundsSystem;
            _savingService = savingService;
            _staticDataService = staticDataService;
        }

        public void SetMusicVolume(float volume)
        {
            SettingsData.SetMusicVolume(volume);
            _soundsSystem.SetMusicVolume(volume);
        }

        public void SetSFXVolume(float volume)
        {
            SettingsData.SetSFXVolume(volume);
            _soundsSystem.SetSFXVolume(volume);
        }

        public void SetMasterVolume(float volume)
        {
            SettingsData.SetMasterVolume(volume);
            _soundsSystem.SetMasterVolume(volume);
        }

        public void SetMouseSensitivity(float sensitivity)
        {
            SettingsData.SetMouseSensitivity(sensitivity);
        }

        public void LoadSettings()
        {
            SettingsData = _savingService.Load<SettingsData>(SettingsKey);
            _soundsSystem.SetMusicVolume(SettingsData.MusicVolume);
            _soundsSystem.SetSFXVolume(SettingsData.SFXVolume);
            _soundsSystem.SetMasterVolume(SettingsData.MasterVolume);

        }

        public bool HasSettingsData()
        {
            return _savingService.HasKey(SettingsKey);
        }

        public void SaveSettings()
        {
            _savingService.Save(SettingsKey, SettingsData);
        }

        public void CreateDefaultSettings()
        {
            SettingsData = new SettingsData(SettingsConfig);
            SaveSettings();
        }
    }
}