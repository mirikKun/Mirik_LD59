using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    public abstract class DashBaseStateConfig: BaseMoveStateConfig
    {
        [field: SerializeField] public float DashSpeed { get; private set; } = 50f;
        [field: SerializeField] public float DashExitSpeed { get; private set; } = 8f;
        [field: SerializeField] public float DashDuration { get; private set; } = 0.24f;

        [field: SerializeField] public float UpdatedFov { get; private set; } = 77;
        [field:Header("Sounds")]
        [field: SerializeField] public SoundData Sound { get; private set; }
        
        
        
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetDashConfiguration(playerEntity,abilitiesInstance)
            };
            return jumpStateConfigurations;
        }

        protected abstract StateConfiguration GetDashConfiguration(ActorEntity playerEntity, AbilityInstance abilitiesInstance);
    }
}