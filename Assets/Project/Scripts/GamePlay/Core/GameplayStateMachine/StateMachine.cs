using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.GameplayStateMachine
{
    public class StateMachine
    {
        private StateNode _currentNode;
        private readonly Dictionary<StateKey, StateNode> _nodes = new();
        private readonly HashSet<Transition> _anyTransitions = new();

        public List<IState> PreviousStates { get; private set; } = new List<IState>();
        public IState CurrentState => _currentNode.State;
        private int _maxHistoryStates = 50;

        public StateMachine(int maxHistoryStates)
        {
            if (maxHistoryStates < 0)
            {
                _maxHistoryStates = int.MaxValue;
            }
            else
            {
                _maxHistoryStates = maxHistoryStates;
            }
        }

        public StateMachine()
        {
        }
        public void Update(float deltaTime)
        {
            var transition = GetTransition();

            if (transition != null)
            {
                ChangeState(transition.To);
                foreach (var node in _nodes.Values)
                {
                    ResetActionPredicateFlags(node.Transitions);
                }

                ResetActionPredicateFlags(_anyTransitions);
            }

            _currentNode.State?.Update(deltaTime);
        }

        private static void ResetActionPredicateFlags(IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions.OfType<Transition<ActionPredicate>>())
            {
                transition.condition.flag = false;
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            _currentNode.State?.FixedUpdate(fixedDeltaTime);
        }

   
        public void SetState(IState state, int index=0)
        {
            var key = new StateKey(state.GetType(), index);
            _currentNode = _nodes[key];
            _currentNode.State?.OnEnter();
            PreviousStates.Add(_currentNode.State);
        }

        void ChangeState(IState state)
        {
            if (state == _currentNode.State)
                return;

            var previousState = _currentNode.State;
            var nextState = state;

            Debug.Log($"Prev {previousState.GetType().Name} Next{nextState.GetType().Name}");
            previousState?.OnExit();
            AddStateToHistory(state);
            nextState.OnEnter();
            _currentNode = FindNodeByState(state);
        }

        private StateNode FindNodeByState(IState state)
        {
            return _nodes.Values.FirstOrDefault(node => node.State == state);
        }
        public T GetState<T>(int index=0) where T : IState
        {
            return (T)_nodes[new StateKey(typeof(T), index)].State;
        }
        private void AddStateToHistory(IState state)
        {
            PreviousStates.Add(state);
            if (PreviousStates.Count > _maxHistoryStates)
                PreviousStates.RemoveAt(0);
        }

        public void AddTransition<T>(IState from, IState to, T condition)
        {
            AddTransition(from, 0, to, 0, condition);
        }

        public void AddTransition<T>(IState from, int fromIndex, IState to, int toIndex, T condition)
        {
            GetOrAddNode(from, fromIndex).AddTransition(GetOrAddNode(to, toIndex).State, condition);
        }

        public void AddAnyTransition<T>(IState to, T condition)
        {
            AddAnyTransition(to, 0, condition);
        }

        public void AddAnyTransition<T>(IState to, int toIndex, T condition)
        {
            _anyTransitions.Add(new Transition<T>(GetOrAddNode(to, toIndex).State, condition));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Evaluate())
                    return transition;

            foreach (var transition in _currentNode.Transitions)
            {
                if (transition.Evaluate())
                    return transition;
            }

            return null;
        }

        private StateNode GetOrAddNode(IState state, int index = 0)
        {
            var key = new StateKey(state.GetType(), index);
            var node = _nodes.GetValueOrDefault(key);
            if (node == null)
            {
                node = new StateNode(state);
                _nodes[key] = node;
            }

            return node;
        }
        private class StateNode
        {
            public IState State { get; }
            public HashSet<Transition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition<T>(IState to, T predicate)
            {
                Transitions.Add(new Transition<T>(to, predicate));
            }
        }

        public void Dispose()
        {
            foreach (var node in _nodes.Values)
            {
                node.State.Dispose();
            }
        }
    }
}