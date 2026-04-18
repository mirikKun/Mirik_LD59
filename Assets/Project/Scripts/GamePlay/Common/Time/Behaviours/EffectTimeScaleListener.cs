using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace Project.Scripts.GamePlay.Common.Time.Behaviours
{
    public class EffectTimeScaleListener : MonoBehaviour, IGameUpdateable
    {
        private static readonly int SimulatedTime = Shader.PropertyToID("SimulatedTime");
        [SerializeField] private ParticleSystem[] _particle;
        [SerializeField] private VisualEffect[] _visualEffects;
        [SerializeField] private MeshRenderer[] _meshRenderers;

        private float _simulatedTime;
        private ITimeService _timeService;
        private IUpdateService _updateService;

        [Inject]
        private void Construct(ITimeService timeService, IUpdateService updateService)
        {
            _updateService = updateService;
            _timeService = timeService;
        }

        private void Start()
        {
            _timeService.OnTimeScaleChanged += AdjustEffectsTimeScale;
        }

        private void OnDestroy()
        {
            _timeService.OnTimeScaleChanged -= AdjustEffectsTimeScale;
        }

        private void OnEnable()
        {
            _updateService.EffectsUpdate.Register(this);
        }

        private void OnDisable()
        {
            _updateService.EffectsUpdate.Unregister(this);
        }

        private void AdjustEffectsTimeScale(float timeScale)
        {
            foreach (var particle in _particle)
            {
                var main = particle.main;
                main.simulationSpeed = timeScale;
            }
            foreach (var visualEffect in _visualEffects)
            {
                visualEffect.playRate = timeScale;
                visualEffect.pause=timeScale<= float.Epsilon;;
            }
        }

        public void GameUpdate(float deltaTime)
        {
            _simulatedTime += deltaTime;
            foreach (var visualEffect in _visualEffects)
            {
                visualEffect.SetFloat(SimulatedTime, _simulatedTime);
            }

            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.SetFloat(SimulatedTime, _simulatedTime);
            }
        }
    }
}