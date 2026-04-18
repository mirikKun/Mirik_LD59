using Project.Scripts.GamePlay.Core.Physic.Enums;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Physic.Raycast
{
    public class RaycastSensor
    {
        public float CastLength = 1f;
        public LayerMask Layermask = 255;

        private Vector3 _origin = Vector3.zero;
        private Transform _tr;

        private CastDirection _castDirection;
        private RaycastHit _hitInfo;

        public RaycastSensor(Transform playerTransform)
        {
            _tr = playerTransform;
        }

        public void Cast()
        {
            Vector3 worldDirection = GetCastDirection();
            Cast(worldDirection);
        }
        public void Cast(Vector3 direction)
        {
            Vector3 worldOrigin = _tr.TransformPoint(_origin);

            Physics.Raycast(worldOrigin, direction, out _hitInfo, CastLength, Layermask,
                QueryTriggerInteraction.Ignore);
        }
  

        public bool SeOriginCastAndCheck(Vector3 newOrigin)
        {
            SetCastOrigin(newOrigin);
            Cast();
            return HasDetectedHit();
        }
        public bool CastAndCheck(Vector3 direction)
        {
            Cast(direction);
            return HasDetectedHit();
        }
        public bool CastAndCheck(Vector3 direction,float castLength)
        {
            CastLength = castLength;
            Cast(direction);
            return HasDetectedHit();
        }
        public bool CastAndCheck()
        {
            Cast();
            return HasDetectedHit();
        }

        public bool HasDetectedHit() => _hitInfo.collider != null;
        public float GetDistance() => _hitInfo.distance;
        public Vector3 GetNormal() => _hitInfo.normal;

        public Quaternion GetNormalRotation() =>
            Quaternion.LookRotation(Vector3.ProjectOnPlane(GetCastDirection(), _hitInfo.normal).normalized,
                _hitInfo.normal);

        public Vector3 GetPosition() => _hitInfo.point;
        public Collider GetCollider() => _hitInfo.collider;
        public Transform GetTransform() => _hitInfo.transform;

        public void SetCastDirection(CastDirection direction) => _castDirection = direction;
        public void SetCastOrigin(Vector3 pos) => _origin = _tr.InverseTransformPoint(pos);

        public bool CastInDirection(CastDirection direction)
        {
            SetCastDirection(direction);
            Cast();
            return HasDetectedHit();
        }
       

        private Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Forward => _tr.forward,
                CastDirection.Right => _tr.right,
                CastDirection.Up => _tr.up,
                CastDirection.Backward => -_tr.forward,
                CastDirection.Left => -_tr.right,
                CastDirection.Down => -_tr.up,
                _ => Vector3.one
            };
        }

        public void DrawDebug(bool simple = true)
        {
            Debug.DrawLine(_tr.TransformPoint(_origin),_tr.TransformPoint(_origin)+GetCastDirection(),Color.deepPink);
            if (!HasDetectedHit()) return;

            if (simple)
                Debug.DrawRay(_hitInfo.point, _hitInfo.normal, Color.red, UnityEngine.Time.deltaTime);
            else
                Debug.DrawLine(_hitInfo.point, _hitInfo.point - GetCastDirection() * _hitInfo.distance, Color.red, 20);

            float markerSize = 0.2f;
            Debug.DrawLine(_hitInfo.point + Vector3.up * markerSize, _hitInfo.point - Vector3.up * markerSize,
                Color.green, UnityEngine.Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.right * markerSize, _hitInfo.point - Vector3.right * markerSize,
                Color.green, UnityEngine.Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.forward * markerSize, _hitInfo.point - Vector3.forward * markerSize,
                Color.green, UnityEngine.Time.deltaTime);
        }
    }
}