using Project.Scripts.Common.IdProvider;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities.Configs
{
    [CreateAssetMenu(fileName = "PlayerStartAbilities", menuName = "Configs/Player/Abilities/PlayerStartAbilities")]
    public class PlayerStartAbilities:ScriptableObject
    {
        [field: SerializeField] public BaseAbilityItem[] BaseAbilityItemConfigs { get; private set; }

        public AbilityItemData[] GetAbilitiesData()
        {
            AbilityItemData[] abilitiesData = new AbilityItemData[BaseAbilityItemConfigs.Length];
            int playerCasterId = (int)IdProvider.ConstId.Player;

            for (var i = 0; i < BaseAbilityItemConfigs.Length; i++)
            {
                abilitiesData[i] =
                    AbilityItemData.FromConfig(new AbilityItemConfig(BaseAbilityItemConfigs[i]),playerCasterId);
            }
            return abilitiesData;
        }
    }
}