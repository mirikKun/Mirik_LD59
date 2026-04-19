using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerResources;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.GamePlay.Player.HUD
{
    public class RopesHudView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _ropesCanvasGroup;
        [SerializeField] private CanvasGroup _aimCanvasGroup;
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private Image _ropeChargePrefab;
        [SerializeField] private Image _ropeProgress;
        [SerializeField] private Transform _ropeChargesParent;

        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;

        [SerializeField] private float _cantGrappleAlpha = 0.01f;

        private readonly List<Image> _spawnedChargeImages = new();
        private RopeResourceController _ropes;
        private int _lastBuiltMaxRopes = -1;

        private void Awake()
        {
            _ropesCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            if (_playerEntity == null || !_playerEntity.TryGet(out RopeResourceController ropes))
                return;

            _ropes = ropes;
            _ropes.RopeChargesChanged += OnRopeChargesChanged;
            _ropes.SwingProgressChanged += OnSwingProgressChanged;
            _ropes.CanGrapple += SetCanGrapple;
        }

        private void OnDisable()
        {
            if (_ropes != null)
            {
                _ropes.RopeChargesChanged -= OnRopeChargesChanged;
                _ropes.SwingProgressChanged -= OnSwingProgressChanged;
                _ropes.CanGrapple -= SetCanGrapple;

                _ropes = null;
            }
        }

        private void OnRopeChargesChanged(int maxRopes, int airHookChargesRemaining) =>
            UpdateRopesUi(maxRopes, airHookChargesRemaining);

        private void UpdateRopesUi(int maxRopes, int airHookChargesRemaining)
        {
            _ropesCanvasGroup.alpha = 1f;

            if (maxRopes != _lastBuiltMaxRopes)
            {
                _lastBuiltMaxRopes = maxRopes;
                RebuildChargeIcons(maxRopes);
            }

            ApplyChargeColors(airHookChargesRemaining);
        }

        private void OnSwingProgressChanged(float progress01)
        {
            if (_ropeProgress != null)
                _ropeProgress.fillAmount = progress01;
        }

        private void SetCanGrapple(bool canGrapple)
        {
            _aimCanvasGroup.alpha = canGrapple ? 1f : _cantGrappleAlpha;
        }

        private void RebuildChargeIcons(int maxRopes)
        {
            foreach (Image img in _spawnedChargeImages)
            {
                if (img != null)
                    Destroy(img.gameObject);
            }

            _spawnedChargeImages.Clear();

            if (_ropeChargePrefab == null || _ropeChargesParent == null || maxRopes <= 0)
                return;

            for (var i = 0; i < maxRopes; i++)
            {
                var instance = Instantiate(_ropeChargePrefab.gameObject, _ropeChargesParent);
                instance.SetActive(true);
                if (instance.TryGetComponent(out Image image))
                    _spawnedChargeImages.Add(image);
            }
        }

        private void ApplyChargeColors(int airHookChargesRemaining)
        {
            for (var i = 0; i < _spawnedChargeImages.Count; i++)
            {
                Image img = _spawnedChargeImages[i];
                if (img == null)
                    continue;
                img.color = i < airHookChargesRemaining ? _activeColor : _inactiveColor;
            }
        }
    }
}
