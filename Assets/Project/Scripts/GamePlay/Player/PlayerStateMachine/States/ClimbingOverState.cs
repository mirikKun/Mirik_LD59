using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States
{
    public class ClimbingOverState:IState
    {
        private readonly ActorEntity _player;

        private readonly ClimbingOverMoveStateConfig _config;
        
        private PlayerMover Mover => _player.Get<PlayerMover>();
        private WallDetector WallDetector => _player.Get<WallDetector>();

        public ClimbingOverState(ActorEntity player,ClimbingOverMoveStateConfig config) {
            _config = config;
            _player = player;
        }

        public  void OnEnter()
        {
            Mover.OnGroundContactLost();
            Mover.SetMomentum(Vector3.zero);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector3 velocity = Mover.CalculateMovementVelocity()*_config.HorizontalSpeedReduction;
            Vector3 verticalVelocity = VectorMath.ExtractDotVector(velocity, Mover.Tr.up);
            Vector3 horizontalMomentum = velocity - verticalVelocity;
            verticalVelocity += Mover.Tr.up * _config.ClimbingOverSpeed;

            velocity = horizontalMomentum + verticalVelocity;
            Mover.SetVelocity(velocity);
        }

        public void OnExit()
        {
  
        }

        public void Update(float deltaTime)
        {
            
        }
        private bool SameDirection()
        {
            Vector3 wallNormal = -WallDetector.GetWallNormal();
            Vector3 inputVelocity = Mover.CalculateMovementVelocity().normalized;
            return Vector3.Dot(wallNormal, inputVelocity) > 0;
        }
        
        public bool FallingToClimbingOver() => WallDetector.HitForwardBottomWall()&&SameDirection();
        public bool RisingToClimbingOver() => Mover.IsFalling() && WallDetector.HitForwardBottomWall()&&SameDirection();
        public bool GroundedToClimbingOver() => WallDetector.HitForwardBottomWall()&&SameDirection();
        public bool ClimbingOverToRising() => !WallDetector.HitForwardBottomWall() || !SameDirection();
        public bool ClimbingOverToFalling() => _player.Get<CeilingDetector>().HitCeiling();

    }
}