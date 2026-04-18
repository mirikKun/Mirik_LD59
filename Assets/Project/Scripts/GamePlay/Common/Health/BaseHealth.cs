using System;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Common.Health
{
    public abstract class BaseHealth:EntityComponent,IHealth
    {
        [SerializeField] private float _health;
        private bool _isInvincible;
        
        public event Action<float,float,BaseEntity> HealthChanged;
        public event Action<BaseEntity> Died;
        public event Action<BaseEntity, BaseEntity> Killed;
        public float Current { get; private set; } = 1;
        public float Max { get; private set; } = 1;
        public bool IsDead { get; set; }

        public override void StartEntity ()
        {
            Current = _health;
            Max = _health;
            HealthChanged?.Invoke(Current/Max,0,null);

        }
        
        public virtual void TakeDamage(float damage, BaseEntity attacker)
        {
            if(_isInvincible||IsDead)
                return;

            damage= Mathf.Clamp(damage, 0, Current);
            Current -= damage;
            Current = Mathf.Clamp(Current, 0, Current);
            if (Current <= 0)
            {
                IsDead = true;
                Died?.Invoke(Entity);
                Killed?.Invoke(Entity,attacker);
            }
            HealthChanged?.Invoke(Current/Max,-damage,attacker);
            
        }

        public void Heal(float healAmount, BaseEntity healer)
        {
            if(IsDead)
                return;
            
            healAmount= Mathf.Clamp(healAmount, 0, Max-Current);
            Current += healAmount;
            Current = Mathf.Clamp(Current, Current, Max);
 
            HealthChanged?.Invoke(Current/Max,healAmount,healer);
        }


        public void Reset()
        {
            Current = Max;
            IsDead = false;
            HealthChanged?.Invoke(Current/Max,0,null);

        }

        public void SetInvincibility(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
    }
}