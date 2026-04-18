using Project.Scripts.GamePlay.Common.Time;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.General;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Abilities.AbilityTypes
{
    public class TimeSlowAbility : BaseAbility
    {
        private TimeSlowingAbilityConfig _config;
        private ITimeService _timeService;

        [Inject]
        private void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void SetConfig(TimeSlowingAbilityConfig config)
        {
            _config = config;
        }

        public override void OnInput(bool pressed)
        {
            if (pressed)
            {
                Execute();
            }
        }

        public override async void Execute()
        {
            InvokeAbilityExecuted();
            float elapsedTime = 0f;
            while (elapsedTime < _config.Duration)
            {
                elapsedTime += _timeService.UnscaledDeltaTime;
                float timeScale = _config.TimeSlowCurve.Evaluate(elapsedTime / _config.Duration);
                _timeService.SetTimeScale(timeScale);
                await Awaitable.NextFrameAsync();
            }
            _timeService.SetTimeScale(1f);
        }
    }
}