using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Abilities;
using Project.Scripts.GamePlay.Player.Inventory.General;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.Configs
{
    [CreateAssetMenu(fileName = "PlayerStartInventory", menuName = "Configs/Inventory/Player Start Inventory")]
    public class PlayerStartInventory:ScriptableObject
    {
        [field:SerializeField] public int InventorySize { get; private set; } = 10;
        [field:SerializeField] public List<AbilitySlot> ActiveAbilities { get; private set; } = new List<AbilitySlot>();
        [field:SerializeField] public List<AbilityItemConfig> InactiveAbilities { get; private set; } = new List<AbilityItemConfig>();
        
        
        
    }
}