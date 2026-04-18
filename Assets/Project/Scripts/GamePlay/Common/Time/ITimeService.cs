using System;

namespace Project.Scripts.GamePlay.Common.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        float UnscaledDeltaTime { get; }

        float FixedDeltaTime { get; }
        float UnscaledFixedDeltaTime { get; }

        
        float TimeScale { get; }
        bool Paused { get; }
        event Action<float> OnTimeScaleChanged;
        
        
        void SetTimeScale(float timeScale);
        void StopTime();
        void StartTime();
    }
}