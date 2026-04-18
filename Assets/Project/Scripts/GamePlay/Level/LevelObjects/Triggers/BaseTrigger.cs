using System;
using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Triggers
{
    public class BaseTrigger : MonoBehaviour
    {
        public event Action Triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity player))
            {
                OnTriggered();
            }
        }

        protected void OnTriggered()
        {
            Triggered?.Invoke();
        }
    }
}