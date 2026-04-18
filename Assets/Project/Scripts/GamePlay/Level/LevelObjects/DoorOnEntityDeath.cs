using System;
using Project.Scripts.GamePlay.Common.Health;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class DoorOnEntityDeath:MonoBehaviour
    {
        [SerializeField] private ActorEntity _entity;
        [SerializeField] private Transform _door;
        [SerializeField] private Transform _targetToMove;
        [SerializeField] private float _moveSpeed = 1f;
        
        [SerializeField] private LineRenderer _lineRenderer;
        private Vector3 _startPosition;
        private bool _entityDied;
        private bool _reachedTarget;

        private void Start()
        {
            _entity.Get<IHealth>().Died+=OnEntityDied;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _entity.GetPosition());
        }

        private void Update()
        {
            if (_reachedTarget)
            {
                return;
            }
            else if(_entityDied)
            {
                _door.position=Vector3.MoveTowards(_door.position, _targetToMove.position, _moveSpeed * Time.deltaTime);
                if (Vector3.Distance(_door.position, _targetToMove.position) <= 0.1f)
                {
                    _reachedTarget=true;
                }
            }
            else
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _entity.GetPosition());
            }
        }

        private void OnEntityDied(BaseEntity obj)
        {
            _entityDied=true;
            _lineRenderer.enabled = false;
        }
    }
}