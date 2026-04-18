using System;
using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Common.Health
{
    public interface IHealth
    {
        event Action<float,float,BaseEntity> HealthChanged;
        float Current { get;}
        float Max { get; }
        bool IsDead { get; set; }
        void TakeDamage(float damage, BaseEntity attacker);
        void Heal(float healAmount, BaseEntity healer);
        void Reset();
        
        void SetInvincibility(bool isInvincible);
        event Action<BaseEntity> Died;
        event Action<BaseEntity,BaseEntity> Killed;
    }
} 