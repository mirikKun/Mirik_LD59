using Project.Scripts.GamePlay.Level.Enums;
using Project.Scripts.GamePlay.Level.LevelObjects;
using Project.Scripts.GamePlay.Level.LevelObjects.PickUp.Behaviours;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.Factories
{
    public interface IInteractablesFactory
    {
        IInteractable Create(InteractableType type, Vector3 position, Quaternion rotation, Transform parent = null);

        void SetupInteractablesParent(Transform parent);
        AbilityPickUp CreateAbilityPickUp(BaseAbilityItem abilityItem, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null);
    }
}