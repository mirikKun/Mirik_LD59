using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Core.Physic.Enums;
using Project.Scripts.GamePlay.Core.Physic.Raycast;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class GrapplingHookState:IState
    {
        private readonly ActorEntity _player;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();

        private readonly GrapplingHookMoveStateConfig _config;
        private RaycastSensor _raycastSensor;
        private  CountdownTimer _grapplingTimer;

        private Vector3 _grapplePoint;
        private Vector3 _grappleDirection;
        private bool _actionKeyIsPressed;


        public GrapplingHookState(ActorEntity player, GrapplingHookMoveStateConfig config,
            AbilityInstance abilitiesInstance)
        {
            _player = player;
            _config = config;
            
            abilitiesInstance.OnAbilityInput += HandleActionInput;
            _raycastSensor = new RaycastSensor(PlayerController.CameraTrY);
            _raycastSensor.CastLength=(_config.GrappleMaxDistance);
            _raycastSensor.SetCastDirection(CastDirection.Forward);
        }
        public void Dispose()
        {
        }

        private void HandleActionInput(bool isButtonPressed)
        {
            _actionKeyIsPressed = isButtonPressed;
        }

        public  void OnEnter() {
            Mover.OnGroundContactLost();
            OnGrapplingHookStart();
            


        }

        public void OnExit()
        {
            Mover.SetMomentum(Mover.GetMomentum()*_config.GrapplingExitSpeedMultiplier);
            _player.Get<PlayerEffects.PlayerEffects>().HookEffects.ClearGrappleLine();

        }

        private void OnGrapplingHookStart()
        {
            _grappleDirection = PlayerController.CameraTrY.forward;
            _grapplePoint = _raycastSensor.GetPosition();
            float distance = Vector3.Distance(Mover.Tr.position, _grapplePoint);

            float grappleDuration = (Mathf.Clamp(distance,_config.GrappleMinDistance, _config.GrappleMaxApproachableDistance )-_config.AdaptiveGrappleLetGoDistance) / _config.GrappleSpeed;
            _grapplingTimer = new CountdownTimer(grappleDuration);
            _grapplingTimer.Start();

            Vector3 momentum = _grappleDirection*_config.GrappleSpeed;
            Mover.SetMomentum(momentum);
        }
        public void Update(float deltaTime)
        {
            _player.Get<PlayerEffects.PlayerEffects>().HookEffects.DrawGrappleLine(_grapplePoint, 1);
        }
        private bool CanGrapple => _raycastSensor.SeOriginCastAndCheck(PlayerController.CameraTrY.position) && _raycastSensor.GetDistance() > _config.GrappleMinDistance;


        public bool GroundedToGrappleHook()=>_actionKeyIsPressed&&CanGrapple;
        public bool AirToGrappleHook()=>_actionKeyIsPressed&&CanGrapple&&!_player.Get<PlayerStateMachineContainer>().HaveStateBeforeStateInHistory<GrapplingHookState,IGroundState>();
        public bool GrappleHookToRising() => _grapplingTimer.IsFinished&&Mover.IsRising();
        public bool GrappleHookToFalling() => _grapplingTimer.IsFinished&&Mover.IsFalling();
    }
}