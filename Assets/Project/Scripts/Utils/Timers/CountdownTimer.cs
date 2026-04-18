using UnityEngine;

namespace ImprovedTimers.Project.Scripts.Utils.Timers {
    /// <summary>
    /// Timer that counts down from a specific value to zero.
    /// </summary>
    public class CountdownTimer : Timer {
        private float _timeScale=1;

        public CountdownTimer(float value) : base(value) { }
        public void SetTimeScale(float timeScale)
        {
            _timeScale= timeScale;
        }

        public override void Tick(float deltaTime) {
            if (IsRunning && CurrentTime > 0) {
                CurrentTime -= deltaTime*_timeScale;
            }

            if (IsRunning && CurrentTime <= 0) {
                Stop();
            }
        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}