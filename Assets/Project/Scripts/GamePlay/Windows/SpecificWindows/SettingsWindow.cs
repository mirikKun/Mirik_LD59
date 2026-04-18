using Project.Scripts.Infrastructure.Settings;
using Project.Scripts.Infrastructure.Sounds;
using Project.Scripts.Infrastructure.Sounds.Enum;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GamePlay.Windows.SpecificWindows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _sfxVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _mouseSensitivity;

        [SerializeField] private Button _closeButton;
        private ISettingsService _settingsService;
        private IWindowService _windowService;
        private ISoundsSystem _soundsSystem;

        [Inject]
        private void Construct(ISettingsService settingsService, IWindowService windowService,
            ISoundsSystem soundsSystem)
        {
            _soundsSystem = soundsSystem;
            _windowService = windowService;
            _settingsService = settingsService;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            
            _masterVolume.onValueChanged.AddListener(_settingsService.SetMasterVolume);
            _sfxVolume.onValueChanged.AddListener(_settingsService.SetSFXVolume);
            _musicVolume.onValueChanged.AddListener(_settingsService.SetMusicVolume);
            _mouseSensitivity.onValueChanged.AddListener(_settingsService.SetMouseSensitivity);

            _masterVolume.onValueChanged.AddListener((_) => PlayButtonClickSound());
            _sfxVolume.onValueChanged.AddListener((_) => PlayButtonClickSound());
            _musicVolume.onValueChanged.AddListener((_) => PlayButtonClickSound());
            _mouseSensitivity.onValueChanged.AddListener((_) => PlayButtonClickSound());

            _closeButton.onClick.AddListener(CloseWindow);

            //_settingsService.LoadSettings();

            _masterVolume.SetValueWithoutNotify(_settingsService.SettingsData.MasterVolume);
            _sfxVolume.SetValueWithoutNotify(_settingsService.SettingsData.SFXVolume);
            _musicVolume.SetValueWithoutNotify(_settingsService.SettingsData.MusicVolume);

            _mouseSensitivity.minValue = _settingsService.SettingsConfig.MinMouseSensitivity;
            _mouseSensitivity.maxValue = _settingsService.SettingsConfig.MaxMouseSensitivity;
            _mouseSensitivity.SetValueWithoutNotify( _settingsService.SettingsData.MouseSensitivity);


            //_soundsSystem.Play(DefaultSounds.WindowOpen);
        }

        private void PlayButtonClickSound()
        {
            if(UnityEngine.Input.GetMouseButtonDown(0))
            _soundsSystem.Play(DefaultSounds.ButtonClick);
        }

        private void CloseWindow()
        {
            PlayButtonClickSound();
            _windowService.Close(WindowId.Settings);
            _settingsService.SaveSettings();
        }
    }
}