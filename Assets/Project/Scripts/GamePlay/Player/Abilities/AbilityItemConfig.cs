using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities
{
    [Serializable]
    public class AbilityItemConfig
    {
        public BaseAbilityItem AbilityItem;

        public AbilityItemConfig(BaseAbilityItem abilityItem)
        {
            AbilityItem=abilityItem;
        }
    }
}
