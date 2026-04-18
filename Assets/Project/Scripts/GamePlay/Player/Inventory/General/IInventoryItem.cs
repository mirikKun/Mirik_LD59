using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.General
{
    public interface IInventoryItem
    {
        string ID { get; }
        string Name { get; }
        string Type { get; }
        string Cost { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}