using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Input;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Inventory.General;
using Project.Scripts.GamePlay.Player.Inventory.Systems;

namespace Project.Scripts.GamePlay.Player.Abilities.Systems
{
    public class AbilitiesSystem : IAbilitiesSystem
    {
        private List<AbilityInstance> _abilities;
        private IInventorySystem _inventorySystem;
        private readonly IInputReader _inputReader;
        private PlayerStartAbilities _playerStartAbilities;

        public List<AbilityInstance> Abilities => _abilities;
        public PlayerStartAbilities PlayerStartAbilities => _playerStartAbilities;

        public event Action AbilitiesListChanged;
        public event Action<AbilitySlotKey> EmptySlotKeyPressed;
        public event Action AbilityRemoved;
        public event Action<AbilityItemData> NewAbilityChosen;

        private bool _emptySlotInputSubscribed;

        private AbilitiesSystem(IInventorySystem inventorySystem, IInputReader inputReader)
        {
            _inventorySystem = inventorySystem;
            _inputReader = inputReader;
        }

        public void Setup(PlayerStartAbilities playerStartAbilities)
        {
            _playerStartAbilities = playerStartAbilities;
            SetupStates();
            _inventorySystem.ActiveInventoryChanged += SetupStates;
            SubscribeEmptySlotInputIfNeeded();
        }
        

        private bool HasAbilityInSlot(AbilitySlotKey slotKey)
        {
            return _abilities.Exists(a => a.SlotKey == slotKey);
        }

        private void SubscribeEmptySlotInputIfNeeded()
        {
            if (_emptySlotInputSubscribed)
                return;
            _emptySlotInputSubscribed = true;

            _inputReader.Jump += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.SpaceAction);
            _inputReader.Crouch += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.CtrlAction);
            _inputReader.Dash += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.ShiftAction);
            _inputReader.Action1 += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.AbilityAction);
            _inputReader.Action2 += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.AbilityAction2);
            _inputReader.Action3 += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.AbilityAction3);
            _inputReader.Attack += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.MouseLeft);
            _inputReader.AttackAlt += (pressed) => OnEmptySlotKeyInput(pressed, AbilitySlotKey.MouseRight);
        }

        private void OnEmptySlotKeyInput(bool pressed, AbilitySlotKey slotKey)
        {
            if (pressed && !HasAbilityInSlot(slotKey))
                EmptySlotKeyPressed?.Invoke(slotKey);
        }


        public void RemoveAbility(AbilityInstance abilityInstance)
        {
            _inventorySystem.RemoveActiveAbility(abilityInstance.AbilityItemData);
            abilityInstance.Clear();
            DisconnectFromInput(abilityInstance);
            _abilities.Remove(abilityInstance);
            AbilityRemoved?.Invoke();
        }

        public void SetupNewActiveAbility(AbilityInstance abilityInstance)
        {
            AbilityItemData newAbilityItem = abilityInstance.AbilityItemData;
            AbilitySlotKey newAbilitySlotKey = abilityInstance.SlotKey;
            AbilitySlot newAbilitySlot = new AbilitySlot(newAbilitySlotKey, newAbilityItem);
            _inventorySystem.RemoveInactiveAbility(newAbilityItem);
            _inventorySystem.SetActiveAbility(newAbilitySlot);
            
            InitAbilitySlot(newAbilitySlot);
            AbilitiesListChanged?.Invoke();
            NewAbilityChosen?.Invoke(newAbilityItem);

        }

        private void SetupStates()
        {
            if (_abilities != null)
            { 
                foreach (AbilityInstance ability in _abilities)
                {
                    DisconnectFromInput(ability);
                }
            }
            
            _abilities = new List<AbilityInstance>();
            foreach (AbilityItemData  abilityData in _playerStartAbilities.GetAbilitiesData())
            {
                AbilityInstance abilityInstance = new AbilityInstance(AbilitySlotKey.None, abilityData);
                _abilities.Add(abilityInstance);
            }

            List<AbilitySlot> activeAbilities = _inventorySystem.ActiveAbilities;
            foreach (AbilitySlot abilitySlot in activeAbilities)
            {
                InitAbilitySlot(abilitySlot);
            }

            AbilitiesListChanged?.Invoke();
        }

        private void InitAbilitySlot(AbilitySlot abilitySlot)
        {
            AbilityInstance abilityInstance = new AbilityInstance(abilitySlot.SlotKey, abilitySlot.EquippedAbility);
            _abilities.Add(abilityInstance);
            ConnectToInput(abilityInstance);
        }

        private void ConnectToInput(AbilityInstance abilityInstance)
        {
            
            switch (abilityInstance.SlotKey)
            {
                case AbilitySlotKey.SpaceAction:
                    _inputReader.Jump += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.CtrlAction:
                    _inputReader.Crouch += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.ShiftAction:
                    _inputReader.Dash += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction:
                    _inputReader.Action1 += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction2:
                    _inputReader.Action2 += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction3:
                    _inputReader.Action3 += abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.MouseLeft:
                    _inputReader.Attack += abilityInstance.OnKeyInput;      
                    break;
                case AbilitySlotKey.MouseRight:
                    _inputReader.AttackAlt += abilityInstance.OnKeyInput;
                    break;
            }
        }

        private void DisconnectFromInput(AbilityInstance abilityInstance)
        {
            switch (abilityInstance.SlotKey)
            {
                case AbilitySlotKey.SpaceAction:
                    _inputReader.Jump -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.CtrlAction:
                    _inputReader.Crouch -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.ShiftAction:
                    _inputReader.Dash -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction:
                    _inputReader.Action1 -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction2:
                    _inputReader.Action2 -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.AbilityAction3:
                    _inputReader.Action3 -= abilityInstance.OnKeyInput;
                    break;
                case AbilitySlotKey.MouseLeft:
                    _inputReader.Attack -= abilityInstance.OnKeyInput;      
                    break;
                case AbilitySlotKey.MouseRight:
                    _inputReader.AttackAlt -= abilityInstance.OnKeyInput;
                    break;
            
            }
        }
    }
}