using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class FallingState : BaseAirState
    {
        private Vector3 _fallStartPosition;

        public FallingState(ActorEntity player) : base(player)
        {
        }

        public override void OnEnter()
        {
            _fallStartPosition = Mover.Tr.position;
            Vector3 momentum = Mover.GetMomentum();
            momentum = VectorMath.RemoveDotVector(momentum, Mover.Tr.up);

            Mover.SetMomentum(momentum);
        }

        public override void OnExit()
        {
            Vector3 fallingDistance = _fallStartPosition - Mover.Tr.position;
            float fallingHeight = Vector3.Dot(fallingDistance, Mover.Tr.up);
            if (FallingToGrounded())
            {
                _player.Get<PlayerEffects.PlayerEffects>().CameraMovingEffects.StartFallEffect(fallingHeight);
            }
        }

        public override void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;
            verticalMomentum -= Mover.Tr.up * (Mover.Gravity * fixedDeltaTime);

            horizontalMomentum =
                AdjustHorizontalAirMomentum(fixedDeltaTime, horizontalMomentum, Mover.CalculateMovementVelocity());

            float friction = Mover.AirFriction;
            horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * fixedDeltaTime);
            momentum = horizontalMomentum + verticalMomentum;

            Mover.SetMomentum(momentum);
        }

        public bool FallingToRising() => Mover.IsRising();
        public bool FallingToGrounded() => Mover.IsGrounded() && !Mover.IsGroundTooSteep();
        public bool FallingToSliding() => Mover.IsGrounded() && Mover.IsGroundTooSteep();
    }
}