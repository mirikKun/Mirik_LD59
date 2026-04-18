using Project.Scripts.GamePlay.Common.Health;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Health
{
    public class PlayerHealth:BaseHealth
    {
        public override void TakeDamage(float damage, BaseEntity attacker)
        {
            base.TakeDamage( damage, attacker);
            Debug.Log($"Player took {damage} damage, current health: {Current}");
        }

     
    }
}