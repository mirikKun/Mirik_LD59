using System;
using System.Collections;
using Project.Scripts.GamePlay.Common.Time;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    public class CastingEffects : MonoBehaviour
    {
        [Header("Particles")] 
        [SerializeField] private ParticleSystem _chantingParticles;
        [SerializeField] private ParticleSystem _chargingParticles;
        [SerializeField] private Transform _particlesTarget;
        [SerializeField] private float _minParticlesDuration = 0.1f;

        [Header("Magic Circles")] [SerializeField]
        private MagicCircle[] _magicCircles;

        [SerializeField] private float _circleAppearDuration = 0.5f;

        [SerializeField] private float _circleDisappearDuration = 0.5f;
        private ITimeService _timeService;

        //private bool _active;
        [Inject]
        private void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }
        private void Start()
        {
            foreach (var magicCircle in _magicCircles)
            {
                magicCircle.Material.color = new Color(magicCircle.Material.color.r, magicCircle.Material.color.g,
                    magicCircle.Material.color.b, 0);
            }
        }

        public void Tick(float deltaTime)
        {
            // var shape = _chantingParticles.shape;
            // shape.position = _chantingParticles.transform.InverseTransformPoint(_particlesTarget.position);

            foreach (var magicCircle in _magicCircles)
            {
                magicCircle.CircleTransform.Rotate(Vector3.forward, magicCircle.RotationSpeed * deltaTime);
            }
        }

        public void PlayChantingEffect()
        {
            var shape = _chantingParticles.shape;
            shape.position = _chantingParticles.transform.InverseTransformPoint(_particlesTarget.position);
            
            
            
            _chantingParticles.Play();
        }

        public void PlayAppearEffect()
        {
            //_active=true;
            StartCoroutine(FadeMagicCircles(0f, 1f, _circleAppearDuration));
        }

        public void PlayDisappearEffect()
        {
            StartCoroutine(FadeMagicCircles(1f, 0f, _circleDisappearDuration));
        }

        public void StartChargingEffect()
        {
            _chargingParticles.Play();
        }
        public void StopChargingEffect()
        {
            _chargingParticles.Stop();
        }

        private IEnumerator FadeMagicCircles(float alphaFrom, float alphaTo, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += _timeService.DeltaTime;
                foreach (var magicCircle in _magicCircles)
                {
                    magicCircle.Material.color = new Color(magicCircle.Material.color.r, magicCircle.Material.color.g,
                        magicCircle.Material.color.b, Mathf.Lerp(alphaFrom, alphaTo, elapsedTime / duration));
                }

                yield return null;
            }
        }
    }

    [Serializable]
    public class MagicCircle
    {
        public Transform CircleTransform;
        public Material Material;
        public float RotationSpeed;
    }
}