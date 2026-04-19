using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Level.LevelObjects.Ships
{
    public class ShipsController : MonoBehaviour, IGameUpdateable
    {
        [SerializeField] private ShipsInfo[] _ships;
        [SerializeField] private ShipRoute[] _routes;
        [SerializeField] private float _spawnInterval = 6f;
        [SerializeField] private Transform _shipsParent;

        private DiContainer _container;
        private IUpdateService _updateService;
        private float _spawnTimer;
        private int _lastRouteIndex = -1;

        [Inject]
        private void Construct(DiContainer container, IUpdateService updateService)
        {
            _container = container;
            _updateService = updateService;
        }

        private void Start()
        {
            _spawnTimer = 0f;
            _updateService.EnemiesUpdate.Register(this);
        }

        private void OnDestroy()
        {
            _updateService.EnemiesUpdate.Unregister(this);
        }

        public void GameUpdate(float deltaTime)
        {
            _spawnTimer -= deltaTime;
            while (_spawnTimer <= 0f)
            {
                TrySpawnShip();
                _spawnTimer += _spawnInterval;
            }
        }

        private void TrySpawnShip()
        {
            var prefab = PickShipPrefab();
            var routeIndex = PickNextRouteIndex();
            var route = _routes[routeIndex];
            var from = route.From.position;
            var to = route.To.position;
            var ship = _container.InstantiatePrefabForComponent<Ship>(
                prefab.gameObject, from, Quaternion.identity, _shipsParent);
            ship.ConfigureRoute(from, to);
        }

        private int PickNextRouteIndex()
        {
            if (_routes.Length == 1)
                return 0;

            int index;
            do
            {
                index = Random.Range(0, _routes.Length);
            } while (index == _lastRouteIndex);

            _lastRouteIndex = index;
            return index;
        }

        private Ship PickShipPrefab()
        {
            var totalWeight = 0f;
            foreach (var entry in _ships)
                totalWeight += Mathf.Max(0f, entry.Weight);

            var roll = Random.Range(0f, totalWeight);
            foreach (var entry in _ships)
            {
                roll -= Mathf.Max(0f, entry.Weight);
                if (roll <= 0f)
                    return entry.ShipPrefab;
            }

            return _ships[0].ShipPrefab;
        }
    }

    [System.Serializable]
    public class ShipsInfo
    {
        public Ship ShipPrefab;
        public float Weight = 1f;
    }
}
