namespace Project.Scripts.GamePlay.Core.GameBehaviour.Services
{
    public interface IUpdateService
    {
        GameBehaviorCollection<IGameUpdateable> PlayerUpdate { get; }
        GameBehaviorCollection<IGameFixedUpdateable> PlayerFixedUpdate { get; }

        GameBehaviorCollection<IGameUpdateable> EnemiesUpdate { get; }
        GameBehaviorCollection<IGameFixedUpdateable> EnemiesFixedUpdate { get; }

        GameBehaviorCollection<IGameUpdateable> ProjectilesUpdate { get; }
        GameBehaviorCollection<IGameFixedUpdateable> ProjectilesFixedUpdate { get; }

        GameBehaviorCollection<IGameUpdateable> EffectsUpdate { get; }
        GameBehaviorCollection<IGameFixedUpdateable> EffectsFixedUpdate { get; }
        GameBehaviorCollection<IGameLateUpdateable> LateUpdate{ get; }
        
        GameBehaviorCollection<IPausable> Pausable { get; }

        // Execution
        void UpdateAll(float deltaTime);
        void FixedUpdateAll(float fixedDeltaTime);
        void LateUpdateAll(float timeServiceDeltaTime);
        
        void PauseAll();
        void ResumeAll();
    }
}