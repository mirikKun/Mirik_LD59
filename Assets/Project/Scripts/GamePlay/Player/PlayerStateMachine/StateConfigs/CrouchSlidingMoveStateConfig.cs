using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Crouch Sliding State Config", fileName = "CrouchSlidingStateConfig")]

    public class CrouchSlidingMoveStateConfig:BaseMoveStateConfig
    {
        [field: SerializeField] public float SlideSpeed { get; private set; } = 20f;
        [field: SerializeField] public float SlideMaxSpeedDuration { get; private set; } = 0.4f;
        [field: SerializeField] public float MinSlideSpeed { get; private set; } = 3f;
        [field: SerializeField] public float MinSlideAngle { get; private set; } = 20f;
        [field: SerializeField] public float ColliderHeight { get; private set; } = 0.6f;
        [field: SerializeField] public float SlidingFriction { get; private set; } = 10f;

        [field: Space]
        [field: SerializeField] public float PouncePower { get; private set; } = 21f;
        [field: SerializeField] public float PounceMinAngle { get; private set; } = 30f;
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetSlideConfiguration(playerEntity,abilitiesInstance),
                GetSlideJumpConfiguration(playerEntity)
            };
            return jumpStateConfigurations;

        }
        private StateConfiguration GetSlideConfiguration(ActorEntity playerEntity,AbilityInstance abilitiesInstance)
        {
            var sliding = new CrouchSlidingState(playerEntity,this,abilitiesInstance);
            StateConfiguration configuration = new StateConfiguration
            {
                State = sliding,
                Transitions = new List<TransitionConfiguration>()
                {
                    //TransitionConfiguration.GetConfiguration<DashState,GroundedState>(dash.DashToGround),
                    TransitionConfiguration.GetConfiguration<CrouchSlidingState,GroundedState>(sliding.CrouchSlidingToGround),
                    TransitionConfiguration.GetConfiguration<CrouchSlidingState,FallingState>(sliding.CrouchSlidingToFalling),
                    TransitionConfiguration.GetConfiguration<GroundedState,CrouchSlidingState>(sliding.GroundedToCrouchSliding)
              
                }
            };
            return configuration;
        }
        private StateConfiguration GetSlideJumpConfiguration(ActorEntity playerEntity)
        {
            var slidingJump = new CrouchSlidingJumpState(playerEntity,this);
            StateConfiguration configuration = new StateConfiguration
            {
                State = slidingJump,
                Transitions = new List<TransitionConfiguration>()
                {
                    //TransitionConfiguration.GetConfiguration<DashState,GroundedState>(dash.DashToGround),
                    TransitionConfiguration.GetConfiguration<CrouchSlidingState,CrouchSlidingJumpState>(slidingJump.SlidingToJump),
                    TransitionConfiguration.GetConfiguration<CrouchSlidingJumpState,FallingState>(slidingJump.JumpToFalling),
                    TransitionConfiguration.GetConfiguration<CrouchSlidingJumpState,RisingState>(slidingJump.JumpToRising)
              
                }
            };
            return configuration;
        }
    }
}