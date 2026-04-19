using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Level.LevelObjects;
using Project.Scripts.GamePlay.Player.PlayerResources;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Ships
{
    public class ShipPlankInteractable : BaseInteractable
    {
        [SerializeField] private LootPlanarObstacleMovement _movement;

        public void BeginDriftingAt(Vector3 worldPosition)
        {
            if (_movement != null)
                _movement.BeginAt(worldPosition);
        }

        public override void Interact(BaseEntity entity)
        {
            if (entity is ActorEntity actor &&
                actor.TryGet(out WoodResourceController wood))
            {
                wood.AddOne();
            }

            base.Interact(entity);
        }
    }
}
