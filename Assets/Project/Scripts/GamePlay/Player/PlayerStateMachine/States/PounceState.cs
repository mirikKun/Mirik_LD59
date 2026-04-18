using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class PounceState : IJumpState
    {
        private readonly ActorEntity _player;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();
        private readonly PounceMoveStateConfig _config;
        private bool _jumpKeyIsPressed;



        public PounceState(ActorEntity player, PounceMoveStateConfig config, AbilityInstance abilitiesInstance)
        {
            _player = player;
            _config = config;
     
            abilitiesInstance.OnAbilityInput += HandleJumpKeyInput;
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
            OnPounceStart();
        }

        private void OnPounceStart()
        {
            var jumpDirection = GetJumpDirection();

            Vector3 momentum = jumpDirection * _config.PouncePower;

            Mover.SetMomentum(momentum);
            _jumpKeyIsPressed= false;
        }

        private Vector3 GetJumpDirection()
        {
            Vector3 cameraForward = PlayerController.CameraTrY.transform.forward;
            var horizontalDirection = GetHorizontalDirection(cameraForward);

            float cameraAngle = Vector3.Angle(Mover.Tr.up, cameraForward);
            float clampedAngle = Mathf.Max(90f - cameraAngle, _config.PounceMinAngle); 

            Vector3 axis = Vector3.Cross(Mover.Tr.up, horizontalDirection);
            Quaternion jumpRotation = Quaternion.AngleAxis(-clampedAngle, axis);

            Vector3 jumpDirection = jumpRotation * horizontalDirection;
            return jumpDirection;
        }

        
        private Vector3 GetHorizontalDirection(Vector3 cameraForward)
        {
            if(_config.EveryDirection&&PlayerController.GetInputMovementDirection().magnitude>float.Epsilon)
            {
                return Mover.GetMomentum().normalized;
            }
            else
            {
                Vector3 horizontalDirection = (cameraForward - VectorMath.ExtractDotVector(cameraForward, Mover.Tr.up));
                return horizontalDirection.normalized; 
            }
        }

        public bool GroundedToPounce() => _jumpKeyIsPressed;
        public bool PounceToRising() => !_jumpKeyIsPressed;
        public bool PounceToFalling() => _player.Get<CeilingDetector>().HitCeiling();
    }
}