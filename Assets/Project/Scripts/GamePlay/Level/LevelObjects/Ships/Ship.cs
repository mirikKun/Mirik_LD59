using System;
using Project.Scripts.Common.Extensions;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Level.Factories;
using Project.Scripts.GamePlay.Level.LevelGenerator;
using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Ships
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ship : MonoBehaviour, IGameUpdateable
    {
        [SerializeField] private float _minSpeed = 2f;
        [SerializeField] private float _maxSpeed = 4f;
        [SerializeField] private float _arrivalThreshold = 0.5f;

        [Header("Waves (curve time 0–1, value = degrees or meters)")]
        [SerializeField] private float _waveCyclesPerSecond = 0.12f;
        [SerializeField] private AnimationCurve _pitchWave;
        [SerializeField] private AnimationCurve _yawWave;
        [SerializeField] private AnimationCurve _rollWave;
        [SerializeField] private AnimationCurve _heightWave;

        [Header("Sink")]
        [SerializeField] private ParticleSystem _sinkParticles;
        [SerializeField] private float _sinkDepth = 15f;
        [SerializeField] private float _sinkDuration = 4f;
        [SerializeField] private AnimationCurve _sinkProgress;
        [SerializeField] private AnimationCurve _sinkTiltPitch;
        [SerializeField] private AnimationCurve _sinkTiltYaw;
        [SerializeField] private AnimationCurve _sinkTiltRoll;
        
        
        [SerializeField] private float _lootSpawnHeight;

        [Header("Model")]
        [SerializeField] private Vector3 _modelRotationOffset = new Vector3(0f, 180f, 0f);

        private IUpdateService _updateService;
        private IInteractablesFactory _interactablesFactory;
        private Vector3 _basePosition;
        private Vector3 _moveDelta;
        private Vector3 _routeEnd;
        private float _speed;
        private float _wavePhase;
        private bool _sinking;

        private float _sinkElapsed;
        private Vector3 _sinkStartPosition;
        private Vector3 _sinkEndPosition;
        private Quaternion _sinkStartRotation;
        private PlayerEntity _player;

        [Inject]
        private void Construct(IUpdateService updateService, IInteractablesFactory interactablesFactory)
        {
            _updateService = updateService;
            _interactablesFactory = interactablesFactory;
        }

[ContextMenu("Reset")]
        private void Reset()
        {
            ApplyDefaultAnimationCurves(onlyIfEmpty: false);
        }

        private void Awake()
        {
            ApplyDefaultAnimationCurves(onlyIfEmpty: true);

            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        private void ApplyDefaultAnimationCurves(bool onlyIfEmpty)
        {
            void Assign(ref AnimationCurve field, AnimationCurve value)
            {
                if (!onlyIfEmpty || IsCurveEmpty(field))
                    field = value;
            }

            Assign(ref _pitchWave, CreateSeamlessLoop(0f, 4f, 0f, -4f, 0f));
            Assign(ref _yawWave, CreateSeamlessLoop(0f, -3f, 0f, 3f, 0f));
            Assign(ref _rollWave, CreateSeamlessLoop(0f, 6f, 0f, -6f, 0f));
            Assign(ref _heightWave, CreateSeamlessLoop(0f, 0.12f, 0f, -0.1f, 0f));

            Assign(ref _sinkProgress, AnimationCurve.Linear(0f, 0f, 1f, 1f));
            Assign(ref _sinkTiltPitch, AnimationCurve.EaseInOut(0f, 0f, 1f, 14f));
            Assign(ref _sinkTiltYaw, AnimationCurve.EaseInOut(0f, 0f, 1f, 3f));
            Assign(ref _sinkTiltRoll, AnimationCurve.EaseInOut(0f, 0f, 1f, 20f));
        }

        private static bool IsCurveEmpty(AnimationCurve curve)
        {
            return curve == null || curve.length == 0;
        }

        private static AnimationCurve CreateSeamlessLoop(float v0, float v1, float v2, float v3, float v4)
        {
            var curve = new AnimationCurve(
                new Keyframe(0f, v0),
                new Keyframe(0.25f, v1),
                new Keyframe(0.5f, v2),
                new Keyframe(0.75f, v3),
                new Keyframe(1f, v4));
            for (var i = 0; i < curve.length; i++)
                curve.SmoothTangents(i, 0.35f);
            return curve;
        }

        private void OnDestroy()
        {
            _updateService.EnemiesUpdate.Unregister(this);
        }

        public void ConfigureRoute(Vector3 from, Vector3 to)
        {
            _basePosition = from;
            _routeEnd = to;
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _wavePhase = 0f;

            _updateService.EnemiesUpdate.Register(this);

            ApplyPose();
        }

        public void GameUpdate(float deltaTime)
        {
            if (_sinking)
            {
                TickSink(deltaTime);
                return;
            }

            Vector3 lastPos = _basePosition;
            _basePosition = Vector3.MoveTowards(_basePosition, _routeEnd, _speed * deltaTime);
            _moveDelta = _basePosition - lastPos;
            _wavePhase = Mathf.Repeat(_wavePhase + deltaTime * _waveCyclesPerSecond, 1f);
            ApplyPose();

            if (Vector3.SqrMagnitude(_basePosition - _routeEnd) <= _arrivalThreshold * _arrivalThreshold)
            {
                _interactablesFactory.CreateShipRopeLoot(transform.position.SetY(_lootSpawnHeight));
                Destroy(gameObject);
            }
        }

        private void ApplyPose()
        {
            var delta = _routeEnd - _basePosition;
            if (delta.sqrMagnitude < 1e-6f)
                delta = transform.forward;

            var baseRot = Quaternion.LookRotation(delta.normalized, Vector3.up) *
                          Quaternion.Euler(_modelRotationOffset);
            if (!_sinking)
            {
                var t = _wavePhase;
                var bob = Quaternion.Euler(
                    EvaluateOrZero(_pitchWave, t),
                    EvaluateOrZero(_yawWave, t),
                    EvaluateOrZero(_rollWave, t));
                baseRot *= bob;
            }

            transform.rotation = baseRot;
            var bobY = _sinking ? 0f : EvaluateOrZero(_heightWave, _wavePhase);
            transform.position = _basePosition + Vector3.up * bobY;
            if(_player)
            _player.transform.position += _moveDelta;
        }

        private static float EvaluateOrZero(AnimationCurve curve, float time)
        {
            return curve != null && curve.length > 0 ? curve.Evaluate(time) : 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_sinking)
                return;

            if (other.TryGetComponent(out PlayerEntity player))
            {
                _player = player;
            }
            if (!other.TryGetComponent(out Cliff _))
                return;
            

            BeginSink();
        }

        private void OnTriggerExit(Collider other)
        {   if (other.TryGetComponent(out PlayerEntity player))
            {
                _player = null;
            }
        }

        private void BeginSink()
        {
            _sinking = true;
         
                _player = null;
            
            _sinkElapsed = 0f;
            _sinkStartPosition = transform.position;
            _sinkEndPosition = _sinkStartPosition + Vector3.down * _sinkDepth;
            _sinkStartRotation = transform.rotation;

            _sinkParticles.Play();
            _interactablesFactory.CreateShipPlankLoot(transform.position.SetY(_lootSpawnHeight));

        }

        private void TickSink(float deltaTime)
        {
            _sinkElapsed += deltaTime;
            var t = _sinkDuration > Mathf.Epsilon ? Mathf.Clamp01(_sinkElapsed / _sinkDuration) : 1f;
            var progress = EvaluateOrOne(_sinkProgress, t);
            transform.position = Vector3.Lerp(_sinkStartPosition, _sinkEndPosition, progress);

            var tilt = Quaternion.Euler(
                EvaluateOrZero(_sinkTiltPitch, t),
                EvaluateOrZero(_sinkTiltYaw, t),
                EvaluateOrZero(_sinkTiltRoll, t));
            transform.rotation = _sinkStartRotation * tilt;

            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }

        private static float EvaluateOrOne(AnimationCurve curve, float time)
        {
            return curve != null && curve.length > 0 ? curve.Evaluate(time) : time;
        }
    }
}
