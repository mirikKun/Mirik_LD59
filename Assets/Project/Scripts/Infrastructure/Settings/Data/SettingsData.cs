using System;
using Project.Scripts.Infrastructure.Saving;
using Project.Scripts.Infrastructure.Settings.Configs;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Settings.Data
{
    [Serializable]
    public class SettingsData : BaseSaveModel<SettingsData>
    {
        [field: SerializeField] public float MusicVolume { get; private set; } = 1f;
        [field: SerializeField] public float SFXVolume { get; private set; } = 1f;
        [field: SerializeField] public float MasterVolume { get; private set; } = 1f;
        [field: SerializeField] public float MouseSensitivity { get; private set; } = 0.5f;


        public SettingsData()
        {
            MusicVolume = 1f;
            SFXVolume = 1f;
            MasterVolume = 1f;
            MouseSensitivity = 1f;
        }

        public SettingsData(SettingsConfig config)
        {
            MusicVolume = config.DefaultMusicVolume;
            SFXVolume = config.DefaultSFXVolume;
            MasterVolume = config.DefaultMasterVolume;
            MouseSensitivity = config.DefaultMouseSensitivity;
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            OnDataChanged();
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            OnDataChanged();
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume = volume;

            OnDataChanged();
        }

        public void SetMouseSensitivity(float volume)
        {
            MouseSensitivity = volume;
            OnDataChanged();
        }
    }
}