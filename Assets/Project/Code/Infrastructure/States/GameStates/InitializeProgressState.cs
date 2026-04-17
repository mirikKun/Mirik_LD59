using Code.Infrastructure.Progress.Data;
using Code.Infrastructure.Progress.Provider;
using Code.Infrastructure.Settings;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;

namespace Code.Infrastructure.States.GameStates
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