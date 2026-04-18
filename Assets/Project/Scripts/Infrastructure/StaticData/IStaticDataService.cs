using Project.Scripts.GamePlay.Level.Configs;
using Project.Scripts.GamePlay.Windows;
using Project.Scripts.Infrastructure.Settings.Configs;
using Project.Scripts.Infrastructure.Sounds.ScriptableObjects;
using UnityEngine;

namespace Project.Scripts.Infrastructure.StaticData
{
    public interface IStaticDataService
    {
        void LoadAll();
        GameObject GetWindowPrefab(WindowId id);
        SettingsConfig GetSettingsConfig();
        SoundMixersSO GetSoundMixersSo();
        DefaultSoundsConfig GetDefaultSoundsConfig();
        InteractablesConfig GetInteractablesConfig();
    }
}