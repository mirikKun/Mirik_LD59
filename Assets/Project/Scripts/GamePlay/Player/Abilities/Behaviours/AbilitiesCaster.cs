using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Abilities.AbilityTypes;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Abilities.Factory;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Abilities.Systems;
using Project.Scripts.GamePlay.Player.Animations;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerStateMachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Abilities.Behaviours
{
    public class AbilitiesCaster : EntityComponent
    {
        private List<IAbility> _abilities = new List<IAbility>();
        private IAbilitiesFactory _abilitiesFactory;
        private IAbilitiesSystem _abilitiesSystem;

        private IAbility _currentCastingActionAbility;
        private ActionAbilityConfig _currentCastingActionAbilityConfig;
        private AbilityInstance _currentAbilityInstance;
        private AbilityCastingState _castingState;
        private float _castingTimer;
        private bool _finishCasting;
        private int _lastAbilityId;
        private bool _newAbility;
        private bool _disabled;

        [Inject]
        public void Construct(IAbilitiesFactory abilitiesFactory, IAbilitiesSystem abilitiesSystem)
        {
            _abilitiesFactory = abilitiesFactory;
            _abilitiesSystem = abilitiesSystem;
        }

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity);
            _abilitiesSystem.AbilitiesListChanged += Init;
            _abilitiesSystem.NewAbilityChosen += OnNewAbilityChosen;
        }

        public override void StartEntity()
        {
            Init();
        }

        private void OnDestroy()
        {
            _abilitiesSystem.AbilitiesListChanged -= Init;
            _abilitiesSystem.NewAbilityChosen -= OnNewAbilityChosen;
        }

        public void SetActive(bool active)
        {
            _disabled = !active;
            if (!active)
            {
                _castingState = AbilityCastingState.Idle;
                _castingTimer = 0;
                Entity.Get<PlayerAnimator>().PlayHandAnimation(HandAnimationType.Idle);

                _finishCasting = false;
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (var ability in _abilities)
            {
                if (ability is ITickableAbility tickableAbility)
                {
                    tickableAbility.Tick(deltaTime);
                }
            }

            UpdateCastingState(deltaTime);
        }

        public void Init()
        {
            DisposeAbilities();
            foreach (AbilityInstance abilityInstance in _abilitiesSystem.Abilities)
            {
                SetupAbility(abilityInstance);
            }

            Entity.Get<PlayerStateMachineContainer>().SetupStateMachine();
        }

        private void SetupAbility(AbilityInstance abilityInstance)
        {
            // abilityInstance.Clear();
            // if (abilityInstance.AbilityItemData.AbilityData.AbilityConfig is ActionAbilityConfig actionAbilityConfig)
            // {
            //     IAbility ability = actionAbilityConfig.CreateAbility(_abilitiesFactory,
            //         abilityInstance.AbilityItemData.AbilityData as ArmamentSpawnAbilityData);
            //     ability.Init(Entity);
            //     abilityInstance.OnAbilityInput += (pressed) =>
            //         OnActionAbilityInput(ability, pressed, actionAbilityConfig, abilityInstance);
            //     ability.AbilityChargesEnded += () => OnAbilityChargesEnded(abilityInstance);
            //     ability.AbilityUnlocked += () => OnAbilityUnlocked(abilityInstance);
            //     ability.AbilityExecuted += () => OnAbilityExecuted(abilityInstance);
            //     _abilities.Add(ability);
            // }
            
        }


        private void UpdateCastingState(float deltaTime)
        {
            if (_castingState == AbilityCastingState.Idle)
                return;
            if (_castingState == AbilityCastingState.Preparing)
            {
                _castingTimer += deltaTime;
                if (_castingTimer >= _currentCastingActionAbilityConfig.PreparationTime)
                {
                    OnAbilityCastingStart();
                }
            }
            else if (_castingState == AbilityCastingState.Casting)
            {
                _castingTimer += deltaTime;
                if (_castingTimer >= _currentCastingActionAbilityConfig.ActiveLockDuration && _finishCasting)
                {
                    OnAbilityCastingEnd();
                }
            }
        }

        private void DisposeAbilities()
        {
            foreach (var ability in _abilities)
            {
                if (ability is IDisposableAbility disposableAbility)
                {
                    disposableAbility.Dispose();
                }
            }

            _abilities.Clear();
            _currentCastingActionAbility = null;
            _currentCastingActionAbilityConfig = null;
            _currentAbilityInstance = null;
        }

        private void OnActionAbilityInput(IAbility ability, bool pressed, ActionAbilityConfig actionAbilityConfig,
            AbilityInstance abilityInstance)
        {
            if (_disabled || _currentCastingActionAbility != null && _currentCastingActionAbility != ability||_currentCastingActionAbility == null&&!pressed)
                return;


            _currentCastingActionAbility = ability;
            _currentCastingActionAbilityConfig = actionAbilityConfig;
            _currentAbilityInstance = abilityInstance;

            _newAbility = _lastAbilityId != abilityInstance.AbilityItemData.AbilityData.Id;
            _lastAbilityId = abilityInstance.AbilityItemData.AbilityData.Id;

            if (_castingState == AbilityCastingState.Idle)
            {
                OnAbilityPreparingStart();
            }
            else if (!pressed)
            {
                _finishCasting = true;
            }
        }

        private void OnAbilityPreparingStart()
        {
            _castingTimer = 0;
            _finishCasting = false;
            Entity.Get<PlayerAnimator>().PlayHandAnimation(_currentCastingActionAbilityConfig.PreparationAnimation);
         

            _castingState = AbilityCastingState.Preparing;
        }

        private void OnNewAbilityChosen(AbilityItemData abilityItemData)
        {
            _newAbility = false;
            _lastAbilityId = abilityItemData.AbilityData.Id;
        }

        private void OnAbilityCastingStart()
        {
            _castingTimer = 0;
            _currentCastingActionAbility.OnInput(true);

            Entity.Get<PlayerAnimator>().PlayHandAnimation(_currentCastingActionAbilityConfig.ActiveAnimation);
            
            _castingState = AbilityCastingState.Casting;
        }

        private void OnAbilityCastingEnd()
        {
            _castingTimer = 0;
            _currentCastingActionAbility.OnInput(false);


            
            _currentCastingActionAbility = null;
            _currentCastingActionAbilityConfig = null;
            _currentAbilityInstance = null;

            Entity.Get<PlayerAnimator>().PlayHandAnimation(HandAnimationType.Idle);

       
            
            _castingState = AbilityCastingState.Idle;
        }
    }
}