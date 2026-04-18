using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class WallRunJumpState:IJumpState
    {
        private readonly ActorEntity _player;
        private readonly WallRunMoveStateConfig _config;
        private bool _jumpKeyIsPressed;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        public WallRunJumpState(ActorEntity player, WallRunMoveStateConfig config)
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
            Vector3 wallNormal = _player.Get<WallDetector>().GetWallNormal();
            Vector3 movingDirection = Mover.GetMomentum().normalized;
            Vector3 upDirection = Mover.Tr.up;
            
            Vector3 momentum = wallNormal * _config.JumpFromWallPower + movingDirection * _config.JumpForwardPower + upDirection * _config.JumpUpPower;

            Mover.SetMomentum(momentum);
            _jumpKeyIsPressed= false;        
        }
        public bool WallRunningToWallRunJump() => _jumpKeyIsPressed;
        public bool WallRunJumpToRising() => !_jumpKeyIsPressed;
        public bool WallRunJumpToFalling() => _player.Get<CeilingDetector>().HitCeiling();

    }
}