using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Grappling Hook State Config", fileName = "GrapplingHookStateConfig")]

    public class GrapplingHookMoveStateConfig : BaseMoveStateConfig
    {
        [field: SerializeField] public float GrappleSpeed { get; private set; } = 20f;
        [field: SerializeField] public float GrappleMaxDistance { get; private set; } = 100f;
        [field: SerializeField] public float GrappleMaxApproachableDistance { get; private set; } = 50f;
        [field: SerializeField] public float GrappleMinDistance { get; private set; } = 5f;
        [field: SerializeField] public float GrapplingExitSpeedMultiplier { get; private set; } = 0.5f;
        [field: Space] 
        [field: SerializeField] public bool AdaptiveGrapple{ get; private set; }  = false;
        [field: SerializeField] public float AdaptiveGrappleOffset { get; private set; } = 2f;
        [field: SerializeField] public float AdaptiveGrappleLetGoDistance { get; private set; } = 3f;

        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> grapplingHookStateConfigurations = new List<StateConfiguration>()
            {
                GetGrapplingHookConfiguration(playerEntity,abilitiesInstance)
            };
            return grapplingHookStateConfigurations;
        }

        private StateConfiguration GetGrapplingHookConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var grapplingHook = new GrapplingHookState(playerEntity, this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = grapplingHook,
                Transitions = new List<TransitionConfiguration>()
                {
                    //TransitionConfiguration.GetConfiguration<DashState,GroundedState>(dash.DashToGround),
                    TransitionConfiguration.GetConfiguration<GroundedState, GrapplingHookState>(grapplingHook.GroundedToGrappleHook),
                    TransitionConfiguration.GetConfiguration<RisingState, GrapplingHookState>(grapplingHook.AirToGrappleHook),
                    TransitionConfiguration.GetConfiguration<FallingState, GrapplingHookState>(grapplingHook.AirToGrappleHook),
                    TransitionConfiguration.GetConfiguration<GrapplingHookState, RisingState>(grapplingHook.GrappleHookToRising),
                    TransitionConfiguration.GetConfiguration<GrapplingHookState, FallingState>(grapplingHook.GrappleHookToFalling),
                }
            };
            return configuration;
        }
    }
}