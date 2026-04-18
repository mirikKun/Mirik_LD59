using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates
{
    public abstract class BaseAirState: IJumpState
    {
        protected readonly ActorEntity _player;
        protected PlayerMover Mover => _player.Get<PlayerMover>();
        protected PlayerController PlayerController => _player.Get<PlayerController>();


        public BaseAirState(ActorEntity player) {
            this._player = player;
        }

        public virtual void FixedUpdate(float fixedDeltaTime){}
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Dispose() { }

        protected Vector3 AdjustHorizontalAirMomentum(float fixedDeltaTime, Vector3 horizontalMomentum, Vector3 movementVelocity)
        {
            if (horizontalMomentum.magnitude > Mover.MovementSpeed)
            {
                if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                {
                    movementVelocity = VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);
                }

                horizontalMomentum += movementVelocity * (fixedDeltaTime * Mover.AirControlRate * 0.25f);
            }
            else
            {
                horizontalMomentum += movementVelocity * (fixedDeltaTime * Mover.AirControlRate);
                horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, Mover.MovementSpeed);
            }

            return horizontalMomentum;
        }
    }
}