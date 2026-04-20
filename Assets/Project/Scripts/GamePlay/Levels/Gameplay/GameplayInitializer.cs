using System;
using System.Collections.Generic;
using Project.Scripts.Common.IdProvider;
using Project.Scripts.GamePlay.Player.Abilities;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.Systems;
using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using Project.Scripts.GamePlay.Player.Inventory.General;
using Project.Scripts.GamePlay.Player.Inventory.Systems;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Levels.Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class GameplayInitializer:MonoBehaviour
    {
        
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private PlayerStartAbilities _playerStartAbilities;
        [SerializeField] private PlayerStartInventory _playerStartInventory;
        private IInventorySystem _inventorySystem;
        private IAbilitiesSystem _abilitiesSystem;


        [Inject]
        private void Construct(IAbilitiesSystem abilitiesSystem, IInventorySystem inventorySystem)
        {
            _abilitiesSystem = abilitiesSystem;
            _inventorySystem = inventorySystem;
        }
        
        private void Start()
        {
            SetupInventory(_playerStartInventory);
            SetupAbilities(_playerStartAbilities);
            _playerEntity.Components.StartEntities();
        }
        private void SetupAbilities(PlayerStartAbilities abilities)
        {
            _abilitiesSystem.Setup(abilities);
        }

        private void SetupInventory(PlayerStartInventory startInventory)
        {
            List<AbilitySlot> activeAbilities = new List<AbilitySlot>(startInventory.ActiveAbilities);
            int playerId = (int)IdProvider.ConstId.Player;

            foreach (AbilitySlot abilitySlot in activeAbilities)
            {
                abilitySlot.ResolveEquippedAbility(playerId);
            }

            List<AbilityItemData> inactiveAbilities = new List<AbilityItemData>();
            foreach (AbilityItemConfig config in startInventory.InactiveAbilities)
            {
                inactiveAbilities.Add(AbilityItemData.FromConfig(config, playerId));
            }

            SetupInventory(activeAbilities, inactiveAbilities, startInventory.InventorySize);
        }
        private void SetupInventory(List<AbilitySlot> activeAbilities, List<AbilityItemData> inactiveAbilities,
            int inventorySize)
        {
            _inventorySystem.SetupInventory(activeAbilities, inactiveAbilities, inventorySize);
        }
    }
}