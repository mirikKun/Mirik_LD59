using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.Entity.Effects;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Physic.ColliderLogic
{
    public interface IAttackTrigger
    {
        public void Init(BaseEntity casterEntity);
        public List<Effect> Effects { get; }
        BaseEntity CasterEntity { get; }
        List<ITriggerHittable> HitObjects { get; }
        public void Reset();
        public Transform Transform { get; }
        public void AddHitProtected(ITriggerHittable hittable);
        event Action<ITriggerHittable> Hitted;
    }
}