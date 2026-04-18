using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Animations
{
    public enum HandAnimationType
    {
        Idle,
        Straight,
        Palm,
        Chanting,
        WithoutAnimation
    }

    public class PlayerAnimator:EntityComponent
    {
        [SerializeField] private HandAnimator _handAnimator;
        public HandAnimator HandAnimator => _handAnimator;

        public void PlayHandAnimation(HandAnimationType animationType, float transitionDuration=-1)
        {
            if (_handAnimator == null||animationType == HandAnimationType.WithoutAnimation)
            {
                return;
            }

            switch (animationType)
            {
                case HandAnimationType.Straight:
                    _handAnimator.PlayStraightCastAnimation(transitionDuration);
                    break;
                case HandAnimationType.Palm:
                    _handAnimator.PlayPalmCastAnimation(transitionDuration);
                    break;
                case HandAnimationType.Chanting:
                    _handAnimator.PlayChantingAnimation(transitionDuration);
                    break;
                default:
                    _handAnimator.PlayIdleAnimation(transitionDuration);
                    break;
            }
        }
    }
}