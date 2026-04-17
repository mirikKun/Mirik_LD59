using System;

namespace Code.Gameplay.Common.Time
{
    public class UnityTimeService : ITimeService
    {
        private bool _paused=false;
        private float _timeScale=1;

        public float DeltaTime => !_paused ? UnityEngine.Time.deltaTime : 0;
        public float TimeScale => _paused?0:_timeScale;
        

        public DateTime UtcNow => DateTime.UtcNow;
        public event Action<float> TimeScaleChanged;

        public void StopTime() => _paused = true;
        public void StartTime() => _paused = false;
        public void SetTimeScale(float timeScale)
        {
            _timeScale = timeScale;
            TimeScaleChanged?.Invoke(_timeScale);
        }
    }
}