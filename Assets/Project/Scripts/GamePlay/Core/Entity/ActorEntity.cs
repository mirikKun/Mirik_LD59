using System.Collections.Generic;
using Project.Scripts.GamePlay.Common.Health;
using Project.Scripts.GamePlay.Core.Entity.Effects;
using Project.Scripts.GamePlay.Core.Physic.ColliderLogic;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Entity
{
    public class ActorEntity:BaseEntity,ITriggerHittable
    {
        private Transform _transform;
        private readonly List<Effect> _activeEffects = new();

        protected override void Awake()
        {
            _transform = transform;

            base.Awake();

            if (TryGet(out IHealth health))
                health.Died += OnHealthDied;
        }

        private void OnDestroy()
        {
            if (TryGet(out IHealth health))
                health.Died -= OnHealthDied;

            CancelAllActiveEffects();
        }

        protected override void InitComponentsRegistry()
        {
            Components= new ComponentsRegistry(_componentsList,this);
        }
        public Vector3 GetPosition()
        {
            return _transform.position;
        }
        public Transform GetTransform()
        {
            return _transform;
        }

        public virtual void OnHit(IAttackTrigger attackTrigger)
        {
            foreach (var effect in attackTrigger.Effects)
            {
                ApplyEffect(effect, attackTrigger.CasterEntity, attackTrigger.Transform);
            }
        }

        public void ApplyEffect(Effect effect, BaseEntity caster, Transform from)
        {
            if (TryGet(out IHealth health) && health.IsDead)
                return;

            effect.OnCompleted += RemoveEffect;
            _activeEffects.Add(effect);
            effect.Execute(caster, this, from);
        }

        private void RemoveEffect(Effect effect)
        {
            effect.OnCompleted -= RemoveEffect;
            _activeEffects.Remove(effect);
        }

        private void OnHealthDied(BaseEntity _)
        {
            CancelAllActiveEffects();
        }

        private void CancelAllActiveEffects()
        {
            for (var i = _activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = _activeEffects[i];
                effect.OnCompleted -= RemoveEffect;
                effect.Cancel();
            }

            _activeEffects.Clear();
        }
    }
}