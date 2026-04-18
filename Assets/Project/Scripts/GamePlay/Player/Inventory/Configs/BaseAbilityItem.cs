using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Inventory.General;
using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Inventory.Configs
{
    [CreateAssetMenu(fileName = "BaseAbilityItem", menuName = "Configs/Inventory/BaseAbilityItem", order = 1)]
    public class BaseAbilityItem : BaseItem, IAbilityItem
    {
        [SerializeField] private AbilitySlotType _slotType;

        [SerializeField] private BaseAbilityConfig _abilityConfig;
        public BaseAbilityConfig AbilityConfig => _abilityConfig;
        public AbilitySlotType SlotType => _slotType;
        public override string Cost => GetCost();

        private string GetCost()
        {
         if (_abilityConfig is MovingAbilityConfig movingAbilityConfig &&
                     movingAbilityConfig.MovementMoveStateConfig is IMoveStateWithCostConfig { ManaCost: > 0 } moveStateWithCostConfig)
            {
                return $"<color=blue><b>{moveStateWithCostConfig.ManaCost}</b> mana</color> per use";
            }
            else
            {
                return base.Cost;
            }
        }
    }
}