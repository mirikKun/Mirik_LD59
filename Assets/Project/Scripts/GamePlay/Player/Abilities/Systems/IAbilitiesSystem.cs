using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Enums;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Inventory.General;

namespace Project.Scripts.GamePlay.Player.Abilities.Systems
{
    public interface IAbilitiesSystem
    {
        void Setup(PlayerStartAbilities playerStartAbilities);
        void RemoveAbility(AbilityInstance abilityInstance);
        List<AbilityInstance> Abilities { get; }
        PlayerStartAbilities PlayerStartAbilities { get; }
        event Action AbilitiesListChanged;
        event Action<AbilitySlotKey> EmptySlotKeyPressed;
        void SetupNewActiveAbility(AbilityInstance abilityInstance);
        event Action<AbilityItemData> NewAbilityChosen;
        event Action AbilityRemoved;
    }
}