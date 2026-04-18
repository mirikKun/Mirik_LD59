using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Basic State Config", fileName = "BasicStateConfig")]
    public class BasicMoveStateConfig:BaseMoveStateConfig
    {
        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> configurations = new List<StateConfiguration>();
            
            
            configurations.Add(GetGroundedConfiguration(playerEntity));
            configurations.Add(GetFallingConfiguration(playerEntity));
            configurations.Add(GetSlidingConfiguration(playerEntity));
            configurations.Add(GetRisingConfiguration(playerEntity));
            return configurations;
        }
        private StateConfiguration GetGroundedConfiguration(ActorEntity playerEntity)
        {
            var grounded = new GroundedState(playerEntity);
            StateConfiguration configuration = new StateConfiguration
            {
                State = grounded,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<GroundedState,RisingState>(grounded.GroundedToRising),
                    TransitionConfiguration.GetConfiguration<GroundedState,SlopeSlidingState>(grounded.GroundedToSliding),
                    TransitionConfiguration.GetConfiguration<GroundedState,FallingState>(grounded.GroundedToFalling)
                }
            };
            return configuration;
        }
        
        private StateConfiguration GetFallingConfiguration(ActorEntity playerEntity)
        {
            var falling = new FallingState(playerEntity);
            StateConfiguration configuration = new StateConfiguration
            {
                State = falling,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<FallingState,RisingState>(falling.FallingToRising),
                    TransitionConfiguration.GetConfiguration<FallingState,GroundedState>(falling.FallingToGrounded),
                    TransitionConfiguration.GetConfiguration<FallingState,SlopeSlidingState>(falling.FallingToSliding)
                }
            };
            return configuration;
        }
        
        private StateConfiguration GetSlidingConfiguration(ActorEntity playerEntity)
        {
            var slopeSliding = new SlopeSlidingState(playerEntity);
            StateConfiguration configuration = new StateConfiguration
            {
                State = slopeSliding,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<SlopeSlidingState,RisingState>(slopeSliding.SlidingToRising),
                    TransitionConfiguration.GetConfiguration<SlopeSlidingState,FallingState>(slopeSliding.SlidingToFalling),
                    TransitionConfiguration.GetConfiguration<SlopeSlidingState,GroundedState>(slopeSliding.SlidingToGround)
                }
            };
            return configuration;
        }        
        private StateConfiguration GetRisingConfiguration(ActorEntity playerEntity)
        {
            var rising = new RisingState(playerEntity);
            StateConfiguration configuration = new StateConfiguration
            {
                State = rising,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<RisingState,GroundedState>(rising.RisingToGrounded),
                    TransitionConfiguration.GetConfiguration<RisingState,SlopeSlidingState>(rising.GroundToSliding),
                    TransitionConfiguration.GetConfiguration<RisingState,FallingState>(rising.RisingToFalling)
                }
            };
            return configuration;
        }
    }
    
    
}