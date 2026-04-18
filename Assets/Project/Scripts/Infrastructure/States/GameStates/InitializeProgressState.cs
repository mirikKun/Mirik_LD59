using Project.Scripts.Infrastructure.Progress.Provider;
using Project.Scripts.Infrastructure.Settings;
using Project.Scripts.Infrastructure.States.StateInfrastructure;
using Project.Scripts.Infrastructure.States.StateMachine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class InitializeProgressState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IProgressProvider _progressProvider;
        private ISettingsService _settingsService;

        public InitializeProgressState(
            IGameStateMachine stateMachine,
            IProgressProvider progressProvider,ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _stateMachine = stateMachine;
            _progressProvider = progressProvider;
        }

        public void Enter()
        {
            InitializeProgress();

            _stateMachine.Enter<LoadingHomeScreenState>();
        }
        
        private void InitializeProgress()
        {
            if (_progressProvider.HasProgress())
            {
                _progressProvider.LoadProgress();
            }
            else
            {
                _progressProvider.CreateDefaultProgress();
            }

            if (_settingsService.HasSettingsData())
            {
                _settingsService.LoadSettings();
            }
            else
            {
                _settingsService.CreateDefaultSettings();
            }
        }
        public void Exit()
        {
        }
    }
}