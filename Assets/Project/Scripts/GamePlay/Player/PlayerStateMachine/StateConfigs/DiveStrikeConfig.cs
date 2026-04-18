using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/DiveStrike Config", fileName = "DiveStrikeStateConfig")]
    public class DiveStrikeMoveStateConfig : BaseMoveStateConfig,IMoveStateWithCostConfig
    {
        [field: SerializeField] public float DiveStrikeSpeed { get; private set; } = 35f;

        [field: SerializeField] public float ManaCost { get; private set; }


        [field:Header("Sounds")]
        [field: SerializeField] public SoundData StartSound { get; private set; }

        [field: SerializeField] public SoundData LandSound { get; private set; }

        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity, AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> diveStrikeStateConfigurations = new List<StateConfiguration>()
            {
                GetDiveStrikeConfiguration(playerEntity,abilitiesInstance)
            };
            return diveStrikeStateConfigurations;
        }

        private StateConfiguration GetDiveStrikeConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var divestrike = new DiveStrikeState(playerEntity, this, abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = divestrike,
                Transitions = new List<TransitionConfiguration>()
                {
                    // Add transitions here
                    TransitionConfiguration.GetConfiguration<FallingState,DiveStrikeState>(divestrike.FallingToDiveStrike),
                    TransitionConfiguration.GetConfiguration<RisingState,DiveStrikeState>(divestrike.RisingToDiveStrike),
                    TransitionConfiguration.GetConfiguration<DiveStrikeState,GroundedState>(divestrike.DiveStrikeToGrounded)
                }
            };
            return configuration;
        }
    }
}
