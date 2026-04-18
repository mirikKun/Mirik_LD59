using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    [SelectionBase]
    public class MovingDoor : MonoBehaviour
    {
        [SerializeField] private Transform _door;
        [SerializeField] private Transform _targetToMove;
        [SerializeField] private float _moveSpeed = 1f;
        private Vector3 _startPosition;
        private bool _moving;


        protected virtual void Update()
        {
            if (_moving)
            {
                _door.position =
                    Vector3.MoveTowards(_door.position, _targetToMove.position, _moveSpeed * Time.deltaTime);
                if (Vector3.Distance(_door.position, _targetToMove.position) <= 0.1f)
                {
                    _moving = false;
                }
            }
        }

        public void StartDoorMovement()
        {
            _moving = true;
        }
    }
}