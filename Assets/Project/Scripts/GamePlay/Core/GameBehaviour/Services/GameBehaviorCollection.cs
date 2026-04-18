using System.Collections.Generic;

namespace Project.Scripts.GamePlay.Core.GameBehaviour.Services
{
    public class GameBehaviorCollection<T> where T : class
    {
        private readonly List<T> _behaviors = new List<T>();
        private readonly List<T> _behaviorsToAdd = new List<T>();
        private readonly List<T> _behaviorsToRemove = new List<T>();
        private bool _isExecuting = false;

        public void Register(T behavior)
        {
            if (_isExecuting)
            {
                if (!_behaviorsToAdd.Contains(behavior))
                    _behaviorsToAdd.Add(behavior);
            }
            else
            {
                if (!_behaviors.Contains(behavior))
                    _behaviors.Add(behavior);
            }
        }

        public void Unregister(T behavior)
        {
            if (_isExecuting)
            {
                if (!_behaviorsToRemove.Contains(behavior))
                    _behaviorsToRemove.Add(behavior);
            }
            else
            {
                _behaviors.Remove(behavior);
            }
        }

        public void ExecuteAll(System.Action<T, float> executeAction, float parameter)
        {
            _isExecuting = true;
        
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (_behaviors[i] != null)
                    executeAction(_behaviors[i], parameter);
            }
        
            _isExecuting = false;
            ProcessPendingChanges();
        }
        public void ExecuteAll(System.Action<T> executeAction)
        {
            _isExecuting = true;
        
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (_behaviors[i] != null)
                    executeAction(_behaviors[i]);
            }
        
            _isExecuting = false;
            ProcessPendingChanges();
        }
        private void ProcessPendingChanges()
        {
            foreach (var behavior in _behaviorsToAdd)
            {
                if (!_behaviors.Contains(behavior))
                    _behaviors.Add(behavior);
            }
            _behaviorsToAdd.Clear();

            foreach (var behavior in _behaviorsToRemove)
            {
                _behaviors.Remove(behavior);
            }
            _behaviorsToRemove.Clear();
        }

        public int Count => _behaviors.Count;
    }
}