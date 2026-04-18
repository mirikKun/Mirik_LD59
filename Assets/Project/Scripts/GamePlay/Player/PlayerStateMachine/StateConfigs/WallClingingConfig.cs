using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Wall Clinging State Config", fileName = "WallClingingStateConfig")]

    public class WallClingingConfig:BaseMoveStateConfig

    {
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetClingingConfiguration(playerEntity)
            };
            return jumpStateConfigurations;
        }
        private StateConfiguration GetClingingConfiguration(ActorEntity playerEntity)
        {
            var clinging = new WallClingingState(playerEntity);
            StateConfiguration configuration = new StateConfiguration
            {
                State = clinging,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<FallingState,WallClingingState>(clinging.FallingToClinging),
                    TransitionConfiguration.GetConfiguration<RisingState,WallClingingState>(clinging.RisingToClinging),
                    TransitionConfiguration.GetConfiguration<WallClingingState,PounceState>(clinging.ClingingToPounce),
                    TransitionConfiguration.GetConfiguration<WallClingingState,FallingState>(clinging.ClingingToFalling)
                }
            };
            return configuration;
        }
    }
}