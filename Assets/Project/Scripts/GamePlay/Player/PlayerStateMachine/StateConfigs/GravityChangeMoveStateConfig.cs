using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Gravity Change State Config", fileName = "GravityChangeStateConfig")]

    public class GravityChangeMoveStateConfig : BaseMoveStateConfig
    {
        [field: SerializeField] public float RaycastNearDistance { get; private set; } = 4.5f;
        [field: SerializeField] public float ChangingDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float GravityChangeFullDuration { get; private set; } = 3f;
        
        [field: SerializeField] public float GravityChangeSpeed { get; private set; } = 10f;
        
        [field: Space]
        [field: SerializeField] public bool GravityChangeJumpAvailable   { get; private set; }

        [field: SerializeField] public float GravityChangeJumpMaxHorizontalDistance { get; private set; } = 13f;
        [field: SerializeField] public float GravityChangeJumpMaxVerticalDistance { get; private set; } = 30f;
        [field:Header("Sounds")]
        [field: SerializeField] public SoundData Sound { get; private set; }
        
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetPounceConfiguration(playerEntity,abilitiesInstance),
                //GetGravityChangePreparingConfiguration(playerEntity)
            };
            return jumpStateConfigurations;
        }

        private StateConfiguration GetPounceConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var gravityChange = new GravityChangeState(playerEntity, this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = gravityChange,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<GroundedState, GravityChangeState>(gravityChange.CanGravityChange),
                    TransitionConfiguration.GetConfiguration<RisingState, GravityChangeState>(gravityChange.CanGravityChange),
                    TransitionConfiguration.GetConfiguration<GroundedState, GravityChangeState>(gravityChange.GravityChangeDurationEnded),
                    TransitionConfiguration.GetConfiguration<RisingState, GravityChangeState>(gravityChange.GravityChangeDurationEnded),
                    TransitionConfiguration.GetConfiguration<FallingState, GravityChangeState>(gravityChange.GravityChangeDurationEnded),
                    TransitionConfiguration.GetConfiguration<SlopeSlidingState, GravityChangeState>(gravityChange.GravityChangeDurationEnded),
                    TransitionConfiguration.GetConfiguration<GravityChangeState, GroundedState>(gravityChange.GravityChangeToGrounded),
                    TransitionConfiguration.GetConfiguration<GravityChangeState, FallingState>(gravityChange.GravityChangeToFalling)
                    
                    
                    
                }
            };
            return configuration;
        }
        private StateConfiguration GetGravityChangePreparingConfiguration(ActorEntity playerEntity)
        {
            var state = new GravityChangeJumpState(playerEntity, this);
            StateConfiguration configuration = new StateConfiguration
            {
                State = state,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<GroundedState, GravityChangeJumpState>(state.GroundedToGravityJumpChangePreparing),
                    TransitionConfiguration.GetConfiguration<GravityChangeJumpState, GroundedState>(state.GravityJumpChangePreparingToGrounded),
                }
            };
            return configuration;
        }
    }
}