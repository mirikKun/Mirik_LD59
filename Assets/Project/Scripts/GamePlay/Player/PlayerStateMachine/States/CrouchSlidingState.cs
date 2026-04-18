using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class CrouchSlidingState : IState
    {
        private readonly ActorEntity _player;
        private readonly CrouchSlidingMoveStateConfig _config;
        private bool _crouchKeyIsPressed;
        private readonly CountdownTimer _slideMaxSpeedTimer;
        private PlayerMover Mover => _player.Get<PlayerMover>();


        private Vector3 _slideDirection;
        private bool _firstTimerFinished;

        public CrouchSlidingState(ActorEntity player,  CrouchSlidingMoveStateConfig config,AbilityInstance abilitiesInstance)
        {
            _player = player;
            _config = config;


            abilitiesInstance.OnAbilityInput += HandleCrouchKeyInput;
            _slideMaxSpeedTimer = new CountdownTimer(_config.SlideMaxSpeedDuration);
            //_slideMaxSpeedTimer.se
        }
        private void HandleCrouchKeyInput(bool isButtonPressed)
        {
            _crouchKeyIsPressed = isButtonPressed;
        }

        public void OnEnter()
        {
            _slideMaxSpeedTimer.Start();
            Mover.StartCrouching(_config.ColliderHeight);
            Vector3 momentum = Mover.GetMomentum();
            _slideDirection = Vector3.ProjectOnPlane(momentum, Mover.GetGroundNormal()).normalized;
            Mover.SetMomentum(_slideDirection * _config.SlideSpeed);
            _firstTimerFinished = false;
        }

        public void OnExit()
        {
            Mover.StopCrouching();
        }


        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 momentum = Mover.GetMomentum();
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, Mover.Tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;
            verticalMomentum -= Mover.Tr.up * (Mover.Gravity * fixedDeltaTime);
            momentum = horizontalMomentum + verticalMomentum;


            momentum = Vector3.ProjectOnPlane(momentum, Mover.GetGroundNormal());
            if (VectorMath.GetDotProduct(momentum, Mover.Tr.up) > 0f)
            {
                momentum = VectorMath.RemoveDotVector(momentum, Mover.Tr.up);
            }

            Vector3 slopeDirection = Vector3.ProjectOnPlane(-Mover.Tr.up, Mover.GetGroundNormal())
                .normalized;
            float slideOppositeAngle = Vector3.Angle(slopeDirection, momentum.normalized);
            bool onSlope = Vector3.Angle(Mover.Tr.up, Mover.GetGroundNormal()) > 0.1f;
            if (onSlope && slideOppositeAngle < _config.MinSlideAngle)
            {
                if(_firstTimerFinished||_slideMaxSpeedTimer.IsFinished)
                {
                    _firstTimerFinished = true;
                    _slideMaxSpeedTimer.Start();
                    momentum = Vector3.MoveTowards(momentum, slopeDirection * _config.SlideSpeed,
                        _config.SlidingFriction * fixedDeltaTime);
                }
            }
            else
             if (_slideMaxSpeedTimer.IsFinished)
             {
                 _firstTimerFinished = true;

                momentum = Vector3.MoveTowards(momentum, Vector3.zero, _config.SlidingFriction * fixedDeltaTime);
            }
            
            Mover.SetMomentum(momentum);
        }

        public bool CrouchSlidingToGround() => (!_crouchKeyIsPressed&&_firstTimerFinished)|| (Mover.GetMomentum().magnitude < _config.MinSlideSpeed && !_crouchKeyIsPressed) ;

        public bool CrouchSlidingToFalling() => !Mover.IsGrounded()&&Mover.IsFalling();
        public bool GroundedToCrouchSliding() => _crouchKeyIsPressed;
    }
}