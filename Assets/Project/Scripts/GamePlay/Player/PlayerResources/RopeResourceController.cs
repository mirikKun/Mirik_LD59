using System;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.PlayerStateMachine;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{

    public class RopeResourceController : EntityResourceController
    {
        [SerializeField] private int _maxRopes;
        private int _airHookCharges;
        private bool _wasGrounded;

        public event Action<int, int> RopeChargesChanged;

        public event Action<float> SwingProgressChanged;
        public event Action<bool> CanGrapple;

        public int MaxRopes => _maxRopes;
        public int AirHookChargesRemaining => _airHookCharges;

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity);
            _airHookCharges = Mathf.Max(0, _maxRopes);
        }

        public override void Tick(float deltaTime)
        {
            if (Entity == null || !Entity.TryGet(out PlayerStateMachineContainer sm))
                return;

            bool grounded = sm.IsGroundedState();
            if (grounded && !_wasGrounded)
                RestoreAllSpent();
            _wasGrounded = grounded;
        }

        public void AddOneRopeTowardMax()
        {
            _maxRopes++;
            if (Entity != null && Entity.TryGet(out PlayerStateMachineContainer sm) && sm.IsGroundedState())
                _airHookCharges = _maxRopes;
            RaiseRopeChargesChanged();
        }

        public bool HasAirHookCharge() => _airHookCharges > 0;

        public bool TrySpendAirHookCharge()
        {
            if (_airHookCharges <= 0)
                return false;
            _airHookCharges--;
            RaiseRopeChargesChanged();
            return true;
        }

        public void SetRopeProgress(float progress)
        {
            SwingProgressChanged?.Invoke(Mathf.Clamp01(progress));
        }

        public void SetCanGrapple(bool canGrapple)
        {
            CanGrapple?.Invoke(canGrapple);
        }

        public void RestoreAllSpent()
        {
            int next = Mathf.Max(0, _maxRopes);
            if (_airHookCharges == next)
                return;
            _airHookCharges = next;
            RaiseRopeChargesChanged();
        }

        private void RaiseRopeChargesChanged() =>
            RopeChargesChanged?.Invoke(_maxRopes, _airHookCharges);
    }
}
