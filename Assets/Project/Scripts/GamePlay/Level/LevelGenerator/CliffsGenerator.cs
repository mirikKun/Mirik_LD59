using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelGenerator
{
    public class CliffsGenerator : MonoBehaviour
    {
        private const int RandomSampleTries = 8000;
        private const int GridResolution = 32;

        [SerializeField] private Cliff[] _cliffPrefabs;
        [SerializeField] private int _numberOfCliffs = 10;
        [SerializeField] private Transform _cliffsParent;

        [SerializeField] private Vector3 _spawnAreaCenter;
        [SerializeField] private Vector3 _spawnAreaSize = new Vector3(40f, 4f, 40f);

        [SerializeField] private Vector3 _exclusionAreaCenter;
        [SerializeField] private Vector3 _exclusionAreaSize = new Vector3(8f, 8f, 8f);

        [SerializeField] private Vector3 _scaleFrom = new Vector3(0.85f, 0.85f, 0.85f);
        [SerializeField] private Vector3 _scaleTo = new Vector3(1.15f, 1.15f, 1.15f);

        [SerializeField] private Vector3 _rotationFrom = new Vector3(-15f, -180f, -15f);
        [SerializeField] private Vector3 _rotationTo = new Vector3(15f, 180f, 15f);

        private void Start()
        {
            if (_cliffsParent == null)
                _cliffsParent = transform;

            GenerateCliffs();
        }

        private void GenerateCliffs()
        {
            var spawnBounds = new Bounds(_spawnAreaCenter, _spawnAreaSize);
            var exclusionBounds = new Bounds(_exclusionAreaCenter, _exclusionAreaSize);

            for (var i = 0; i < _numberOfCliffs; i++)
            {
                var position = SamplePositionGuaranteed(spawnBounds, exclusionBounds);
                Cliff prefab = PickRandomPrefab(_cliffPrefabs);

                var euler = RandomVectorPerAxis(_rotationFrom, _rotationTo);
                var instance = Instantiate(prefab, position, Quaternion.Euler(euler), _cliffsParent);

                instance.gameObject.SetActive(true);
                var scaleMul = RandomVectorPerAxis(_scaleFrom, _scaleTo);
                instance.transform.localScale = Vector3.Scale(prefab.transform.localScale, scaleMul);
            }
        }

        private static Cliff PickRandomPrefab(Cliff[] prefabs)
        {
            var start = Random.Range(0, prefabs.Length);
            for (var i = 0; i < prefabs.Length; i++)
            {
                var p = prefabs[(start + i) % prefabs.Length];
                if (p != null)
                    return p;
            }

            return null;
        }

        private static Vector3 SamplePositionGuaranteed(Bounds spawn, Bounds exclusion)
        {
            for (var attempt = 0; attempt < RandomSampleTries; attempt++)
            {
                var point = RandomPointInBounds(spawn);
                if (!exclusion.Contains(point))
                    return point;
            }

            for (var ix = 0; ix <= GridResolution; ix++)
            {
                for (var iy = 0; iy <= GridResolution; iy++)
                {
                    for (var iz = 0; iz <= GridResolution; iz++)
                    {
                        var point = PointOnSpawnGrid(spawn, ix, iy, iz);
                        if (!exclusion.Contains(point))
                            return point + Random.insideUnitSphere * 0.02f;
                    }
                }
            }

            Debug.LogError(
                $"{nameof(CliffsGenerator)}: spawn volume is fully inside the exclusion box; using spawn center (may overlap exclusion).");
            return spawn.center;
        }

        private static Vector3 RandomPointInBounds(Bounds b)
        {
            return new Vector3(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y),
                Random.Range(b.min.z, b.max.z));
        }

        private static Vector3 PointOnSpawnGrid(Bounds spawn, int ix, int iy, int iz)
        {
            var tx = ix / (float)GridResolution;
            var ty = iy / (float)GridResolution;
            var tz = iz / (float)GridResolution;
            return new Vector3(
                Mathf.Lerp(spawn.min.x, spawn.max.x, tx),
                Mathf.Lerp(spawn.min.y, spawn.max.y, ty),
                Mathf.Lerp(spawn.min.z, spawn.max.z, tz));
        }

        private static Vector3 RandomVectorPerAxis(Vector3 from, Vector3 to)
        {
            return new Vector3(
                Random.Range(from.x, to.x),
                Random.Range(from.y, to.y),
                Random.Range(from.z, to.z));
        }
    }
}
