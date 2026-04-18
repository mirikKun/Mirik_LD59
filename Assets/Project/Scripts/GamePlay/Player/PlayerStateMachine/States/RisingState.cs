using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class RisingState : BaseAirState
    {
        public RisingState(ActorEntity player) : base(player)
        {
        }

        public override void OnEnter()
        {
            Mover.OnGroundContactLost();
        }

        public override void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;
            verticalMomentum -= Mover.Tr.up * (Mover.Gravity * fixedDeltaTime);

            horizontalMomentum =
                AdjustHorizontalAirMomentum(fixedDeltaTime,horizontalMomentum, Mover.CalculateMovementVelocity());


            float friction = Mover.AirFriction;
            horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * fixedDeltaTime);
            momentum = horizontalMomentum + verticalMomentum;

            Mover.SetMomentum(momentum);
        }

        public bool RisingToGrounded() => Mover.IsGrounded()&&!Mover.IsGroundTooSteep()&&!Mover.IsRising();
        public bool GroundToSliding() =>  Mover.IsGrounded()&&Mover.IsGroundTooSteep();
        public bool RisingToFalling() => Mover.IsFalling()||_player.Get<CeilingDetector>().HitCeiling();

    }
}