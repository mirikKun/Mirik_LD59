using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Core.Physic.Enums;
using Project.Scripts.GamePlay.Core.Physic.Raycast;
using Project.Scripts.GamePlay.Player.Abilities.AbilityTypes;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.Indication;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.Infrastructure.Sounds.Behaviours;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class GravityChangeState : IState
    {
        private readonly ActorEntity _player;
        private readonly GravityChangeMoveStateConfig _config;
        private readonly RaycastSensor _raycastNearSensor;
        private readonly RaycastSensor _raycastFarSensor;
        private readonly CountdownTimer _gravityChangeTimer;
        private CountdownTimer _gravityFullChangeTimer;

        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();


        private Vector3 _gravityDirection;
        private Vector3 _lastGravityDirection;

        private Quaternion _startRotation;
        private Quaternion _changeRotation;

        private bool _actionKeyIsPressed;
        private float _angleTreashold = 0.01f;

        private bool _actionKeyPressedDown;
        private bool _actionKeyPressedUp;
        private bool _wrongGravity;


        public GravityChangeState(ActorEntity player, GravityChangeMoveStateConfig config,
            AbilityInstance abilitiesInstance)
        {
            _player = player;
            _config = config;

            _gravityChangeTimer = new CountdownTimer(_config.ChangingDuration);
            _gravityFullChangeTimer = new CountdownTimer(_config.GravityChangeFullDuration);

            abilitiesInstance.OnAbilityInput += HandleActionInput;

            _raycastNearSensor = new RaycastSensor(PlayerController.CameraTrY);
            _raycastNearSensor.CastLength = (_config.RaycastNearDistance);
            _raycastNearSensor.SetCastDirection(CastDirection.Forward);

            _raycastFarSensor = new RaycastSensor(PlayerController.CameraTrY);
            _raycastFarSensor.CastLength = (_config.GravityChangeJumpMaxVerticalDistance +
                                            _config.GravityChangeJumpMaxHorizontalDistance);
            _raycastFarSensor.SetCastDirection(CastDirection.Forward);
            _player.Get<AbilitiesIndicationController>().RangeIndication
                .EquipAbilityWithRange(RangeIndicationType.GravityChange, _raycastNearSensor);
        }

        public void Dispose()
        {
            //_controller.Input.Action1 -= HandleActionInput;
            _player.Get<AbilitiesIndicationController>().RangeIndication
                .UnequipAbilityWithRange(RangeIndicationType.GravityChange);
        }

        private void HandleActionInput(bool isButtonPressed)
        {
            _actionKeyPressedUp = false;
            _actionKeyPressedDown = false;


            if (_actionKeyIsPressed && !isButtonPressed)
            {
                _actionKeyPressedUp = true;
            }
            else if (!_actionKeyIsPressed && isButtonPressed)
            {
                _actionKeyPressedDown = true;
            }

            _actionKeyIsPressed = isButtonPressed;
        }

        public void OnEnter()
        {
            Mover.SetMomentum(Vector3.zero);
            _player.Get<SoundSource>().PlaySound(_config.Sound);

            _startRotation = Mover.Tr.rotation;
            _changeRotation = Quaternion.FromToRotation(Mover.Tr.up, _raycastNearSensor.GetNormal());
            _gravityChangeTimer.Start();
            _actionKeyIsPressed = false;


            if (!_wrongGravity || (_actionKeyIsPressed))
            {
                _gravityFullChangeTimer.Start();

                _wrongGravity = true;


                Mover.SetMomentum(Vector3.zero);
                _lastGravityDirection = Mover.Tr.up;
                _startRotation = Mover.Tr.rotation;
                _changeRotation = FromToRotation(Mover.Tr.up, _raycastNearSensor.GetNormal());
                _gravityChangeTimer.Start();
                _actionKeyIsPressed = false;
            }
            else
            {

                _wrongGravity = false;
                Mover.SetMomentum(Vector3.zero);
                _lastGravityDirection = Vector3.up;
                _startRotation = Mover.Tr.rotation;
                _changeRotation = FromToRotation(Mover.Tr.up, _lastGravityDirection);
                _gravityChangeTimer.Start();
            }
        }

        private Quaternion FromToRotation(Vector3 aFrom, Vector3 aTo)
        {
            float angle = Vector3.Angle(aFrom, aTo);
            Vector3 axis = Vector3.Cross(aFrom, aTo);
            if (Mathf.Approximately(angle, 180f))
            {
                axis = -PlayerController.CameraTrX.right;
            }
            else if (Vector3.Dot(axis, PlayerController.CameraTrX.right) > 0 && angle > 135)
            {
                axis = -axis;
                angle = 360 - angle;
            }


            return Quaternion.AngleAxis(angle / 2, axis);
        }

        public void OnExit()
        {
            Mover.Tr.rotation = _changeRotation * _changeRotation * _startRotation;
            _actionKeyPressedUp = false;
            _actionKeyPressedDown = false;
        }

        public void Update(float deltaTime)
        {
            float progress = 1 - _gravityChangeTimer.Progress;
            Quaternion from = progress < 0.5f ? _startRotation : _changeRotation * _startRotation;
            Quaternion to = progress < 0.5f
                ? _changeRotation * _startRotation
                : _changeRotation * _changeRotation * _startRotation;
            if (progress < 0.5f)
                Mover.Tr.rotation = Quaternion.Lerp(from, to, progress * 2);
            else
                Mover.Tr.rotation = Quaternion.Lerp(from, to, (progress - 0.5f) * 2);
        }


        private bool IsActionPressed()
        {
            bool actionKeyPressedUp = _actionKeyPressedUp;

            _actionKeyPressedUp = false;
            return actionKeyPressedUp;
        }

        public bool CanGravityChange() => IsActionPressed() &&
                                          (_wrongGravity ||
                                           _raycastNearSensor.SeOriginCastAndCheck(PlayerController.CameraTrY.position) &&
                                           Vector3.Angle(_raycastNearSensor.GetNormal(), Mover.Tr.up) >
                                           _angleTreashold);

        public bool GravityChangeDurationEnded() => _wrongGravity && _gravityFullChangeTimer.IsFinished;

        public bool GravityChangeToGrounded() => _gravityChangeTimer.IsFinished && Mover.IsGrounded();
        public bool GravityChangeToFalling() => _gravityChangeTimer.IsFinished;
    }
}