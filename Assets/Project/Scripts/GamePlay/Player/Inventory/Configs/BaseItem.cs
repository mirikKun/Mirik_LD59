using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Inventory.General;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.Configs
{
    public abstract class BaseItem:ScriptableObject, IInventoryItem
    {
        [SerializeField] private string itemID;
        [SerializeField] private string itemName;
        [SerializeField] private string itemType;
        [SerializeField] private string cost="None";
        [SerializeField] private string itemDescription;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int maxStackSize = 1;
        [SerializeField] private AbilityRarity rarity;
        [SerializeField] private bool isStackable = false;

        public string ID => itemID;
        public string Name => itemName;
        public string Type => itemType;
        public virtual string Cost => cost;
        public string Description => itemDescription;
        public Sprite Icon => itemIcon;
        public int MaxStackSize => maxStackSize;
        public AbilityRarity Rarity => rarity;
        public bool IsStackable => isStackable;
    }
}