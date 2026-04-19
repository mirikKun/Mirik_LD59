using System;
using System.Collections;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class LightHouseWheel:BaseInteractable
    {
        [SerializeField] private Transform _rotationTransform;
        [SerializeField] private Vector3 _rotationAxis;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Image _image;
        [SerializeField] private float _highlightFadeDuration = 0.2f;

        private Coroutine _highlightFadeRoutine;

        public event Action<float> WheelRotated; 
        public override void Interact(BaseEntity entity)
        {
           //Debug.Log("Interacted");
           float angle = _rotationSpeed * Time.deltaTime;
           if (_rotationAxis.sqrMagnitude >= Mathf.Epsilon)
               Rotate(angle);
           WheelRotated?.Invoke(angle);
        }

        public void Rotate(float angle)
        {
            _rotationTransform.localRotation *= Quaternion.AngleAxis(angle, _rotationAxis.normalized);
        }

        public override void HighLight(BaseEntity entity)
        {
            base.HighLight(entity);
            if (_highlightFadeRoutine != null)
                StopCoroutine(_highlightFadeRoutine);
            _highlightFadeRoutine = StartCoroutine(FadeHighlightRoutine(1f));
        }

        public override void UnHighLight()
        {
            if (_highlightFadeRoutine != null)
                StopCoroutine(_highlightFadeRoutine);
            _highlightFadeRoutine = StartCoroutine(FadeHighlightRoutine(0f));
        }

        private IEnumerator FadeHighlightRoutine(float targetAlpha)
        {
            if (targetAlpha > 0.001f)
            {
                _image.enabled = true;
                var c0 = _image.color;
                c0.a = 0f;
                _image.color = c0;
            }

            var startAlpha = _image.color.a;
            var elapsed = 0f;
            var duration = Mathf.Max(0.01f, _highlightFadeDuration);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var a = Mathf.Lerp(startAlpha, targetAlpha, t);
                var col = _image.color;
                col.a = a;
                _image.color = col;
                yield return null;
            }

            {
                var col = _image.color;
                col.a = targetAlpha;
                _image.color = col;
            }

            if (targetAlpha < 0.001f)
                _image.enabled = false;

            _highlightFadeRoutine = null;
        }
    }
}