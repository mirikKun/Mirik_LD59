using System;
using Project.Scripts.GamePlay.Common.Time;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Animations
{
    public class HandAnimator:MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _defaultTransitionTime = 0.75f;
        private ITimeService _timeService;

        private int StraightState=>Animator.StringToHash("Straight");
        private int IdleState=>Animator.StringToHash("Idle");
        private int ChantingState=>Animator.StringToHash("Chanting");
        private int PalmState=>Animator.StringToHash("Palm");


        [Inject]
        private void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }

        private void Start()
        {
            _timeService.OnTimeScaleChanged += ChangeAnimationsTimeScale;
        }

        private void OnDestroy()
        {
            _timeService.OnTimeScaleChanged -= ChangeAnimationsTimeScale;

        }

        private void ChangeAnimationsTimeScale(float timeScale)
        {
            _animator.speed = timeScale;
        }

        public void PlayStraightCastAnimation(float transitionDuration=-1)
        {
            transitionDuration = GetTransitionDuration(transitionDuration);
            _animator.CrossFade(StraightState,transitionDuration);
        }
        public void PlayPalmCastAnimation(float transitionDuration=-1)
        {
            transitionDuration = GetTransitionDuration(transitionDuration);

            _animator.CrossFade(PalmState,transitionDuration);
        }

        public void PlayIdleAnimation(float transitionDuration=-1)
        {
            transitionDuration = GetTransitionDuration(transitionDuration);

            _animator.CrossFade(IdleState,transitionDuration);
        }

        public void PlayChantingAnimation(float transitionDuration=-1)
        {
            transitionDuration = GetTransitionDuration(transitionDuration);

            _animator.CrossFade(ChantingState,transitionDuration);
        }
        private float GetTransitionDuration(float transitionDuration)
        {
            return transitionDuration > 0 ? transitionDuration : _defaultTransitionTime;
        }
    }
    
}