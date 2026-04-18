using System;
using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Common.Movement;
using Project.Scripts.GamePlay.Common.Time;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Core.Physic.Enums;
using Project.Scripts.GamePlay.Core.Physic.Raycast;
using Project.Scripts.GamePlay.Player.PlayerStateMachine;
using Project.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Controller
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class PlayerMover : EntityComponent, IPausable, IBaseMover
    {
        [SerializeField] private Transform _footRoot;

        [Header("Collider Settings:")] [Range(0f, 1f)] [SerializeField]
        private float _stepHeightRatio = 0.175f;

        private float _footRootOffset = 0.175f;

        [SerializeField] private float _colliderHeight = 1.5f;
        [SerializeField] private float _colliderThickness = 1f;
        [SerializeField] private Vector3 _colliderOffset = Vector3.zero;
        [SerializeField] private float _adjustmentVelocityMultiplier = 0.5f;

        [Header("Movement Settings:")] [SerializeField]
        private float _movementSpeed = 7f;


        [SerializeField] private float _combatSpeedMultiplier = 0.5f;

        [SerializeField] private float _airControlRate = 8f;

        [SerializeField] private float _airFriction = 0.7f;
        [SerializeField] private float _groundFriction = 80;
        [SerializeField] private float _gravity = 30f;

        [SerializeField] private float _slideGravity = 5f;

        [SerializeField] private float _slopeLimit = 46;
        [SerializeField] private float _coyoteTimeDuration = 0.15f;

        [SerializeField] private bool _useLocalMomentum;


        private Rigidbody _rb;
        private Transform _tr;
        private CapsuleCollider _col;
        private RaycastSensor _sensor;
        private  CountdownTimer _coyoteTimer;

        private float _baseColliderHeight;

        private bool _isGrounded;
        private MovingObject _currentMovingObject;
        private float _additionalRaycastLength;
        private float _baseSensorRange;

        private Vector3
            _currentGroundAdjustmentVelocity; // Velocity to adjust player position to maintain ground contact


        private int _currentLayer;

        private Vector3 _momentum;
        private Vector3 _savedVelocity;
        private Vector3 _savedMovementVelocity;

        [Header("Sensor Settings:")] [SerializeField]
        private bool _isInDebugMode;


        private bool _isUsingExtendedSensorRange = true; // Use extended range for smoother ground transitions
        private IUpdateService _updateService;
        private ITimeService _timeService;

        public event Action Teleported;
        public float Gravity => _gravity;
        public float GroundFriction => _groundFriction;
        public float AirFriction => _airFriction;
        public float SlideGravity => _slideGravity;
        public float MovementSpeed => _movementSpeed ;
        public float AirControlRate => _airControlRate;
        public bool CoyoteTimeFinished => _coyoteTimer.IsFinished;


        public bool IsFlying => false;

        public Vector3 GetVelocity() => _savedVelocity;
        public MovingObject  MovingObject => _currentMovingObject;

        public Vector3 GetMomentum() => _useLocalMomentum ? _tr.localToWorldMatrix * _momentum : _momentum;

        public Transform Tr => _tr;


        [Inject]
        private void Construct(ITimeService timeService, IUpdateService updateService)
        {
            _timeService = timeService;
            _updateService = updateService;
        }

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity); 
            Setup();
            RecalculateColliderDimensions();
        }

        public override void StartEntity()
        {
            _updateService.Pausable.Register(this);
        }

        private void OnDestroy()
        {
            _updateService.Pausable.Unregister(this);
        }

        private void OnValidate()
        {
            if (gameObject.activeInHierarchy)
            {
                RecalculateColliderDimensions();
            }
        }

        public void FixedTick(float fixedDeltaTime)
        {
            _savedVelocity += _useLocalMomentum ? _tr.localToWorldMatrix * _momentum : _momentum;
            SetExtendSensorRange(Entity.Get<PlayerStateMachineContainer>().IsGroundedState());
            _savedMovementVelocity = CalculateMovementVelocity();
            SetRbVelocity(_savedVelocity, _timeService.TimeScale);
        }

        public void Tick(float deltaTime)
        {
            SetFootRootTransform();
        }

        public void LateTick(float deltaTime)
        {
            if (_isInDebugMode)
            {
                _sensor.DrawDebug();
            }
        }

        public void SetVelocity(Vector3 velocity)
        {
            _savedVelocity = velocity;
        }

        public void OnGroundContactLost()
        {
            if (_useLocalMomentum) _momentum = _tr.localToWorldMatrix * _momentum;

            Vector3 velocity = _savedMovementVelocity;
            if (velocity.sqrMagnitude >= 0f && _momentum.sqrMagnitude > 0f)
            {
                Vector3 projectedMomentum = Vector3.Project(_momentum, velocity.normalized);
                float dot = VectorMath.GetDotProduct(projectedMomentum.normalized, velocity.normalized);

                if (projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f) velocity = Vector3.zero;
                else if (dot > 0f) velocity -= projectedMomentum;
            }

            _momentum += velocity;

            if (_useLocalMomentum) _momentum = _tr.worldToLocalMatrix * _momentum;
        }

        public void SetMomentum(Vector3 momentum)
        {
            _momentum = momentum;

            if (_useLocalMomentum) _momentum = _tr.worldToLocalMatrix * _momentum;
        }

        public void Teleport(Vector3 position)
        {
            Tr.position = position;
            // if(_sensor.CastAndCheck())
            // {
            //     Tr.position+=_sensor.GetNormal()*(_baseSensorRange + _colliderHeight * _tr.localScale.x * _stepHeightRatio + _additionalRaycastLength-_sensor.GetDistance());
            // }
         
            Teleported?.Invoke();
        }

        public bool IsRising() => VectorMath.GetDotProduct(GetMomentum(), _tr.up) > 0.001f;

        public bool IsFalling() => VectorMath.GetDotProduct(GetMomentum(), _tr.up) < 0.001f;

        public bool IsGroundTooSteep() => !IsGrounded() || Vector3.Angle(GetGroundNormal(), _tr.up) > _slopeLimit;

        public Vector3 GetHorizontalMomentum() => GetMomentum() - GetVerticalMomentum();

        public Vector3 GetVerticalMomentum() => VectorMath.ExtractDotVector(GetMomentum(), _tr.up);


        public Vector3 CalculateMovementVelocity() =>
            Entity.Get<PlayerController>().GetInputMovementDirection() * MovementSpeed;

        public void StartCrouching(float height)
        {
            SetColliderHeight(height);
        }


        public void StopCrouching()
        {
            ResetColliderHeight();
        }

        private void SetColliderHeight(float height)
        {
            _colliderHeight = height;
            RecalculateColliderDimensions();
            _additionalRaycastLength = (_baseColliderHeight - height) / 2f;
        }

        private void ResetColliderHeight()
        {
            SetColliderHeight(_baseColliderHeight);
        }

        private void SetFootRootTransform()
        {
            if (!_isGrounded) return;

            _footRoot.position = _sensor.GetPosition() + _sensor.GetNormal() * _footRootOffset;
            _footRoot.up = _sensor.GetNormal();
        }

        public void SetParent(Transform parent)
        {
            Tr.SetParent(parent);
        }

        public void CheckForGround(float fixedDeltaTime)
        {
            if (_currentLayer != gameObject.layer)
            {
                RecalculateSensorLayerMask();
            }

            _currentGroundAdjustmentVelocity = Vector3.zero;
            _sensor.CastLength = _isUsingExtendedSensorRange
                ? _baseSensorRange + _colliderHeight * _tr.localScale.x * _stepHeightRatio + _additionalRaycastLength
                : _baseSensorRange + _additionalRaycastLength;
            _sensor.Cast();

            bool wasGrounded = _isGrounded;
            _isGrounded = _sensor.HasDetectedHit();

            if (_isGrounded)
            {
                _currentMovingObject = _sensor.GetCollider().GetComponentInParent<MovingObject>();
            }
            if (wasGrounded != _isGrounded&&!IsRising())
            {
                _coyoteTimer.Start();
            }
            if (!_isGrounded) return;


            float distance = _sensor.GetDistance();
            float upperLimit = _colliderHeight * _tr.localScale.x * (1f - _stepHeightRatio) * 0.5f;
            float middle = upperLimit + _colliderHeight * _tr.localScale.x * _stepHeightRatio;
            float distanceToGo = middle - distance;

            _currentGroundAdjustmentVelocity =
                _tr.up * (distanceToGo / fixedDeltaTime * _adjustmentVelocityMultiplier);
        }

        public bool IsGrounded() => _isGrounded;

        public Vector3 GetGroundNormal() => _sensor.GetNormal();


        public void SetRbVelocity(Vector3 velocity, float timeScale) => _rb.linearVelocity =timeScale>0?
            (velocity + _currentGroundAdjustmentVelocity) * timeScale:Vector3.zero;

        public void SetExtendSensorRange(bool isExtended) => _isUsingExtendedSensorRange = isExtended;


        private void Setup()
        {
            _tr = transform;
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<CapsuleCollider>();

            _rb.freezeRotation = true;
            _rb.useGravity = false;
            _coyoteTimer=new CountdownTimer(_coyoteTimeDuration);
            _baseColliderHeight = _colliderHeight;
        }

        private void RecalculateColliderDimensions()
        {
            if (_col == null)
            {
                Setup();
            }

            _col.height = _colliderHeight * (1f - _stepHeightRatio);
            _col.radius = _colliderThickness / 2f;
            _col.center = _colliderOffset * _colliderHeight + new Vector3(0f, _stepHeightRatio * _col.height / 2f, 0f);

            if (_col.height / 2f < _col.radius)
            {
                _col.radius = _col.height / 2f;
            }

            RecalibrateSensor();
        }

        private void RecalibrateSensor()
        {
            _sensor ??= new RaycastSensor(_tr);

            _sensor.SetCastOrigin(_col.bounds.center);
            _sensor.SetCastDirection(CastDirection.Down);
            RecalculateSensorLayerMask();

            const float
                safetyDistanceFactor =
                    0.005f; // Small factor added to prevent clipping issues when the sensor range is calculated

            float length = _colliderHeight * (1f - _stepHeightRatio) * 0.5f + _colliderHeight * _stepHeightRatio;
            _baseSensorRange = length * (1f + safetyDistanceFactor) * _tr.localScale.x;
            _sensor.CastLength = length * _tr.localScale.x;
        }

        private void RecalculateSensorLayerMask()
        {
            int objectLayer = gameObject.layer;
            int layerMask = Physics.AllLayers;

            for (int i = 0; i < 32; i++)
            {
                if (Physics.GetIgnoreLayerCollision(objectLayer, i))
                {
                    layerMask &= ~(1 << i);
                }
            }

            int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
            layerMask &= ~(1 << ignoreRaycastLayer);

            _sensor.Layermask = layerMask;
            _currentLayer = objectLayer;
        }

        public void SetRigidbodyKinematic(bool kinematic)
        {
            _rb.isKinematic = kinematic;
        }

        public void Pause()
        {
            SetRigidbodyKinematic(true);
        }

        public void Resume()
        {
            SetRigidbodyKinematic(false);
        }

        public void ApplyForce(Vector3 force)
        {
            SetMomentum(_momentum + force);
        }
    }
}