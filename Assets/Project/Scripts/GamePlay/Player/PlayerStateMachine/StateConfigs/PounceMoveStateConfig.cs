using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Pounce State Config", fileName = "PounceStateConfig")]

    public class PounceMoveStateConfig:BaseMoveStateConfig
    {
        [field: SerializeField] public float PouncePower { get; private set; } = 21f;
        [field: SerializeField] public float PounceMinAngle { get; private set; } = 30f;
        [field: SerializeField] public bool EveryDirection { get; private set; } = true;

        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetPounceConfiguration(playerEntity,abilitiesInstance)
            };
            return jumpStateConfigurations;

        }
        private StateConfiguration GetPounceConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var pounce = new PounceState(playerEntity,this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = pounce,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<GroundedState,PounceState>(pounce.GroundedToPounce),
                    TransitionConfiguration.GetConfiguration<PounceState,RisingState>(pounce.PounceToRising),
                    TransitionConfiguration.GetConfiguration<PounceState,FallingState>(pounce.PounceToFalling)
                }
            };
            return configuration;
        }
    }
}