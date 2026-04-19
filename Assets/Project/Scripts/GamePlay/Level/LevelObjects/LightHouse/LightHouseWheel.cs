
using System;
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

        public override void HighLight()
        {
            _image.enabled = true;
        }

        public override void UnHighLight()
        {
            _image.enabled = false;
        }
    }
}