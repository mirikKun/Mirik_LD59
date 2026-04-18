using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.Entity.Effects;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerEffects;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Physic.ColliderLogic
{
    public class SimpleAttackTrigger:MonoBehaviour,IAttackTrigger
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        private List<ITriggerHittable> _hitObjects=new List<ITriggerHittable>();
        private List<ITriggerHittable> _hitProtectedObjects=new List<ITriggerHittable>();
        private BaseEntity _casterEntity;
        private Coroutine _hitCoroutine;
        public BaseEntity CasterEntity=> _casterEntity;
        public List<ITriggerHittable> HitObjects => _hitObjects;
        public List<Effect> Effects { get; private set; }
        public Transform Transform => transform;
        public event Action<ITriggerHittable> Hitted;


        public void Init(BaseEntity casterEntity)
        {
            _casterEntity= casterEntity;
        }

        public void SetEffects(List<Effect> effects)
        {
            Effects = effects;
        }

        public void Reset()
        {
            _hitObjects.Clear();
            _hitProtectedObjects.Clear();
            _hitCoroutine= null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out ITriggerHittable hittable)&&!_hitObjects.Contains(hittable) &&!_hitProtectedObjects.Contains(hittable))
            {
                OnHit(hittable);
                //Debug.Break();
                // if(_hitCoroutine==null)
                // {
                //     _hitCoroutine = StartCoroutine(HandleHit());
                // }
            }
        }

        private void OnHit(ITriggerHittable hittable)
        {
            _hitObjects.Add(hittable);

            hittable.OnHit(this);
            
            foreach (var particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
            if (CasterEntity is PlayerEntity)
            {
                CasterEntity.Get<PlayerEffects>().TimeSlowEffect.PlayTimeStop();
            }
            Hitted?.Invoke(hittable);
        }

        public void AddHitProtected(ITriggerHittable hittable)
        {
            _hitProtectedObjects.Add(hittable);
        }

        private IEnumerator HandleHit()
        {
            yield return null;

            foreach (var hitObject in _hitObjects)
            {
                // if(hitObject is)
                // hitObject.OnHit(this);
            }
        }
    }
}