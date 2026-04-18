using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Infrastructure.Sounds.Behaviours;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class DashState:IState
    {
        protected readonly ActorEntity _player;
        private readonly DashBaseStateConfig _config;

        private Vector3 _dashDirection;
        private readonly CountdownTimer _dashTimer;
        private bool _keyIsPressed;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();
        private PlayerEffects.PlayerEffects Effects => _player.Get<PlayerEffects.PlayerEffects>();
        private SoundSource SoundSource => _player.Get<SoundSource>();


        public DashState(ActorEntity player, DashBaseStateConfig config, AbilityInstance abilitiesInstance)
        {
            _player = player;
            _config = config;
            _dashTimer = new CountdownTimer(_config.DashDuration);
            abilitiesInstance.OnAbilityInput += HandleKeyInput;
        }

        private void HandleKeyInput(bool isButtonPressed)
        {
            _keyIsPressed = isButtonPressed;
        }

        public virtual void OnEnter()
        {
            Mover.OnGroundContactLost();
            
            _dashDirection = 
                PlayerController.GetInputMovementDirection().magnitude>0?
                Vector3.ProjectOnPlane(PlayerController.GetInputMovementDirection(), PlayerController.CameraTrY.up).normalized:
                PlayerController.CameraTrY.forward;
            Mover.SetMomentum(Vector3.zero);
            _dashTimer.Start();
            _keyIsPressed = false;
            
            Effects.CameraMovingEffects.SetTargetFOV(_config.UpdatedFov);

            Effects.TimeSlowEffect.PlayCurve();
            SoundSource.PlaySound(_config.Sound);
            
        }

        public virtual void OnExit()
        {
            Mover.SetMomentum(_dashDirection * _config.DashExitSpeed);
            Effects.CameraMovingEffects.ResetFOV();

        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            Mover.SetMomentum(_dashDirection * _config.DashSpeed);
            
            
        }

        public bool GroundToDash() => CanDash();

        public bool AirToToDash<State>() where State : DashState
        {
            return CanDash() && _player.Get<PlayerStateMachineContainer>()
                .HaveStateBeforeStateInHistory<IGroundState,State>();
        }

        // public bool DashToRising() => (_dashTimer.IsFinished )&&_controller.IsRising();
        // public bool DashToFalling() => (_dashTimer.IsFinished || _controller.HitCeiling());
        
        public bool EndOfDash() => (_dashTimer.IsFinished );
        public bool WallClingingToDash() => CanDash();
        protected virtual bool CanDash() => _keyIsPressed;


    }
    public class DashLongState : DashState
    {
        public DashLongState(ActorEntity player, DashBaseStateConfig config, AbilityInstance abilitiesInstance) : base(player, config, abilitiesInstance){ }
    }
    public class DashEvadeState : DashState
    {
        public DashEvadeState(ActorEntity player, DashBaseStateConfig config, AbilityInstance abilitiesInstance) : base(player, config, abilitiesInstance){ }
    }
}