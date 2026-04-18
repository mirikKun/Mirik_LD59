using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class SaveZone:MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity entity))
            {
                entity.Get<PlayerController>().SetRespawnPosition(_respawnPoint.position);
            }
        }
    }
}