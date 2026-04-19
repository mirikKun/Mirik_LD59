using System;
using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class DeathZone:MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity player))
            {
                player.Get<PlayerController>().RespawnPlayer(player);
            }
        }
    }
}