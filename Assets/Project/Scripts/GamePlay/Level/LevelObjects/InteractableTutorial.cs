using System;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class InteractableTutorial:MonoBehaviour
    {
        [SerializeField] private Transform _rotator;
        private Transform _target;
        public void SetupTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            _rotator.LookAt(_target.position);
        }
    }
}