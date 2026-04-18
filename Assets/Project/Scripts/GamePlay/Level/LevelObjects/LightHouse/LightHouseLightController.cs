using System;
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

        [SerializeField] [Range(1, 70)] private float _strenght;


        private void OnValidate()
        {
            SetLightStrength();
        }

        private void SetLightStrength()
        {
            _spotLight.intensity = _baseLightIntensity * _strenght;
            _spotLight.range = _baseLightLenght * _strenght;
            _coneTransform.localScale = _baseConeScale * _strenght;
        }
    }
}