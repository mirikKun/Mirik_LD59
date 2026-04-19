using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class LightHouseController : MonoBehaviour
    {
        [SerializeField] private LightHouseWheel _leftWheel;
        [SerializeField] private LightHouseWheel _rigtWheel;
        [SerializeField] private LightHouseWheel _upWheel;
        [SerializeField] private LightHouseWheel _downWheel;

        [SerializeField] private Transform _lightHouseRotationPlatform;
        [SerializeField] private Transform _lightHouseMirror;
        [SerializeField] private Vector3 _horizontalAxis = new Vector3(0, 1, 0);
        [SerializeField] private Vector3 _verticalAxis = new Vector3(1, 0, 0);

        [SerializeField] private float _minVerticalAngle = 0f;
        [SerializeField] private float _maxVerticalAngle = 70f;
        [SerializeField] private float _startHorizontalRotation;
        [SerializeField] private float _startVerticalRotation = 60f;

        private float _horizontalAngle;
        private float _verticalAngle;

        private void Start()
        {
            _horizontalAngle = _startHorizontalRotation;
            var minV = Mathf.Min(_minVerticalAngle, _maxVerticalAngle);
            var maxV = Mathf.Max(_minVerticalAngle, _maxVerticalAngle);
            _verticalAngle = Mathf.Clamp(_startVerticalRotation, minV, maxV);

            _leftWheel.WheelRotated += RotateMirrorHorizontally;
            _rigtWheel.WheelRotated += RotateMirrorHorizontally;
            _downWheel.WheelRotated += RotateMirrorVertically;
            _upWheel.WheelRotated += RotateMirrorVertically;

            ApplyRotations();
        }

        private void RotateMirrorHorizontally(float angle)
        {
            _horizontalAngle += angle;
            ApplyRotations();
        }

        private void RotateMirrorVertically(float angle)
        {
            var minV = Mathf.Min(_minVerticalAngle, _maxVerticalAngle);
            var maxV = Mathf.Max(_minVerticalAngle, _maxVerticalAngle);
            float targetAngle = _verticalAngle + angle;
          
            _verticalAngle = Mathf.Clamp(targetAngle, minV, maxV);
            if (Mathf.Abs(_verticalAngle - targetAngle) > Mathf.Epsilon)
            {
                _upWheel.Rotate(_verticalAngle - targetAngle);
            }
            
            ApplyRotations();
        }

        private void ApplyRotations()
        {
            if (_lightHouseRotationPlatform != null && _horizontalAxis.sqrMagnitude >= Mathf.Epsilon)
                _lightHouseRotationPlatform.localRotation =
                    Quaternion.AngleAxis(_horizontalAngle, _horizontalAxis.normalized);

            if (_lightHouseMirror != null && _verticalAxis.sqrMagnitude >= Mathf.Epsilon)
                _lightHouseMirror.localRotation =
                    Quaternion.AngleAxis(_verticalAngle, _verticalAxis.normalized);
        }
    }
}
