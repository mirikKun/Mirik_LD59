using Project.Scripts.GamePlay.Levels.Enum;
using Project.Scripts.Infrastructure.Loading;
using Project.Scripts.Infrastructure.States.StateInfrastructure;
using Project.Scripts.Infrastructure.States.StateMachine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class LoadingGameplayState : IPayloadState<Scenes>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public LoadingGameplayState(IGameStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(Scenes scene)
        {
            _sceneLoader.LoadScene(scene.ToString(),EnterBattleLoopState);
        }

        private void EnterBattleLoopState()
        {
            _stateMachine.Enter<GameplayEnterState>();
        }

        public void Exit()
        {
        }
    }
}