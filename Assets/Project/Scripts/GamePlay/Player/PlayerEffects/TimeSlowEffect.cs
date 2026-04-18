using System;
using Project.Scripts.GamePlay.Common.Time;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    public class TimeSlowEffect : MonoBehaviour
    {
        [SerializeField] private float _curveDuration = 0.6f;
        [SerializeField] private AnimationCurve _timeSpeedCurve;

        [SerializeField] private float _timeStopDuration = 0.03f;
        [SerializeField] private float _timeStopMultiplier = 0.1f;

        [Inject] private ITimeService _timeService;

        public void PlayCurve()
        {
            Console.WriteLine("Time Slowed.");
            ApplyTimeSlowCurved(_curveDuration);
        }

        public void PlayTimeStop()
        {
            ApplyTimeStop(_timeStopDuration);
        }

        public void Stop()
        {
            Console.WriteLine("Time resumed.");
        }

        private async void ApplyTimeSlowCurved(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (!_timeService.Paused)

                {
                    elapsedTime += _timeService.UnscaledDeltaTime;
                    float timeScale = _timeSpeedCurve.Evaluate(elapsedTime / duration);

                    _timeService.SetTimeScale(timeScale);
                }

                await Awaitable.NextFrameAsync();
            }

            _timeService.SetTimeScale(1f);
            Stop();
        }

        private async void ApplyTimeStop(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (!_timeService.Paused)

                {
                    elapsedTime += _timeService.UnscaledDeltaTime;
                    _timeService.SetTimeScale(_timeStopMultiplier);
                }

                await Awaitable.NextFrameAsync();
            }

            _timeService.SetTimeScale(1f);
            Stop();
        }
    }
}