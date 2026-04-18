using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Dash Evade State Config", fileName = "DashEvadeStateConfig")]
    public class DashEvadeStateConfig: DashBaseStateConfig
    {
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetDashConfiguration(playerEntity,abilitiesInstance)
            };
            return jumpStateConfigurations;
        }
        protected override StateConfiguration GetDashConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var dash = new DashEvadeState(playerEntity, this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = dash,
                Transitions = new List<TransitionConfiguration>()
                {
                    //TransitionConfiguration.GetConfiguration<DashState,GroundedState>(dash.DashToGround),
                    TransitionConfiguration.GetConfiguration<RisingState, DashEvadeState>(dash.AirToToDash<DashEvadeState>),
                    TransitionConfiguration.GetConfiguration<FallingState, DashEvadeState>(dash.AirToToDash<DashEvadeState>),
                    // TransitionConfiguration.GetConfiguration<DashState,RisingState>(dash.DashToRising),
                    // TransitionConfiguration.GetConfiguration<DashState,FallingState>(dash.DashToFalling),
                    TransitionConfiguration.GetConfiguration<DashEvadeState, FallingState>(dash.EndOfDash),
                    TransitionConfiguration.GetConfiguration<GroundedState, DashEvadeState>(dash.GroundToDash),
                    TransitionConfiguration.GetConfiguration<WallClingingState, DashEvadeState>(dash.WallClingingToDash)
                }
            };
            return configuration;
        }

    }
}