using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class SlopeSlidingState : IGroundState 
    {
        private readonly ActorEntity _player;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        public SlopeSlidingState(ActorEntity player) 
        {
            _player = player;
        }

        public void OnEnter() 
        {
            Mover.OnGroundContactLost();
        }
        public void FixedUpdate(float fixedDeltaTime) 
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;
            verticalMomentum -= Mover.Tr.up * (Mover.Gravity * fixedDeltaTime);
            horizontalMomentum = GetHorizontalMomentum(horizontalMomentum,fixedDeltaTime);
            
            
            float friction = Mover.AirFriction;
            horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * fixedDeltaTime);
            momentum = horizontalMomentum + verticalMomentum;
            
            
            momentum = Vector3.ProjectOnPlane(momentum, Mover.GetGroundNormal());
            if (VectorMath.GetDotProduct(momentum, Mover.Tr.up) > 0f) {
                momentum = VectorMath.RemoveDotVector(momentum, Mover.Tr.up);
            }

            Vector3 slideDirection = Vector3.ProjectOnPlane(-Mover.Tr.up, Mover.GetGroundNormal()).normalized;
            momentum += slideDirection * (Mover.SlideGravity * fixedDeltaTime);

            Mover.SetMomentum(momentum);
        }

        private Vector3 GetHorizontalMomentum( Vector3 horizontalMomentum,float fixedDeltaTime) 
        {
            Vector3 pointDownVector = Vector3.ProjectOnPlane(Mover.GetGroundNormal(), Mover.Tr.up).normalized;
            Vector3 movementVelocity = Mover.CalculateMovementVelocity();
            movementVelocity = VectorMath.RemoveDotVector(movementVelocity, pointDownVector);
            horizontalMomentum += movementVelocity * fixedDeltaTime;
            return horizontalMomentum;
        }
        public bool SlidingToRising() => Mover.IsRising();
        public bool SlidingToFalling() => !Mover.IsGrounded();
        public bool SlidingToGround() => Mover.IsGrounded() && !Mover.IsGroundTooSteep();

    }
}