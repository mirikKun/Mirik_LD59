using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class GroundedState : IGroundState
    {
        private readonly ActorEntity _player;


        private bool _moving;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerEffects.PlayerEffects Effects => _player.Get<PlayerEffects.PlayerEffects>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        public GroundedState(ActorEntity player)
        {
            _player = player;
        }

        public void OnEnter()
        {
            _moving = false;
        }

        public void OnExit()
        {
            Effects.CameraMovingEffects.SetGrounded(false);
            Mover.SetParent(null);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;
            verticalMomentum -= Mover.Tr.up * (Mover.Gravity * fixedDeltaTime);


            if (VectorMath.GetDotProduct(verticalMomentum, Mover.Tr.up) < 0f)
            {
                verticalMomentum = Vector3.zero;
            }

            float friction = Mover.GroundFriction;
            horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * fixedDeltaTime);

            Vector3 velocity = Mover.CalculateMovementVelocity();
            horizontalMomentum = velocity.sqrMagnitude > 0 ? velocity : horizontalMomentum;

            momentum = horizontalMomentum + verticalMomentum;

            Mover.SetMomentum(momentum);


            Mover.SetParent(Mover.MovingObject?.Root);

        }

        public void Update(float deltaTime)
        {
            bool oldMoving = _moving;
            _moving = PlayerController.GetInputMovementDirection().magnitude > 0;
            if (_moving != oldMoving)
            {
                Effects.CameraMovingEffects.SetGrounded(_moving);
            }
        }

        public bool GroundedToRising() => Mover.IsRising();
        public bool GroundedToSliding() => Mover.IsRising() && Mover.IsGroundTooSteep();
        public bool GroundedToFalling() => !Mover.IsGrounded();
    }
}