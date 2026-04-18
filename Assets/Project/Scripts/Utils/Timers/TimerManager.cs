using System.Collections.Generic;
using ImprovedTimers.Project.Scripts.Utils.Timers.Extensions;
using UnityEngine;

namespace ImprovedTimers.Project.Scripts.Utils.Timers {
    public static class TimerManager {
        static readonly List<Timer> timers = new();
        static readonly List<Timer> sweep = new();
        static float timeMultiplier = 1f;

        public static void RegisterTimer(Timer timer) => timers.Add(timer);
        public static void DeregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimers() {
            if (timers.Count == 0) return;
            
            sweep.RefreshWith(timers);
            foreach (var timer in sweep) {
                timer.Tick(Time.deltaTime*timeMultiplier);
            }
        }
        public static void  SetTimeMultiplier(float multiplier)
        {
            timeMultiplier = multiplier;
        }
        public static void Clear() {
            sweep.RefreshWith(timers);
            foreach (var timer in sweep) {
                timer.Dispose();
            }
            
            timers.Clear();
            sweep.Clear();
        }
    }
}