using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    /// <summary>
    /// Marker on the lighthouse beam / destroyer volume. <see cref="LevelGenerator.Cliff"/> reacts via trigger overlap.
    /// </summary>
    public class ClifDestroyer : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _player;
    }
}