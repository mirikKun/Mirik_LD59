using Project.Scripts.GamePlay.Level.LevelObjects;
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
    }
}
