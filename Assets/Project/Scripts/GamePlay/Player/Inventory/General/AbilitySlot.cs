using System;
using Project.Scripts.GamePlay.Player.Abilities;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.General
{
    [Serializable]
    public class AbilitySlot
    {
        [field:SerializeField] public AbilitySlotKey SlotKey { get; private set; }
        [field:SerializeField] public AbilitySlotType SlotType { get; private set; }

        [field:SerializeField] public AbilityItemConfig EquippedAbilityConfig { get; private set; }

        public AbilityItemData EquippedAbility { get; private set; }

        public AbilitySlot(AbilitySlotKey slotKey, AbilityItemData equippedAbility)
        {
            if (equippedAbility == null) return;
            
            SlotKey = slotKey;
            SlotType = equippedAbility.AbilityItem.SlotType;
            EquippedAbility = equippedAbility;
        }

        public void ResolveEquippedAbility(int originCasterId)
        {
            if (EquippedAbilityConfig != null)
                EquippedAbility = AbilityItemData.FromConfig(EquippedAbilityConfig, originCasterId);
        }
    }
}