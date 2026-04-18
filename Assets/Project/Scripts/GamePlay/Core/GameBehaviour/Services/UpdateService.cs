namespace Project.Scripts.GamePlay.Core.GameBehaviour.Services
{
    public class UpdateService : IUpdateService
    {
        // Player collections
        public GameBehaviorCollection<IGameUpdateable> PlayerUpdate { get; } = new GameBehaviorCollection<IGameUpdateable>();
        public GameBehaviorCollection<IGameFixedUpdateable> PlayerFixedUpdate { get; } = new GameBehaviorCollection<IGameFixedUpdateable>();

        // Enemies collections
        public GameBehaviorCollection<IGameUpdateable> EnemiesUpdate { get; } = new GameBehaviorCollection<IGameUpdateable>();

        public GameBehaviorCollection<IGameFixedUpdateable> EnemiesFixedUpdate { get; } = new GameBehaviorCollection<IGameFixedUpdateable>();

        // Projectiles collections
        public GameBehaviorCollection<IGameUpdateable> ProjectilesUpdate { get; } = new GameBehaviorCollection<IGameUpdateable>();
        public GameBehaviorCollection<IGameFixedUpdateable> ProjectilesFixedUpdate { get; } = new GameBehaviorCollection<IGameFixedUpdateable>();

        // Effects collections
        public GameBehaviorCollection<IGameUpdateable> EffectsUpdate { get; } = new GameBehaviorCollection<IGameUpdateable>();
        public GameBehaviorCollection<IGameFixedUpdateable> EffectsFixedUpdate { get; } = new GameBehaviorCollection<IGameFixedUpdateable>();
        public GameBehaviorCollection<IGameLateUpdateable> LateUpdate { get; } = new GameBehaviorCollection<IGameLateUpdateable>();
        public GameBehaviorCollection<IPausable> Pausable { get; }= new GameBehaviorCollection<IPausable>();

        // Execution methods
        public void UpdateAll(float deltaTime)
        {       
            PlayerUpdate.ExecuteAll((updateable, dt) => updateable.GameUpdate(dt), deltaTime);
            EnemiesUpdate.ExecuteAll((updateable, dt) => updateable.GameUpdate(dt), deltaTime);
            ProjectilesUpdate.ExecuteAll((updateable, dt) => updateable.GameUpdate(dt), deltaTime);
            EffectsUpdate.ExecuteAll((updateable, dt) => updateable.GameUpdate(dt), deltaTime);
        }

        public void FixedUpdateAll(float fixedDeltaTime)
        {
            PlayerFixedUpdate.ExecuteAll((fixedUpdateable, dt) => fixedUpdateable.GameFixedUpdate(dt), fixedDeltaTime);
            EnemiesFixedUpdate.ExecuteAll((fixedUpdateable, dt) => fixedUpdateable.GameFixedUpdate(dt), fixedDeltaTime);
            ProjectilesFixedUpdate.ExecuteAll((fixedUpdateable, dt) => fixedUpdateable.GameFixedUpdate(dt), fixedDeltaTime);
            EffectsFixedUpdate.ExecuteAll((fixedUpdateable, dt) => fixedUpdateable.GameFixedUpdate(dt), fixedDeltaTime);
        }

        public void LateUpdateAll(float timeServiceDeltaTime)
        {
            LateUpdate.ExecuteAll((lateUpdateable, dt) => lateUpdateable.GameLateUpdate(dt), timeServiceDeltaTime);
        }

        public void PauseAll()
        {
            Pausable.ExecuteAll(pausable => pausable.Pause());
        }

        public void ResumeAll()
        {
            Pausable.ExecuteAll(pausable => pausable.Resume());
        }
    }
}