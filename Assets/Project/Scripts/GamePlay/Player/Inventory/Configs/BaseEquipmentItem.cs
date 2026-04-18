using Project.Scripts.GamePlay.Player.Inventory.Enums;
using Project.Scripts.GamePlay.Player.Inventory.General;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.Configs
{
    public class BaseEquipmentItem: BaseItem, IEquippableItem
    {        
        [SerializeField] private EquipmentSlot equipSlot;
        [SerializeField] private GameObject equipmentPrefab;

        public EquipmentSlot EquipSlot => equipSlot;
        
        public virtual void Equip(GameObject character)
        {
            // Базовая логика экипировки предмета
            Debug.Log($"Экипирован предмет {Name} на персонажа {character.name}");
        }

        public virtual void Unequip(GameObject character)
        {
            // Базовая логика снятия предмета
            Debug.Log($"Снят предмет {Name} с персонажа {character.name}");
        }
        
    }
}