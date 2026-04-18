using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class CrouchSlidingJumpState: IJumpState
    {
        private readonly ActorEntity _player;
        private readonly CrouchSlidingMoveStateConfig _config;
        private bool _jumpKeyIsPressed;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        public CrouchSlidingJumpState(ActorEntity player, CrouchSlidingMoveStateConfig config)
        {
            _player = player;
            _config = config;

            PlayerController.Input.Jump += HandleJumpKeyInput;
        }
        public void Dispose()
        {
            PlayerController.Input.Jump -= HandleJumpKeyInput;
        }

        private void HandleJumpKeyInput(bool isButtonPressed)
        {
            _jumpKeyIsPressed = isButtonPressed;
        }
        public void OnEnter()
        {
            Mover.OnGroundContactLost();
            OnJumpStart();
        }

        private void OnJumpStart()
        {
            Vector3 moveDirection = Mover.CalculateMovementVelocity();
            Vector3 cameraForward = PlayerController.CameraTrY.transform.forward;
            
            Vector3 direction =moveDirection.sqrMagnitude>0?moveDirection.normalized:cameraForward;
            Vector3 horizontalDirection = (direction - VectorMath.ExtractDotVector(direction, Mover.Tr.up)).normalized;


            Vector3 axis = Vector3.Cross(Mover.Tr.up, horizontalDirection);
            Quaternion jumpRotation = Quaternion.AngleAxis(-_config.PounceMinAngle, axis);

            Vector3 jumpDirection = jumpRotation * horizontalDirection;
            Vector3 newMomentum = jumpDirection * _config.PouncePower;

            Mover.SetMomentum(newMomentum);
            _jumpKeyIsPressed= false;
        }

        public bool SlidingToJump() => _jumpKeyIsPressed;
        public bool JumpToRising() => !_jumpKeyIsPressed;
        public bool JumpToFalling() => _player.Get<CeilingDetector>().HitCeiling();
    }
}