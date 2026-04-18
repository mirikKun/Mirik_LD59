using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.GamePlay.Level.Configs;
using Project.Scripts.GamePlay.Windows;
using Project.Scripts.GamePlay.Windows.Configs;
using Project.Scripts.Infrastructure.Settings.Configs;
using Project.Scripts.Infrastructure.Sounds.ScriptableObjects;
using UnityEngine;

namespace Project.Scripts.Infrastructure.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private SettingsConfig _settingsConfig;
        private SoundMixersSO _soundMixersSo;
        private InteractablesConfig _interactablesConfig;
        private DefaultSoundsConfig _defaultSoundsConfig;

        private Dictionary<WindowId, GameObject> _windowPrefabsById;

        public void LoadAll()
        {
            LoadWindows();
            LoadSettingsConfig();
            LoadInteractablesConfig();
            LoadMixersData();
            LoadDefaultSoundsConfig();

        }

        private void LoadMixersData()
        {
            _soundMixersSo  = Resources
                .Load<SoundMixersSO>("Configs/Sound/SoundMixersConfig");
            
        }
        private void LoadInteractablesConfig()
        {
            _interactablesConfig  = Resources
                .Load<InteractablesConfig>("Configs/InteractablesConfig");
            
        }

        private void LoadDefaultSoundsConfig()
        {
            _defaultSoundsConfig = Resources
                .Load<DefaultSoundsConfig>("Configs/Sound/DefaultSoundsConfig");
        }


        public GameObject GetWindowPrefab(WindowId id) =>
            _windowPrefabsById.TryGetValue(id, out GameObject prefab)
                ? prefab
                : throw new Exception($"Prefab config for window {id} was not found");

        private void LoadWindows()
        {
            _windowPrefabsById = Resources
                .Load<WindowsConfig>("Configs/WindowConfig")
                .WindowConfigs
                .ToDictionary(x => x.Id, x => x.Prefab);
        }
        
        public SettingsConfig GetSettingsConfig() => 
            _settingsConfig ?? throw new Exception("Settings config was not loaded");
        public SoundMixersSO GetSoundMixersSo()=>_soundMixersSo;
        public DefaultSoundsConfig GetDefaultSoundsConfig() => _defaultSoundsConfig;
        public InteractablesConfig GetInteractablesConfig() => _interactablesConfig;

        private void LoadSettingsConfig()
        {
            _settingsConfig  = Resources
                .Load<SettingsConfig>("Configs/SettingsConfig");
        }

    }
}