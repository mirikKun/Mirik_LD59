using Project.Scripts.Infrastructure.Settings.Configs;
using Project.Scripts.Infrastructure.Settings.Data;

namespace Project.Scripts.Infrastructure.Settings
{
    public interface ISettingsService
    {
        SettingsData SettingsData { get; }
        SettingsConfig SettingsConfig { get; }

        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);
        void SetMasterVolume(float volume);
        void SetMouseSensitivity(float sensitivity);
        
        void LoadSettings();
        void SaveSettings();
        void CreateDefaultSettings();
        bool HasSettingsData();
    }
}

