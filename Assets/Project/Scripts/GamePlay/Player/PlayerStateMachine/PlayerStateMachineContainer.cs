using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Systems;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.States;
using Zenject;

namespace Project.Scripts.GamePlay.Player.PlayerStateMachine
{
    public class PlayerStateMachineContainer:EntityComponent
    {
        private StateMachine _stateMachine;
        private IAbilitiesSystem _abilitySystem;

        [Inject]
        private void Construct(IAbilitiesSystem abilitySystem)
        {
            _abilitySystem = abilitySystem;
        }

        private void OnDestroy()
        {
            _stateMachine?.Dispose();
        }

        public void Tick(float deltaTime)
        {
            _stateMachine.Update(deltaTime);
        }
        public void FixedTick(float fixedDeltaTime)
        {
            _stateMachine.FixedUpdate(fixedDeltaTime);
        }

     

        public void SetupStateMachine()
        {
            _stateMachine?.Dispose();
            _stateMachine = new StateMachine();
            List<StateConfiguration> configurations = GetStateConfigurations(Entity, _abilitySystem);
            StateMachineFactory factory = new StateMachineFactory(_stateMachine);
            factory.SetupStateMachine(configurations, typeof(GroundedState));
        }

        private List<StateConfiguration> GetStateConfigurations(ActorEntity playerEntity, IAbilitiesSystem abilitiesSystem)
        {
            List<StateConfiguration> stateConfigurations=new List<StateConfiguration>();
         
            foreach (var ability in abilitiesSystem.Abilities)
            {
                if(ability.AbilityItemData.AbilityData.AbilityConfig is MovingAbilityConfig movingAbility)
                    stateConfigurations.AddRange(movingAbility.MovementMoveStateConfig.GetStateConfiguration(playerEntity,ability));
            }
            
            return stateConfigurations;
        }
        public bool HaveStateInHistory<T>(int statesBack = 6)
        {
            int statesCount = _stateMachine.PreviousStates.Count;
            for (int i = 0; i < statesBack; i++)
            {
                if (statesCount - 1 - i < 0) return false;

                if (_stateMachine.PreviousStates[statesCount - 1 - i] is T) return true;
            }

            return false;
        }

        public bool HaveStateBeforeStateInHistory<T, TBefore>(int statesBack = 10)
        {
            int statesCount = _stateMachine.PreviousStates.Count;

            for (int i = 0; i < statesBack; i++)
            {
                if (statesCount - 1 - i < 0) return false;

                if (_stateMachine.PreviousStates[statesCount - 1 - i] is T) return true;
                if (_stateMachine.PreviousStates[statesCount - 1 - i] is TBefore) return false;
            }

            return false;
        }

        public bool IsGroundedState() => _stateMachine.CurrentState is GroundedState or SlopeSlidingState;
        public T GetState<T>() where T : IState => _stateMachine.GetState<T>();

        private void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any<T>(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

    }
}