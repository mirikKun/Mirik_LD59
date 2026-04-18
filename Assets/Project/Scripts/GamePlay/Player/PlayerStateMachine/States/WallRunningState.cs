using ImprovedTimers.Project.Scripts.Utils.Timers;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class WallRunningState : IState
    {
        private readonly ActorEntity _player;
        private readonly WallRunMoveStateConfig _config;
        private readonly CountdownTimer _wallRunTimer;
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private PlayerController PlayerController => _player.Get<PlayerController>();
        private WallDetector WallDetector => _player.Get<WallDetector>();
        private PlayerEffects.PlayerEffects Effects => _player.Get<PlayerEffects.PlayerEffects>();
        private CeilingDetector CeilingDetector => _player.Get<CeilingDetector>();


        public WallRunningState(ActorEntity player, WallRunMoveStateConfig config)
        {
            _player = player;
            _config = config;
            _wallRunTimer = new CountdownTimer(_config.WallRunDuration);
        }

        public void OnEnter()
        {
            Mover.OnGroundContactLost();
            WallDetector.SetWallAngleWithMultiplier(_config.WallAngleMultiplier);
            _wallRunTimer.Start();
            RotateCamera();
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 horizontalCameraDirection = PlayerController.CameraTrX.forward -
                                                VectorMath.ExtractDotVector(PlayerController.CameraTrX.forward,
                                                    Mover.Tr.up);
            Vector3 wallRunDirection = Vector3.ProjectOnPlane(horizontalCameraDirection, WallDetector.GetWallNormal())
                .normalized;
            Vector3 velocity = wallRunDirection * _config.WallRunSpeed -
                               WallDetector.GetWallNormal() * _config.WallGravity;

            Mover.SetVelocity(velocity);
            Mover.SetMomentum(velocity);
        }


        public void OnExit()
        {
            ResetCameraAngle();
            WallDetector.SetWallAngleWithMultiplier(1);
        }

        private void RotateCamera()
        {
            float sign = Mathf.Sign(Vector3.Dot(WallDetector.GetWallNormal(), PlayerController.CameraTrX.right));
            //
            // float targetAngle = sign * _cameraAngle;
            // _controller.CameraViewTr.rotation = Quaternion.AngleAxis(-targetAngle, _controller.CameraTrX.forward)*_controller.CameraViewTr.rotation;

            Effects.CameraMovingEffects.SetWallRunTilt(-sign);
        }

        private void ResetCameraAngle()
        {
            //_controller.CameraViewTr.localRotation = Quaternion.identity;
            Effects.CameraMovingEffects.SetWallRunTilt(0);
        }

        private bool AlignWithInput()
        {
            Vector3 horizontalCameraDirection = PlayerController.CameraTrX.forward - VectorMath.ExtractDotVector(PlayerController.CameraTrX.forward, Mover.Tr.up);
            Vector3 wallRunDirection = Vector3.ProjectOnPlane(horizontalCameraDirection, WallDetector.GetWallNormal()).normalized;
            Vector3 inputDirection = PlayerController.GetInputMovementDirection();

            bool sameForwardDirection = Vector3.Dot(wallRunDirection, inputDirection) > 0f;
            bool sameNormalDirection = Vector3.Dot(WallDetector.GetWallNormal(), inputDirection) < 0f;
            
            bool haveInputIntoWall = PlayerController.Input.Direction.x * GetWallXDirection(wallRunDirection, inputDirection, Mover.Tr.up) < 0f;

            return sameForwardDirection && sameNormalDirection && haveInputIntoWall;
        }

        private float GetWallXDirection(Vector3 to, Vector3 from, Vector3 axis)
        {
            Vector3 cross = Vector3.Cross(from, to);
            float dot = Vector3.Dot(cross, axis);
            return dot;
        }


        public bool WallRunningToGround() => Mover.IsGrounded();
        public bool WallRunningToFalling() => _wallRunTimer.IsFinished || !WallDetector.HitSidewaysWall();

        public bool FallingToWallRunning() =>
            !Mover.IsGrounded() &&
            Mover.GetHorizontalMomentum().magnitude > _config.MinSpeedToStartWallRun &&
            WallDetector.HitSidewaysWall() &&
            Mover.GetVerticalMomentum().magnitude < _config.MaxVerticalSpeedToStartWallRun
            && !_player.Get<PlayerStateMachineContainer>().HaveStateInHistory<WallRunningState>(2) && AlignWithInput();

        public bool RisingToWallRunning() => (Mover.IsFalling() || CeilingDetector.HitCeiling()) &&
                                             Mover.GetHorizontalMomentum().magnitude >
                                             _config.MinSpeedToStartWallRun &&
                                             WallDetector.HitSidewaysWall() && AlignWithInput();
    }
}