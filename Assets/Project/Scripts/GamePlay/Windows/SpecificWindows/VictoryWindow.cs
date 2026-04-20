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
    public class VictoryWindow : BaseWindow
    {
        [SerializeField] private Button _restartButton;
        private IGameStateMachine _stateMachine;
        private ISoundsSystem _soundsSystem;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, ISoundsSystem soundsSystem)
        {
            _stateMachine = gameStateMachine;
            _soundsSystem = soundsSystem;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _restartButton.onClick.AddListener(RestartGameplay);
            _soundsSystem.Play(DefaultSounds.WindowOpen);
        }

        private void RestartGameplay()
        {
            _soundsSystem.Play(DefaultSounds.ButtonClick);
            _stateMachine.Enter<LoadingHomeScreenState>();
        }
    }
}
