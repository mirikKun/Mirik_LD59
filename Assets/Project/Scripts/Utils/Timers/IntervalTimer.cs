using System;
using UnityEngine;

namespace ImprovedTimers.Project.Scripts.Utils.Timers
{
    /// <summary>
    /// Countdown timer that fires an event every interval until completion.
    /// </summary>
    public class IntervalTimer : Timer {
        private readonly float interval;
        private float nextInterval;
        private bool endless; 
    
        public Action OnInterval = delegate { };
    
        public IntervalTimer(float totalTime, float intervalSeconds) : base(totalTime) {
            interval = intervalSeconds;
            if (totalTime <= 0)
            {
                endless = true;
                nextInterval = interval;
                initialTime=interval*2;
            }
            else
            {
                nextInterval = totalTime - interval;
            }
            
        }
    
        public override void Tick(float deltaTime) {
            if (IsRunning && CurrentTime > 0) {
                CurrentTime -= deltaTime;
            
                // Fire interval events as long as thresholds are crossed
                while (CurrentTime <= nextInterval && nextInterval >= 0) {
                    OnInterval.Invoke();
                    if (endless)
                    {
                        CurrentTime+= interval;
                    }
                    else
                    {
                        nextInterval -= interval;

                    }
                    
                }
            }
        
            if (IsRunning && CurrentTime <= 0&&!endless) {
                CurrentTime = 0;
                Stop();
            }
        }

        public override void Start()
        {
            base.Start();
            nextInterval = CurrentTime - interval;

        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}