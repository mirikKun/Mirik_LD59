using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.GamePlay.Player.Abilities;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Inventory.General;

namespace Project.Scripts.GamePlay.Player.Inventory.Systems
{
    public class InventorySystem : IInventorySystem
    {
        private List<AbilitySlot> _activeAbilityItems;
        private List<AbilityItemData> _inactiveAbilityItems;
        private List<AbilityItemData> _allAbilities;
        private int _maxInventorySize;

        public List<AbilitySlot> ActiveAbilities => _activeAbilityItems;
        public List<AbilityItemData> InactiveAbilities => _inactiveAbilityItems;
        public List<AbilityItemData> AllAbilities => _allAbilities;
        public event Action ActiveInventoryChanged;
        public event Action InactiveInventoryChanged;

        public bool IsFull => _maxInventorySize <= _inactiveAbilityItems.Count;

        public bool IsEmpty => _inactiveAbilityItems.Count <= 0;
        public int InventoryCapacity => _maxInventorySize;


        public int GetAmountOfNonDefaultAbilities()
        {
            int amount = 0;
            amount += _inactiveAbilityItems.Count;
            amount += _activeAbilityItems.Count(x => x.SlotType != AbilitySlotType.ActionKey);
            return amount;
        }


        public void SetupInventory(List<AbilitySlot> activeAbilities, List<AbilityItemData> inactiveAbilities,
            int maxInventorySize)
        {
            _maxInventorySize = maxInventorySize;
            _activeAbilityItems = activeAbilities;
            _inactiveAbilityItems = inactiveAbilities;
            _allAbilities = new List<AbilityItemData>();
            _allAbilities.AddRange(_inactiveAbilityItems);
            foreach (AbilitySlot ability in activeAbilities)
            {
                _allAbilities.Add(ability.EquippedAbility);
            }
        }

        public void OnActiveInventoryChanged()
        {
            ActiveInventoryChanged?.Invoke();
        }

        public void AddItem(AbilityItemData item)
        {
            if (item.AbilityItem.SlotType != AbilitySlotType.ActionKey)
            {
                var slotKey= item.AbilityItem.SlotType==AbilitySlotType.Jump?AbilitySlotKey.SpaceAction:AbilitySlotKey.None;
                var activeSlot = new AbilitySlot(slotKey, item);
                _activeAbilityItems.Add(activeSlot);
                _allAbilities.Add(item);
                ActiveInventoryChanged?.Invoke();
                return;
            }

            _inactiveAbilityItems.Add(item);
            _allAbilities.Add(item);
            InactiveInventoryChanged?.Invoke();
        }

        public void SetActiveAbility(AbilitySlot newActiveAbility)
        {
            if (newActiveAbility == null || newActiveAbility.EquippedAbility == null)
                return;
            _activeAbilityItems.Add(newActiveAbility);
            _allAbilities.Add(newActiveAbility.EquippedAbility);
        }

        public void SetInactiveAbility(AbilityItemData activeAbility)
        {
            if (activeAbility == null)
                return;
            _inactiveAbilityItems.Add(activeAbility);
            _allAbilities.Add(activeAbility);
            InactiveInventoryChanged?.Invoke();
        }

        public void RemoveActiveAbility(AbilityItemData activeAbility)
        {
            if (activeAbility == null)
                return;
            AbilitySlot abilitySlot = _activeAbilityItems.Find(x => x.EquippedAbility == activeAbility);
            _activeAbilityItems.Remove(abilitySlot);
            _allAbilities.Remove(activeAbility);
        }


        public void RemoveInactiveAbility(AbilityItemData inactiveAbility)
        {
            if (inactiveAbility == null)
                return;
            _inactiveAbilityItems.Remove(inactiveAbility);
            _allAbilities.Remove(inactiveAbility);
            InactiveInventoryChanged?.Invoke();
        }
    }
}