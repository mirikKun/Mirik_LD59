using Project.Scripts.GamePlay.Level.Configs;
using Project.Scripts.GamePlay.Level.Enums;
using Project.Scripts.GamePlay.Level.LevelObjects;
using Project.Scripts.GamePlay.Level.LevelObjects.PickUp.Behaviours;
using Project.Scripts.GamePlay.Level.LevelObjects.Ships;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using Project.Scripts.Infrastructure.StaticData;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Level.Factories
{
    public class InteractablesFactory : IInteractablesFactory
    {
        private DiContainer _container;
        private InteractablesConfig _config;
        private Transform _interactablesParent;

        [Inject]
        private void Construct(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _config = staticDataService.GetInteractablesConfig();
        }

        public IInteractable Create(InteractableType type, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            GameObject prefab = _config.GetPrefab(type);
            GameObject instance = _container.InstantiatePrefab(prefab, position, rotation, parent);
            IInteractable interactable = instance.GetComponentInChildren<IInteractable>();


            return interactable;
        }

        public void SetupInteractablesParent(Transform parent)
        {
            _interactablesParent = parent;
        }

        public AbilityPickUp CreateAbilityPickUp(BaseAbilityItem abilityItem, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null)
        {
            if (abilityItem == null)
                return null;

            GameObject prefabGo = _config.GetPrefab(InteractableType.AbilityPickUp);
            AbilityPickUp pickUpPrefab = prefabGo.GetComponent<AbilityPickUp>();
            AbilityPickUp pickUp = _container.InstantiatePrefabForComponent<AbilityPickUp>(pickUpPrefab, position, rotation, parent==null?_interactablesParent:parent);
            
            
            pickUp.SetAbilityItem(abilityItem);
            return pickUp;
        }

        public IInteractable CreateShipRopeLoot(Vector3 worldPosition, Transform parent = null)
        {
            GameObject prefabGo = _config.GetPrefab(InteractableType.ShipRopeLoot);
    

            var ropePrefab = prefabGo.GetComponentInChildren<ShipRopeInteractable>(true);
     

            Transform resolvedParent = parent != null ? parent : _interactablesParent;
            ShipRopeInteractable instance = _container.InstantiatePrefabForComponent<ShipRopeInteractable>(
                ropePrefab.gameObject, worldPosition, Quaternion.identity, resolvedParent);
            instance.BeginDriftingAt(worldPosition);
            return instance;
        }

        public IInteractable CreateShipPlankLoot(Vector3 worldPosition, Transform parent = null)
        {
            GameObject prefabGo = _config.GetPrefab(InteractableType.ShipPlankLoot);
            var plankPrefab = prefabGo.GetComponentInChildren<ShipPlankInteractable>(true);
            Transform resolvedParent = parent != null ? parent : _interactablesParent;
            ShipPlankInteractable instance = _container.InstantiatePrefabForComponent<ShipPlankInteractable>(
                plankPrefab.gameObject, worldPosition, Quaternion.identity, resolvedParent);
            instance.BeginDriftingAt(worldPosition);
            return instance;
        }
    }
}
