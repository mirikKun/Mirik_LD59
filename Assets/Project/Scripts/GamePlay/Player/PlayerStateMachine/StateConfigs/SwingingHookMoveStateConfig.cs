using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Swinging Hook State Config", fileName = "SwingingHookStateConfig")]

    public class SwingingHookMoveStateConfig:BaseMoveStateConfig
    {
        [field: SerializeField] public float SwingingSpeed { get; private set; }= 10f;
        [field: SerializeField] public float GrapplingSpeed { get; private set; }= 10f;
        [field: SerializeField] public float PreparingDuration { get; private set; }= 0.25f;

        [field: SerializeField] public   float SwingingDuration{ get; private set; }=5;

        [field: SerializeField] public float SwingingMaxDistance { get; private set; }= 50f;
        [field: SerializeField] public float SwingingMinDistance { get; private set; }= 5f;
        [field: SerializeField] public float MaxSwingingSpeed { get; private set; }= 20f;
        [field: SerializeField] public float SwingingExitSpeedMultiplier{ get; private set; } = 1f;
        [field: SerializeField] public float StartSwingMomentum{ get; private set; } = 2f;
        [field: SerializeField] public AnimationCurve SwingingDirectionLerpCurve{ get; private set; }

        [field:Header("Sounds")]
        [field: SerializeField] public SoundData HookSound { get; private set; }
        [field: SerializeField] public SoundData SwingSound { get; private set; }
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> swingingHookStateConfigurations = new List<StateConfiguration>()
            {
                GetGrapplingHookConfiguration(playerEntity,abilitiesInstance)
            };
            return swingingHookStateConfigurations;
        }
        
        private StateConfiguration GetGrapplingHookConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var swingingHook = new SwingingHookState(playerEntity,this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = swingingHook,
                Transitions = new List<TransitionConfiguration>()
                {
                    //TransitionConfiguration.GetConfiguration<DashState,GroundedState>(dash.DashToGround),
                    TransitionConfiguration.GetConfiguration<GroundedState, SwingingHookState>(swingingHook.GroundedToSwingingHook),
                    TransitionConfiguration.GetConfiguration<RisingState, SwingingHookState>(swingingHook.AirToSwingingHook),
                    TransitionConfiguration.GetConfiguration<FallingState, SwingingHookState>(swingingHook.AirToSwingingHook),
                    TransitionConfiguration.GetConfiguration<SwingingHookState, RisingState>(swingingHook.SwingingHookToRising),
                    TransitionConfiguration.GetConfiguration<SwingingHookState, FallingState>(swingingHook.SwingingHookToFalling),
            }};
            return configuration;
        }
    }
}