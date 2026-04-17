using Code.Gameplay.Windows;
using Code.Infrastructure.Settings.Configs;
using Code.Infrastructure.Sounds.ScriptableObjects;
using UnityEngine;

namespace Code.Infrastructure.StaticData
{
    public interface IStaticDataService
    {
        void LoadAll();
        GameObject GetWindowPrefab(WindowId id);
        SettingsConfig GetSettingsConfig();
        SoundMixersSO GetSoundMixersSO();
        DefaultSoundsConfig GetDefaultSoundsConfig();
    }
}