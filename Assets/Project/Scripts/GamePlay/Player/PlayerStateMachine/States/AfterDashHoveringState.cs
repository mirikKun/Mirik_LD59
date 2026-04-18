using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class AfterDashHoveringState : IState
    {
        private readonly ActorEntity _player;
        private readonly DashMoveStateConfig _config;
        private readonly CountdownTimer _hoveringTimer;
        private PlayerMover Mover => _player.Get<PlayerMover>();

        public AfterDashHoveringState(ActorEntity playerEntity, DashMoveStateConfig config)
        {
            _player = playerEntity;
            _config = config;
            _hoveringTimer = new CountdownTimer(_config.AfterDashHoveringDuration);
        }

        public void OnEnter()
        {
            _hoveringTimer.Start();
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;

            verticalMomentum -= Mover.Tr.up * (_config.AfterDashHoveringGravity* fixedDeltaTime);

            horizontalMomentum += _player.Get<PlayerController>().GetInputMovementDirection() * (_config.AfterDashHoveringSpeed * fixedDeltaTime);
            horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, Mover.MovementSpeed);

            // float friction = _controller.AirFriction;
            // horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * fixedDeltaTime);
            momentum = horizontalMomentum + verticalMomentum;

            Mover.SetMomentum(momentum);
        }


        public bool HoveringToRising() => (_hoveringTimer.IsFinished) && Mover.IsRising();
        public bool HoveringToFalling() => (_hoveringTimer.IsFinished || _player.Get<CeilingDetector>().HitCeiling());
        public bool HoveringToGrounded() => Mover.IsGrounded();
    }
}