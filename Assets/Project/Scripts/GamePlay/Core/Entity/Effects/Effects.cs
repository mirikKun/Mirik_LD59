using System;
using Project.Scripts.GamePlay.Common.Health;
using Project.Scripts.GamePlay.Common.Movement;
using Project.Scripts.GamePlay.Core.Physic.Enums;

using Project.Scripts.Utils.ActionList;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Entity.Effects
{
    [Serializable]
    public abstract class Effect : IActionElement
    {
        public event Action<Effect> OnCompleted;

        public abstract void Execute(BaseEntity caster, BaseEntity target, Transform from );

        public virtual void Cancel() => RaiseCompleted();

        protected void RaiseCompleted() => OnCompleted?.Invoke(this);
    }

    [Serializable]
    public class DamageEffect : Effect
    {
        [SerializeField] private float _amount;

        public override void Execute(BaseEntity caster, BaseEntity target, Transform from )
        {
            if (caster == target)
            {
                RaiseCompleted();
                return;
            }
    
            if (target&&target.TryGet(out IHealth health))
            {
                health.TakeDamage(_amount, caster);
            }

            RaiseCompleted();
        }
        
    }

    
    
    [Serializable]
    public class KnockbackEffect : Effect
    {
        [SerializeField] private float _force;


        public override void Execute(BaseEntity caster, BaseEntity target, Transform from )
        {
            //Debug.Log($"{caster?.name} knocked back {target.name} with force {_force}");
            Vector3 dir = (target.transform.position - from.position).normalized;

            if (target.TryGet<IBaseMover>(out var mover))
            {
                if (!mover.IsFlying)
                {
                    dir.y = Mathf.Abs(dir.y);
                }

                mover.ApplyForce(dir * _force);
            }

            RaiseCompleted();
        }
    }

    [Serializable]
    public class StraightKnockbackEffect : Effect
    {
        [SerializeField] private float _force;
        [SerializeField] private CastDirection _castDirection;

        public override void Execute(BaseEntity caster, BaseEntity target, Transform from )
        {
            //Debug.Log($"{caster.name} knocked back {target.name} with force {_force}");
            Vector3 dir = GetCastDirection(from);
         

            if (target.TryGet<IBaseMover>(out var mover))
            {
                mover.ApplyForce(dir * _force);
            }

            RaiseCompleted();
        }

        private Vector3 GetCastDirection(Transform tr)
        {
            return _castDirection switch
            {
                CastDirection.Forward => tr.forward,
                CastDirection.Right => tr.right,
                CastDirection.Up => tr.up,
                CastDirection.Backward => -tr.forward,
                CastDirection.Left => -tr.right,
                CastDirection.Down => -tr.up,
                _ => Vector3.one
            };
        }
    }



}