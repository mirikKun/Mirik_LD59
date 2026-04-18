using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.Infrastructure.Sounds;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    public class  CameraMovingEffects : MonoBehaviour
    {
        [Header("Camera References")] [SerializeField]
        private CinemachineCamera _playerCamera;

        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Transform _armoryHolder;
        [SerializeField] private float _cameraPositionReturnSpeed = 3;
        [SerializeField] private float _armoryHolderReturnSpeed = 3;
        [SerializeField] private Vector3 _handsChangePositionOffset = new Vector3(0,-0.3f,-0.3f);


        [Header("Walking Shake Settings")] [SerializeField]
        private float _walkShakeIntensityVertical = 0.1f;

        [SerializeField] private float _walkShakeIntensityHorizontal = 0.06f;
        [SerializeField] private float _walkShakeIntensityForward = 0.03f;
        [SerializeField] private float _walkShakeCycleTime = 1.3f;

        [SerializeField] private AnimationCurve _walkShakeVerticalCurve ;
        [SerializeField] private AnimationCurve _walkShakeHorizontalCurve ;
        [SerializeField] private AnimationCurve _walkShakeForwardCurve ;
        [SerializeField] private bool _enableWalkShake = true;
        [SerializeField] private SoundData _stepSound;


        [Header("Fall Settings")] [SerializeField]
        private float _fallMaxHeight = 20;

        [SerializeField] private float _heightMultiplier = 3;

        [SerializeField] private float _fallShakeMaxDuration = 0.4f;
        [SerializeField] private float _fallShakeMaxPower = 0.04f;

        [SerializeField] private float _fallBounceMaxDuration = 1.3f;
        [SerializeField] private float _fallBounceMaxPower = -0.4f;
        [SerializeField] private float _fallBounceMaxRotation = 5f;
        [SerializeField] private AnimationCurve _fallBounceAnimationCurve ;
        [SerializeField] private bool _enableFallBounce = true;
        [SerializeField] private SoundData _fallLandSound;

        [Header("Camera Shake Settings")] [SerializeField]
        private float _defaultCameraShakePower = 0.01f;

        [SerializeField] private float _defaultCameraShakeDuration = 0.3f;
        [SerializeField] private bool _enableCameraShake = true;


        [Header("Attack Settings")] [SerializeField]
        private float _attackTiltDuration = 0.2f;

        [SerializeField] private float _attackTiltAngle = 5f;
        [SerializeField] private AnimationCurve _attackTiltCurve ;
        [SerializeField] private bool _enableAttackTilt = true;


        [Header("Wall Run Settings")] [SerializeField]
        private float _wallRunTiltAngle = 9;

        [SerializeField] private float _wallRunTiltSpeed = 60f;
        [SerializeField] private bool _enableWallRunTilt = true;

        [Header("FOV Settings")] [SerializeField]
        private float _defaultFOV = 60f;

        [SerializeField] private float _maxFOV = 70f;
        [SerializeField] private float _fovChangeSpeed = 135f;
        [SerializeField] private bool _enableFOVChange = true;

        private float _initialCameraY;
        private float _initialCameraZ;

        private Vector3 _targetCameraPosition;
        private Vector3 _targetArmoryPosition;

        private float _movingShakeElapsedTime;
        private float _cameraShakeElapsedTime;
        private float _fallBounceElapsedTime;

        private float _currentCameraShakePower;
        private float _currentCameraShakeDuration;
        
        private float _prevVerticalOffset;
        private bool _walkMovingDown;


        private float _currentWallTiltAngle;
        private float _targetWallTiltAngle;

        private float _attackTiltElapsedTime;
        private bool _enableAttackTiltActive;
        private Quaternion _attackTiltRotation;

        private float _currentFOV;
        private float _targetFOV;
        private bool _isGrounded;

        private bool _cameraShakeActive;

        private bool _fallBounceActive;

        private float _fallHeight;
            private ISoundsSystem _soundsSystem;

            private Vector3 DefaultCameraPosition => new Vector3(0, _initialCameraY, _initialCameraZ);

        [Inject]
        private void Construct(ISoundsSystem soundsSystem)
        {
            _soundsSystem = soundsSystem;
        }
        private void Start()
        {
            if (_cameraHolder == null)
                _cameraHolder = _playerCamera.transform;

            _initialCameraY = _cameraHolder.localPosition.y;
            _initialCameraZ = _cameraHolder.localPosition.z;

            _currentFOV = _defaultFOV;
            _targetFOV = _defaultFOV;
            _playerCamera.Lens.FieldOfView = _defaultFOV;


            _targetCameraPosition = DefaultCameraPosition;
        }

        public void InitEffect(ActorEntity entity)
        {
        }

        public void Tick(float deltaTime)
        {
            _targetArmoryPosition = Vector3.Lerp(_targetArmoryPosition, DefaultCameraPosition, deltaTime * _armoryHolderReturnSpeed);
            _targetCameraPosition = Vector3.Lerp(_targetCameraPosition, DefaultCameraPosition, deltaTime * _cameraPositionReturnSpeed);

            if (_enableWalkShake)
                ApplyWalkingShake(deltaTime);

            if (_enableWallRunTilt)
                ApplyWallRunTilt(deltaTime);

            if (_enableFOVChange)
                ApplyFOVChange(deltaTime);

            if (_enableCameraShake)
                ApplyCameraShakeEffect(deltaTime);
            if (_enableFallBounce)
                ApplyFallBounceEffect(deltaTime);
            if (_enableAttackTilt)
                ApplyAttackTilt(deltaTime);


            _cameraHolder.localPosition = _targetCameraPosition;
            _armoryHolder.localPosition = _targetArmoryPosition;
        }


        public void SetGrounded(bool grounded)
        {
            _isGrounded = grounded;
        }

        public void StartFallEffect(float height)
        {
            float minHeight = 0.3f;
            if (height < minHeight)
                return;
            _soundsSystem.Play(_fallLandSound);

            _fallBounceActive = true;
            _fallHeight = height;
            _fallBounceElapsedTime = 0;

            float fallHeightMultiplier = Mathf.Clamp(_fallHeight * _heightMultiplier / _fallMaxHeight, 0, 1);
            StartCameraShake(fallHeightMultiplier * _fallShakeMaxPower, fallHeightMultiplier * _fallShakeMaxDuration);
        }

        public void StartCameraShake(float intensity = -1, float duration = -1)
        {
            if (!_enableCameraShake)
                return;

            _currentCameraShakePower = intensity > 0 ? intensity : _defaultCameraShakePower;
            _currentCameraShakeDuration = duration > 0 ? duration : _defaultCameraShakeDuration;

            _cameraShakeElapsedTime = 0f;
            _cameraShakeActive = true;
        }


        public void StartAttackTilt(Vector3 attackDirection)
        {
            if (!_enableAttackTilt)
                return;

            _attackTiltElapsedTime = 0f;

            Vector3 forward = attackDirection.normalized;


            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            // if(right.z<0)
            //     right.z = -right.z;
            _attackTiltRotation = Quaternion.identity * Quaternion.Euler(right * _attackTiltAngle);
            ;
            _enableAttackTiltActive = true;
        }

        private void ApplyAttackTilt(float deltaTime)
        {
            if (!_enableAttackTiltActive)
                return;


            if (_attackTiltElapsedTime > _attackTiltDuration)
            {
                _enableAttackTiltActive = false;
                _attackTiltElapsedTime = 0f;
                return;
            }

            _attackTiltElapsedTime += deltaTime;

            float normalizedTime = _attackTiltElapsedTime / _attackTiltDuration;
            Quaternion currentRotation = Quaternion.Lerp(Quaternion.identity, _attackTiltRotation,
                _attackTiltCurve.Evaluate(normalizedTime));

            _cameraHolder.localRotation = currentRotation;
        }


        private void ApplyWalkingShake(float deltaTime)
        {
            if (!_isGrounded)
            {
                _movingShakeElapsedTime = 0f;
                return;
            }


            if (_isGrounded)
            {
                
                
                _movingShakeElapsedTime += deltaTime;

                float normalizedTime = (_movingShakeElapsedTime % _walkShakeCycleTime) / _walkShakeCycleTime;

                
                float verticalOffset = _walkShakeVerticalCurve.Evaluate(normalizedTime) * _walkShakeIntensityVertical;
                float horizontalOffset =
                    _walkShakeHorizontalCurve.Evaluate(normalizedTime) * _walkShakeIntensityHorizontal;
                float forwardOffset = _walkShakeForwardCurve.Evaluate(normalizedTime) * _walkShakeIntensityForward;


                Vector3 targetCameraPosition = new Vector3(horizontalOffset, verticalOffset, forwardOffset);
                _targetCameraPosition =targetCameraPosition;

                CheckOnStepSound(verticalOffset);
            }
        }

        private void CheckOnStepSound(float verticalOffset)
        {
            if (_walkMovingDown && verticalOffset > _prevVerticalOffset)
            {
                    _soundsSystem.Play(_stepSound);
            }
            _walkMovingDown= verticalOffset < _prevVerticalOffset;
            _prevVerticalOffset = verticalOffset;
        }

        private void ApplyCameraShakeEffect(float deltaTime)
        {
            if (!_cameraShakeActive)
                return;


            if (_cameraShakeElapsedTime > _currentCameraShakeDuration)
            {
                _cameraShakeElapsedTime = 0f;
                _cameraShakeActive = false;
                return;
            }


            _targetCameraPosition += Random.insideUnitSphere * _currentCameraShakePower;
            _cameraShakeElapsedTime += deltaTime;
        }

        private void ApplyFallBounceEffect(float deltaTime)
        {

            
            if (!_fallBounceActive)
                return;

            float fallHeightMultiplier = Mathf.Clamp(_fallHeight * _heightMultiplier / _fallMaxHeight, 0, 1);
            float bounceDuration = _fallBounceMaxDuration * fallHeightMultiplier;
            float bouncePower = _fallBounceMaxPower * fallHeightMultiplier;
            float bounceRotation = _fallBounceMaxRotation * fallHeightMultiplier;

            if (_fallBounceElapsedTime > bounceDuration)
            {
                _fallBounceActive = false;
                _fallBounceElapsedTime = 0f;
                return;
            }

            _fallBounceElapsedTime += deltaTime;

            float normalizedTime = _fallBounceElapsedTime / bounceDuration;
            float bounceOffset = _fallBounceAnimationCurve.Evaluate(normalizedTime) * bouncePower;
            float bounceRotationAngle = _fallBounceAnimationCurve.Evaluate(normalizedTime) * bounceRotation;

            _armoryHolder.localPosition = new Vector3(0, bounceOffset, 0);
            _cameraHolder.localEulerAngles = new Vector3(bounceRotationAngle, _cameraHolder.localEulerAngles.y,
                _cameraHolder.localEulerAngles.z);
        }


        private void ApplyWallRunTilt(float deltaTime)
        {
            _currentWallTiltAngle =
                Mathf.MoveTowards(_currentWallTiltAngle, _targetWallTiltAngle, _wallRunTiltSpeed * deltaTime);

            Vector3 currentRotation = _cameraHolder.localEulerAngles;
            _cameraHolder.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, _currentWallTiltAngle);
        }

        private void ApplyFOVChange(float deltaTime)
        {
            _currentFOV = Mathf.MoveTowards(_currentFOV, _targetFOV, _fovChangeSpeed * deltaTime);
            _playerCamera.Lens.FieldOfView = _currentFOV;
        }

        public void SetWallRunTilt(float side)
        {
            if (!_enableWallRunTilt)
                return;

            _targetWallTiltAngle = side * _wallRunTiltAngle;
        }

        public void SetTargetFOV(float newFOV)
        {
            if (!_enableFOVChange)
                return;

            _targetFOV = newFOV;
        }

        public void SetCurrentFOV(float newFOV)
        {
            if (!_enableFOVChange)
                return;

            _currentFOV = newFOV;
            _playerCamera.Lens.FieldOfView = _currentFOV;
        }

        public void ResetFOV()
        {
            _targetFOV = _defaultFOV;
        }

        [ContextMenu("ApplyHandsChangeAnimation")]
        public void PlayHandsChangeAnimation()
        {
            _targetArmoryPosition = _handsChangePositionOffset;
        }
    }
}