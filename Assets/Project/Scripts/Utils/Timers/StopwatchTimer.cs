using UnityEngine;

namespace ImprovedTimers.Project.Scripts.Utils.Timers {
    /// <summary>
    /// Timer that counts up from zero to infinity.  Great for measuring durations.
    /// </summary>
    public class StopwatchTimer : Timer {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime) {
            if (IsRunning) {
                CurrentTime += deltaTime;
            }
        }

        public override bool IsFinished => false;
    }
}