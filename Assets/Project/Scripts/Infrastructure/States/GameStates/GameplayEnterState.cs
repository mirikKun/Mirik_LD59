using Project.Scripts.GamePlay.Levels;
using Project.Scripts.Infrastructure.States.StateInfrastructure;
using Project.Scripts.Infrastructure.States.StateMachine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class GameplayEnterState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ILevelDataProvider _levelDataProvider;


        public GameplayEnterState(
            IGameStateMachine stateMachine,
            ILevelDataProvider levelDataProvider)
        {
            _stateMachine = stateMachine;
            _levelDataProvider = levelDataProvider;
        }

        public void Enter()
        {
            PlacePlayer();

            _stateMachine.Enter<GameLoopState>();
        }

        private void PlacePlayer()
        {
        }

        public void Exit()
        {
        }
    }
}