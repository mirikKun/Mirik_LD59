using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Enums;

namespace Project.Scripts.GamePlay.Player.Inventory.General
{
    public interface IAbilityItem:IInventoryItem
    {
        public BaseAbilityConfig AbilityConfig { get;  }
        public AbilitySlotType SlotType { get;   }
    }
}