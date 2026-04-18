using System;
using Project.Scripts.GamePlay.Levels.Enum;
using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.GamePlay.Level.Systems
{
    public static class PlayerSpawnHelpers
    {
        public static void TeleportPlayer(PlayerEntity player, Vector3 position)
        {
            PlayerController controller = player.Get<PlayerController>();
            PlayerMover mover = player.Get<PlayerMover>();
            controller.SetRespawnPosition(position);
            mover.Teleport(position);
            mover.SetVelocity(Vector3.zero);
            mover.SetMomentum(Vector3.zero);
            mover.SetRbVelocity(Vector3.zero, 1f);
        }

        public static bool IsSameSceneAsActive(Scenes target)
        {
            if (target == Scenes.Unknown)
                return true;
            return string.Equals(SceneManager.GetActiveScene().name, target.ToString(), StringComparison.Ordinal);
        }
    }
}
