using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Physic.ColliderLogic
{
    public interface ITriggerHittable
    {
        public Vector3 GetPosition();
        public void OnHit(IAttackTrigger attackTrigger);
    }
}