using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Core.Physic.Raycast;
using Project.Scripts.GamePlay.Levels;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Ships
{
    /// <summary>
    /// Slides this transform on XZ toward <see cref="ILevelDataProvider.LighthouseTarget"/>,
    /// stopping when <see cref="RaycastSensor"/> detects an obstacle along the next step.
    /// </summary>
    public class LootPlanarObstacleMovement : MonoBehaviour, IGameUpdateable
    {
        [SerializeField] private float _moveSpeed = 2.5f;
        [SerializeField] private float _arrivalPlanarDistance = 1.5f;
        [SerializeField] private float _skinWidth = 0.08f;
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private LayerMask _obstacleMask;

        private IUpdateService _updateService;
        private ILevelDataProvider _levelDataProvider;
        private RaycastSensor _raycastSensor;
        private bool _drifting;
        private bool _registeredUpdate;

        [Inject]
        private void Construct(IUpdateService updateService, ILevelDataProvider levelDataProvider)
        {
            _updateService = updateService;
            _levelDataProvider = levelDataProvider;
        }

        private void OnDestroy()
        {
            StopInternal();
        }

        public void BeginAt(Vector3 worldPosition)
        {
            StopInternal();
            transform.position = worldPosition;
            var origin = _raycastOrigin != null ? _raycastOrigin : transform;
            _raycastSensor = new RaycastSensor(origin);
            _raycastSensor.Layermask = _obstacleMask;
            _drifting = true;
            _updateService.EnemiesUpdate.Register(this);
            _registeredUpdate = true;
        }

        private void StopInternal()
        {
            if (!_registeredUpdate)
                return;
            _updateService.EnemiesUpdate.Unregister(this);
            _registeredUpdate = false;
            _drifting = false;
        }

        public void GameUpdate(float deltaTime)
        {
            if (!_drifting)
                return;

            var lighthouse = _levelDataProvider.LighthouseTarget;
            if (lighthouse == null)
                return;

            var planarDelta = lighthouse.position - transform.position;
            planarDelta.y = 0f;
            if (planarDelta.sqrMagnitude <= _arrivalPlanarDistance * _arrivalPlanarDistance)
            {
                StopInternal();
                return;
            }

            var dir = planarDelta.normalized;
            var step = _moveSpeed * deltaTime;
            var castLength = step + _skinWidth;
            _raycastSensor.CastLength = castLength;
            if (_raycastSensor.CastAndCheck(dir, castLength))
                return;

            transform.position += dir * step;
        }
    }
}
