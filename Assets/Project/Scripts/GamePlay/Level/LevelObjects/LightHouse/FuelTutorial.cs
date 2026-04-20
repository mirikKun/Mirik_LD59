using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class FuelTutorial:MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private LightHouseLightController _lightHouseLightController;
        [SerializeField] private float _maxLightStrengthToShowTutorial = 0.5f;
        [SerializeField] private float _fadeSpeed = 4f;
        
        private bool _isPlayerInside;
        private float _targetAlpha;

        private void Start()
        {
            _canvasGroup.alpha = 0f;
            _targetAlpha = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerEntity _))
                return;
            
            _isPlayerInside = true;
            UpdateTargetAlpha();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerEntity _))
                return;
            
            _isPlayerInside = false;
            _targetAlpha = 0f;
        }

        private void Update()
        {
            if (_isPlayerInside)
                UpdateTargetAlpha();
            
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetAlpha, _fadeSpeed * Time.deltaTime);
        }

        private void UpdateTargetAlpha()
        {
            if (_lightHouseLightController.GetStrengthPercent() < _maxLightStrengthToShowTutorial)
            {
                _targetAlpha = 1f;
            }
            else
            {
                _targetAlpha = 0f;
            }
        }
    }
}