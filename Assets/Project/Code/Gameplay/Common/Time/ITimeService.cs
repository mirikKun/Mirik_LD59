using System;

namespace Code.Gameplay.Common.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        DateTime UtcNow { get; }
        float TimeScale { get; }
        void StopTime();
        void StartTime();
        event Action<float> TimeScaleChanged;
        void SetTimeScale(float timeScale);
    }
}