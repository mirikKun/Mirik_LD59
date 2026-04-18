using System;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Utils.Components
{
    public class Rotator:MonoBehaviour,IGameUpdateable
    {
        [SerializeField] private Vector3 _axis;
        [SerializeField] private float _speed;
        private IUpdateService _updateService;

        [Inject]
        private void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        private void Start()
        {
            _updateService.EffectsUpdate.Register(this);
        }
        private void OnDestroy()
        {
            _updateService.EffectsUpdate.Unregister(this);
        }

        public void GameUpdate(float deltaTime)
        {
            transform.Rotate(_axis,_speed*deltaTime);
        }
    }
}