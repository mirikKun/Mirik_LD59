using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Common.Time.Behaviours
{
    public class AnimatorTimeScaleListener : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private ITimeService _timeService;

        [Inject]
        private void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }

        private void Start()
        {
            _timeService.OnTimeScaleChanged += AdjustAnimatorTimeScale;
        }

        private void OnDestroy()
        {
            _timeService.OnTimeScaleChanged -= AdjustAnimatorTimeScale;
        }

        private void AdjustAnimatorTimeScale(float timeScale)
        {
            _animator.speed = timeScale;
        }
    }
}