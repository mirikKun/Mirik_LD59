using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.GamePlay.Core.GameplayStateMachine
{
    public class StateMachineFactory
    {
        private readonly StateMachine _stateMachine;

        public StateMachineFactory(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void SetupStateMachine(List<StateConfiguration> configurations, Type initialStateType, int initialStateIndex = 0)
        {
            foreach (var config in configurations)
            {
                foreach (var transition in config.Transitions)
                {
                    var fromConfig = configurations.FirstOrDefault(c => c.State.GetType() == transition.FromState && c.Index == transition.FromIndex);
                    var toConfig = configurations.FirstOrDefault(c => c.State.GetType() == transition.ToState && c.Index == transition.ToIndex);

                    if (fromConfig.State != null && toConfig.State != null)
                    {
                        _stateMachine.AddTransition(
                            fromConfig.State, transition.FromIndex,
                            toConfig.State, transition.ToIndex,
                            transition.Condition
                        );
                    }
                }
            }

            var initialConfig = configurations.FirstOrDefault(c => c.State.GetType() == initialStateType && c.Index == initialStateIndex);
            if (initialConfig.State != null)
            {
                _stateMachine.SetState(initialConfig.State, initialStateIndex);
            }
        }
    }}