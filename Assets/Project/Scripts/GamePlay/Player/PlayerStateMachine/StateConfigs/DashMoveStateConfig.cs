using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Dash State Config", fileName = "DashStateConfig")]
    public class DashMoveStateConfig : DashBaseStateConfig
    {

        [field: Space] 
        [field: SerializeField] public float AfterDashHoveringDuration { get; private set; } = 0.67f;
        [field: SerializeField] public float AfterDashHoveringGravity { get; private set; } = 9;
        [field: SerializeField] public float AfterDashHoveringSpeed { get; private set; } = 19;

        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetDashConfiguration(playerEntity,abilitiesInstance),
                GetAfterDashHoveringConfiguration(playerEntity)
            };
            return jumpStateConfigurations;
        }

        protected override StateConfiguration GetDashConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var dash = new DashLongState(playerEntity, this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = dash,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<RisingState, DashLongState>(dash.AirToToDash<DashLongState>),
                    TransitionConfiguration.GetConfiguration<FallingState, DashLongState>(dash.AirToToDash<DashLongState>),
                    TransitionConfiguration.GetConfiguration<DashLongState, AfterDashHoveringState>(dash.EndOfDash),
                    TransitionConfiguration.GetConfiguration<GroundedState, DashLongState>(dash.GroundToDash),
                    TransitionConfiguration.GetConfiguration<WallClingingState, DashLongState>(dash.WallClingingToDash)
                }
            };
            return configuration;
        }

        private StateConfiguration GetAfterDashHoveringConfiguration(ActorEntity playerEntity)
        {
            var hovering = new AfterDashHoveringState(playerEntity, this);
            StateConfiguration configuration = new StateConfiguration
            {
                State = hovering,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<AfterDashHoveringState, RisingState>(hovering.HoveringToRising),
                    TransitionConfiguration.GetConfiguration<AfterDashHoveringState, FallingState>(hovering.HoveringToFalling),
                    TransitionConfiguration.GetConfiguration<AfterDashHoveringState, GroundedState>(hovering.HoveringToGrounded),
                }
            };
            return configuration;
        }
    }
}