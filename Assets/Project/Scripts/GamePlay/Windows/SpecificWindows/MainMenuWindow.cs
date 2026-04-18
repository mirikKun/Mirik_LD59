using Project.Scripts.GamePlay.Levels.Enum;
using Project.Scripts.Infrastructure.Sounds;
using Project.Scripts.Infrastructure.Sounds.Enum;
using Project.Scripts.Infrastructure.States.GameStates;
using Project.Scripts.Infrastructure.States.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GamePlay.Windows.SpecificWindows
{
    public class MainMenuWindow:BaseWindow
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        private IGameStateMachine _stateMachine;
        private IWindowService _windowService;
        private ISoundsSystem _soundsSystem;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine,IWindowService windowService,ISoundsSystem soundsSystem)
        {
            _soundsSystem = soundsSystem;
            _windowService = windowService;
            _stateMachine = gameStateMachine;
        }
        protected override void Initialize()
        {
            base.Initialize();
            _playButton.onClick.AddListener(LoadGameplayScene);
            _settingsButton.onClick.AddListener(OpenSettingsWindow);
            _exitButton.onClick.AddListener(Exit);
#if UNITY_WEBGL
            _exitButton.gameObject.SetActive(false);
#endif
            _soundsSystem.Play(DefaultSounds.WindowOpen);

        }

        private void LoadGameplayScene()
        {
            PlayButtonClickSound();
            _stateMachine.Enter<LoadingGameplayState,Scenes>(Scenes.Gameplay);
        }

        private void OpenSettingsWindow()
        {
            PlayButtonClickSound();
            _windowService.Open(WindowId.Settings);
        }

        private void PlayButtonClickSound()
        {
            _soundsSystem.Play(DefaultSounds.ButtonClick);
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}