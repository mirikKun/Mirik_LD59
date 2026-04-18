using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs
{
    [CreateAssetMenu(menuName = "State Configs/Wall Run Config", fileName = "WallRunConfig")]
    public class WallRunMoveStateConfig : BaseMoveStateConfig
    {
        [field: SerializeField] public float WallRunDuration = 3;
        [field: SerializeField] public float WallRunSpeed = 6;
        [field: SerializeField] public float MinSpeedToStartWallRun = 4;
        [field: SerializeField] public float MaxVerticalSpeedToStartWallRun = 14;
        [field: SerializeField] public float WallGravity = 5;
        [field: SerializeField] public float CameraAngle = 7;
        [field: SerializeField] public float WallAngleMultiplier = 3;

        [Space] [field: SerializeField] public float JumpForwardPower = 12;
        [field: SerializeField] public float JumpUpPower = 12;
        [field: SerializeField] public float JumpFromWallPower = 8;

        public override List<StateConfiguration> GetStateConfiguration(ActorEntity playerEntity,
            AbilityInstance abilitiesInstance)
        {
            List<StateConfiguration> jumpStateConfigurations = new List<StateConfiguration>()
            {
                GetWallRunningConfiguration(playerEntity),
                GetWallRunJumpConfiguration(playerEntity)
            };
            return jumpStateConfigurations;
        }


        private StateConfiguration GetWallRunningConfiguration(ActorEntity playerEntity)
        {
            var wallRunning = new WallRunningState(playerEntity, this);
            StateConfiguration configuration = new StateConfiguration
            {
                State = wallRunning,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<WallRunningState, GroundedState>(wallRunning
                        .WallRunningToGround),
                    TransitionConfiguration.GetConfiguration<WallRunningState, FallingState>(wallRunning
                        .WallRunningToFalling),
                    TransitionConfiguration.GetConfiguration<RisingState, WallRunningState>(wallRunning
                        .RisingToWallRunning),
                    TransitionConfiguration.GetConfiguration<FallingState, WallRunningState>(wallRunning
                        .FallingToWallRunning)
                }
            };
            return configuration;
        }

        private StateConfiguration GetWallRunJumpConfiguration(ActorEntity playerEntity)
        {
            var wallJumping = new WallRunJumpState(playerEntity, this);
            StateConfiguration configuration = new StateConfiguration
            {
                State = wallJumping,
                Transitions = new List<TransitionConfiguration>()
                {
                    TransitionConfiguration.GetConfiguration<WallRunningState, WallRunJumpState>(wallJumping
                        .WallRunningToWallRunJump),
                    TransitionConfiguration.GetConfiguration<WallRunJumpState, RisingState>(wallJumping
                        .WallRunJumpToRising),
                    TransitionConfiguration.GetConfiguration<WallRunJumpState, FallingState>(wallJumping
                        .WallRunJumpToFalling)
                }
            };
            return configuration;
        }
    }
}