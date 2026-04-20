using TMPro;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class LightHouseLightController : MonoBehaviour
    {
        [SerializeField] private Transform _coneTransform;
        [SerializeField] private Vector3 _baseConeScale = new Vector3(5, 5, 5);
        [SerializeField] private Light _spotLight;
        [SerializeField] private float _baseLightLenght = 10;
        [SerializeField] private float _baseLightIntensity = 20;

        [SerializeField] [Range(1f, 70f)] private float _strength = 5f;
        [SerializeField] private float _maxStrength = 45f;
        [SerializeField] private float _minStrength = 1f;
        [SerializeField] private float _strengthFadePerSecond = 0.4f;

        [Header("Fire")]
        [SerializeField] private ParticleSystem _fireParticleSystem;

        [SerializeField][Range(0,0.5f)] private float _minFireSrength=0.2f;
        [SerializeField] private TextMeshProUGUI _fuelText;
        private float _particleLifetimeBase;
        private float _particleSpeedBase;
        private float _particleSizeBase;
        private float _particleStrengthRef;
        private bool _particleBaselinesCaptured;

        private void Awake()
        {
            _strength = Mathf.Clamp(_strength, _minStrength, _maxStrength);
            CaptureFireParticleBaselines();
            SetLightStrength();
        }

        private void OnValidate()
        {
            _maxStrength = Mathf.Max(_minStrength, _maxStrength);
            _strength = Mathf.Clamp(_strength, _minStrength, _maxStrength);
            SetLightStrength();
        }

        private void Update()
        {
            float prev = _strength;
            _strength = Mathf.Max(_minStrength, _strength - _strengthFadePerSecond * Time.deltaTime);
            if (!Mathf.Approximately(prev, _strength))
                SetLightStrength();
        }

        public bool CanAddStrength() => _strength < _maxStrength - Mathf.Epsilon;

        public void AddStrength(float strength)
        {
            if (strength <= 0f)
                return;
            _strength = Mathf.Min(_maxStrength, _strength + strength);
            SetLightStrength();
        }

        public float GetStrengthPercent()
        {
            return _strength / _maxStrength;
        }

        private void CaptureFireParticleBaselines()
        {
            if (_fireParticleSystem == null)
                return;

            var main = _fireParticleSystem.main;
            _particleLifetimeBase = ReadMainScalar(main.startLifetime);
            _particleSpeedBase = ReadMainScalar(main.startSpeed);
            _particleSizeBase = ReadMainScalar(main.startSize);
            _particleStrengthRef = _maxStrength;
            _particleBaselinesCaptured = true;
        }

        private static float ReadMainScalar(ParticleSystem.MinMaxCurve curve)
        {
            switch (curve.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    return curve.constant;
                case ParticleSystemCurveMode.TwoConstants:
                    return (curve.constantMin + curve.constantMax) * 0.5f;
                default:
                    return curve.constantMax > 0f ? curve.constantMax : curve.constant;
            }
        }

        private void SetLightStrength()
        {
            SetFuelText();
            if (_spotLight != null)
            {
                _spotLight.intensity = _baseLightIntensity * _strength;
                _spotLight.range = _baseLightLenght * _strength;
            }

            if (_coneTransform != null)
                _coneTransform.localScale = _baseConeScale * _strength;

            ApplyFireParticles();
        }

        private void SetFuelText()
        {
            int max = 100;
            int current = (int)(_strength / _maxStrength * 100);
            _fuelText.text = $"Fuel {current}/{max}%";
        }

        private void ApplyFireParticles()
        {
            if (_fireParticleSystem == null || !_particleBaselinesCaptured)
                return;

            float refStrength = _particleStrengthRef;
            float k =Mathf.Lerp(_minFireSrength,1,_strength / refStrength) ;

            var main = _fireParticleSystem.main;
            main.startLifetime = _particleLifetimeBase * k;
            main.startSpeed = _particleSpeedBase * k;
            main.startSize = _particleSizeBase * k;
        }
    }
}
