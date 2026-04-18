using Project.Scripts.GamePlay.Player.Abilities.Enums;
using UnityEngine.Events;

namespace Project.Scripts.GamePlay.Player.Abilities.General
{
    public class AbilityInstance
    {
        public readonly AbilitySlotKey SlotKey;

        public readonly AbilityItemData AbilityItemData;

        public event UnityAction<bool> OnAbilityInput;

        public AbilityInstance(AbilitySlotKey slotKey, AbilityItemData abilityData)
        {
            SlotKey = slotKey;
            AbilityItemData = abilityData;
        }

        public void OnKeyInput(bool isPressed)
        {
            OnAbilityInput?.Invoke(isPressed);
        }
        public void Clear()
        {
            OnAbilityInput = null;
        }
        
        
    }
}