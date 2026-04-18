using System;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Utils.Components
{
    public class PositionFollower : MonoBehaviour, IGameUpdateable
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3 _localOffset;
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
            _transform.position = _target.position;
            _transform.localPosition += _localOffset;
        }
    }
}