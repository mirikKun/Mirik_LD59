using ImprovedTimers.Project.Scripts.Utils.Timers;

using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerResources;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates;
using Project.Scripts.Infrastructure.Sounds.Behaviours;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class DiveStrikeState : IState,IMoveStateWithCost
    {
        private readonly ActorEntity _player;
        private readonly DiveStrikeMoveStateConfig _config;
        private CountdownTimer _diveStrikeTimer;

        private Vector3 _diveDirection;
        private Vector3 _startPosition;

        private bool _actionKeyIsPressed;

        private PlayerMover Mover => _player.Get<PlayerMover>();

        public DiveStrikeState(ActorEntity player, DiveStrikeMoveStateConfig config, AbilityInstance abilitiesInstance)
        {
            abilitiesInstance.OnAbilityInput += HandleActionInput;

            _player = player;
            _config = config;
        }

        private void HandleActionInput(bool isButtonPressed)
        {
            _actionKeyIsPressed = isButtonPressed;
        }

        public void OnEnter()
        {
            _diveDirection = -Mover.Tr.up;
            _player.Get<PlayerManaController>().SpendMana(_config.ManaCost);

            Vector3 momentum = _diveDirection * _config.DiveStrikeSpeed;
            Mover.SetMomentum(momentum);
            _startPosition = Mover.Tr.position;
            _player.Get<SoundSource>().PlaySound(_config.StartSound);
        }

        public void OnExit()
        {
            float fallingHeight = _startPosition.y - Mover.Tr.position.y;
            _player.Get<PlayerEffects.PlayerEffects>().CameraMovingEffects.StartFallEffect(fallingHeight * 2);
            _player.Get<SoundSource>().PlaySound(_config.LandSound);

        }

    
        public bool FallingToDiveStrike() => CanDoDiveStrike;
        public bool RisingToDiveStrike() => CanDoDiveStrike;
        public bool DiveStrikeToGrounded() => Mover.IsGrounded();
        public bool CanPayCost =>_player.Get<PlayerManaController>().CurrentMana >= _config.ManaCost;
        private bool CanDoDiveStrike=>_actionKeyIsPressed&&CanPayCost;
    }
}