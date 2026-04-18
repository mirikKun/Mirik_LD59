using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities
{
    public class AbilityItemData
    {
        private AbilityData _abilityData;
        public BaseAbilityItem AbilityItem { get; }

        public AbilityData AbilityData => _abilityData;
        
        private AbilityItemData(BaseAbilityItem abilityItem)
        {
            AbilityItem = abilityItem;
        }

        public static AbilityItemData FromConfig(AbilityItemConfig config, int originCasterId)
        {
            if (config == null)
                return null;

            var data = new AbilityItemData(config.AbilityItem);
            data.GetAbilityData(originCasterId);
            return data;
        }

        public static AbilityItemData FromConfig(BaseAbilityItem abilityItem, int originCasterId)
        {
            return FromConfig(new AbilityItemConfig (abilityItem), originCasterId);
        }

        public AbilityData GetAbilityData(int originCasterId)
        {
  
                _abilityData = new AbilityData(AbilityItem.AbilityConfig, originCasterId);
            

            return _abilityData;
        }
    }
}
