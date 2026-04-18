using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Abilities;
using Project.Scripts.GamePlay.Player.Inventory.General;

namespace Project.Scripts.GamePlay.Player.Inventory.Systems
{
    public interface IInventorySystem
    {
        void SetupInventory(List<AbilitySlot> activeAbilities, List<AbilityItemData> inactiveAbilities,int maxInventorySize);

        void OnActiveInventoryChanged();
        void AddItem(AbilityItemData item);

        void SetActiveAbility(AbilitySlot newActiveAbility);
        void RemoveActiveAbility(AbilityItemData activeAbility);
        void SetInactiveAbility(AbilityItemData activeAbility);
        void RemoveInactiveAbility(AbilityItemData inactiveAbility);
        List<AbilitySlot> ActiveAbilities { get; }
        List<AbilityItemData> InactiveAbilities { get; }
        List<AbilityItemData> AllAbilities { get; }
        event Action ActiveInventoryChanged;

        bool IsFull { get; }
        bool IsEmpty { get; }
        int InventoryCapacity { get; }
        int GetAmountOfNonDefaultAbilities();
        event Action InactiveInventoryChanged;
    }
}