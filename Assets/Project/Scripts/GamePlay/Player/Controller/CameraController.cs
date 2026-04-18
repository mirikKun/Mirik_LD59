using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.Input;
using Project.Scripts.Infrastructure.Settings;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Controller
{
    public class CameraController : EntityComponent
    {
        [Range(0f, 90f)] public float _upperVerticalLimit = 75f;
        [Range(0f, 90f)] public float _lowerVerticalLimit = 76f;

        [SerializeField] private float _maxCameraSpeed = 9;

        [SerializeField] private Transform _horizontalPivot;
        [SerializeField] private Transform _verticalPivot;
        private float CameraSpeedModifier => 1;

        public Vector3 GetUpDirection() => _verticalPivot.up;
        public Vector3 GetFacingDirection() => _horizontalPivot.forward;

        public Transform CameraTrX => _horizontalPivot;
        public Transform CameraTrY => _verticalPivot;


        private float _currentXAngle;
        private float _currentYAngle;
        private IInputReader _input;
        private ISettingsService _settingsService;
        private bool _disabled;

        private float CameraSpeed =>
            _maxCameraSpeed * CameraSpeedModifier * _settingsService.SettingsData.MouseSensitivity;


        [Inject]
        private void Construct(IInputReader input, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _input = input;
        }

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity);
            _currentXAngle = _verticalPivot.localRotation.eulerAngles.x;
            _currentYAngle = _horizontalPivot.localRotation.eulerAngles.y;
        }

        public void TickLateUpdate(float deltaTime)
        {
            if (_disabled) return;
            RotateCamera(deltaTime, _input.LookDirection.x, -_input.LookDirection.y);
        }

        public void SetCameraMovementActive(bool active)
        {
            _disabled = !active;
        }

        private void RotateCamera(float deltaTime, float horizontalInput, float verticalInput)
        {
            _currentYAngle += horizontalInput * deltaTime * CameraSpeed;

            _currentXAngle += verticalInput * deltaTime * CameraSpeed;
            _currentXAngle = Mathf.Clamp(_currentXAngle, -_upperVerticalLimit, _lowerVerticalLimit);

            _horizontalPivot.localRotation = Quaternion.Euler(0, _currentYAngle, 0);
            _verticalPivot.localRotation = Quaternion.Euler(_currentXAngle, 0, 0);
        }
    }
}