using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{
    public abstract class EntityResourceController:EntityComponent
    {
        public abstract void Tick(float deltaTime);
    }
}