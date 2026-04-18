using UnityEngine;

namespace Project.Scripts.Infrastructure.Settings.Configs
{
    [CreateAssetMenu(fileName = "SettingsConfig", menuName = "Configs/Settings/SettingsConfig")]
    public class SettingsConfig : ScriptableObject
    {
        [Header("Volume Settings")]
        [SerializeField] [Range(0f, 1f)] private float _defaultMusicVolume = 1f;
        [SerializeField] [Range(0f, 1f)] private float _defaultSFXVolume = 1f;
        [SerializeField] [Range(0f, 1f)] private float _defaultMasterVolume = 1f;
        
        [Header("Mouse Sensitivity Settings")]
        [SerializeField] [Range(0.1f, 2f)] private float _defaultMouseSensitivity = 0.5f;
        [SerializeField] [Range(0.1f, 2f)] private float _minMouseSensitivity = 0.1f;
        [SerializeField] [Range(0.1f, 2f)] private float _maxMouseSensitivity = 2f;

        public float DefaultMusicVolume => _defaultMusicVolume;
        public float DefaultSFXVolume => _defaultSFXVolume;
        public float DefaultMasterVolume => _defaultMasterVolume;
        public float DefaultMouseSensitivity => _defaultMouseSensitivity;
        public float MinMouseSensitivity => _minMouseSensitivity;
        public float MaxMouseSensitivity => _maxMouseSensitivity;
    }
}










