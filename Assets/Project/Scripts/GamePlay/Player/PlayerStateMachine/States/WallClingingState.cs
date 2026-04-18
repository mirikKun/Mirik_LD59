using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Controller;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class WallClingingState:IState
    {
        private readonly ActorEntity _player;
        private bool _jumpKeyIsPressed;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private WallDetector WallDetector => _player.Get<WallDetector>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        public WallClingingState(ActorEntity player)
        {
            _player = player;
        
            PlayerController.Input.Jump += HandleJumpKeyInput;
        }
        public void Dispose()
        {
            PlayerController.Input.Jump -= HandleJumpKeyInput;
        }

        public void OnEnter()
        {
            Mover.OnGroundContactLost();
            Mover.SetMomentum(Vector3.zero);
            Mover.SetVelocity(Vector3.zero);
            _jumpKeyIsPressed= false;

        }

        public void OnExit()
        {
            _jumpKeyIsPressed= false;
        }

        private void HandleJumpKeyInput(bool isButtonPressed)
        {
            _jumpKeyIsPressed = isButtonPressed;
        }
        public bool FallingToClinging() => _jumpKeyIsPressed&&WallDetector.HitForwardWall();
        public bool RisingToClinging() => _jumpKeyIsPressed&&WallDetector.HitForwardWall();
        public bool ClingingToPounce() => _jumpKeyIsPressed&&!WallDetector.HitForwardWall();
        public bool ClingingToFalling() => _jumpKeyIsPressed;
    }
}