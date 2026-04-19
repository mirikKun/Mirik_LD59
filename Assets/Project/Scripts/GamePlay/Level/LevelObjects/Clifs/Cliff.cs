using System.Collections;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Level.LevelObjects.LightHouse;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelGenerator
{
    /// <summary>
    /// Dissolve + sink when <see cref="ClifDestroyer"/> overlaps; after destroyer leaves, waits 15–25s
    /// then reappears near the last anchor inside spawn bounds (configured by <see cref="CliffsGenerator"/>).
    /// </summary>
    public class Cliff : MonoBehaviour
    {
        private const int RandomSampleTries = 400;
        private const int GridResolution = 16;

        [Header("References (assign on prefab)")] [SerializeField]
        private Renderer[] _renderers;

        [SerializeField] private Collider[] _colliders;

        [Header("Dissolve")] [SerializeField] private float _dissolveDuration = 1.25f;
        [SerializeField] private float _sinkDistance = 6f;
        [SerializeField] private float _sinkDuration = 2.5f;

        [Header("Respawn")] [SerializeField] private float _hiddenDelayMin = 15f;
        [SerializeField] private float _hiddenDelayMax = 25f;
        [SerializeField] private float _respawnOffsetRadius = 4f;

        private Bounds _spawnBounds;
        private Bounds _exclusionBounds;
        private Vector3 _anchorPosition;
        private Quaternion _anchorRotation;
        private Vector3 _anchorScale;

        private Material[] _dissolveMaterials;
        private int _destroyerOverlapCount;
        private Coroutine _routine;

        private static bool HasDestroyer(Collider c)
        {
            return c.GetComponent<ClifDestroyer>() != null || c.GetComponentInParent<ClifDestroyer>() != null;
        }

        public void Initialize(Bounds spawnBounds, Bounds exclusionBounds)
        {
            _spawnBounds = spawnBounds;
            _exclusionBounds = exclusionBounds;
            _anchorPosition = transform.position;
            _anchorRotation = transform.rotation;
            _anchorScale = transform.localScale;
            _destroyerOverlapCount = 0;

            CacheDissolveMaterials();
            SetDissolve(0f);
        }

        private void CacheDissolveMaterials()
        {
            var list = new List<Material>();
            foreach (var r in _renderers)
            {
                if (r == null)
                    continue;

                foreach (var m in r.materials)
                {
                    list.Add(m);
                }
            }

            _dissolveMaterials = list.ToArray();
        }

        private void SetDissolve(float value)
        {
            if (_dissolveMaterials == null)
                return;

            foreach (var m in _dissolveMaterials)
            {
                m.SetFloat("_Dissolve", value);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasDestroyer(other))
                return;

            _destroyerOverlapCount++;

            if (_routine != null)
                return;

            StartSinkAnimation();
        }

        [ContextMenu("Play Dissolve")]
        private void StartSinkAnimation()
        {
            _routine = StartCoroutine(HideAndRespawnRoutine());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!HasDestroyer(other))
                return;

            _destroyerOverlapCount = Mathf.Max(0, _destroyerOverlapCount - 1);
        }

        private IEnumerator HideAndRespawnRoutine()
        {
            var startPos = transform.position;
            var sunkPos = startPos + Vector3.down * _sinkDistance;
            var elapsed = 0f;
            var hideTotal = Mathf.Max(_dissolveDuration, _sinkDuration);

            while (elapsed < hideTotal)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / hideTotal;
                progress = Mathf.Clamp01(progress);

                SetDissolve( progress);
                transform.position = Vector3.Lerp(startPos, sunkPos, progress);
                yield return null;
            }

            SetDissolve(1);
            transform.position = sunkPos;

            foreach (var c in _colliders)
                c.enabled = false;

            var waitSeconds = Random.Range(_hiddenDelayMin, _hiddenDelayMax);
            var waited = 0f;
            while (waited < waitSeconds)
            {
                if (_destroyerOverlapCount == 0)
                    waited += Time.deltaTime;
                else
                    waited = 0f;

                yield return null;
            }

            var target = SampleRespawnNear(_anchorPosition);
            var riseFrom = target + Vector3.down * _sinkDistance;
            transform.SetPositionAndRotation(riseFrom, _anchorRotation);
            transform.localScale = _anchorScale;

            foreach (var c in _colliders)
                c.enabled = true;

            elapsed = 0f;
            while (elapsed < hideTotal)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / hideTotal;
                progress = Mathf.Clamp01(progress);

                SetDissolve(Mathf.SmoothStep(1f, 0f, progress));
                transform.position = Vector3.Lerp(riseFrom, target, progress);
                yield return null;
            }

            SetDissolve(0f);
            transform.position = target;
            _anchorPosition = target;

            _routine = null;
        }

        private Vector3 SampleRespawnNear(Vector3 basePos)
        {
            var spawn = _spawnBounds;
            var exclusion = _exclusionBounds;
            var r = Mathf.Max(0.1f, _respawnOffsetRadius);

            for (var a = 0; a < RandomSampleTries; a++)
            {
                var offset = Random.insideUnitSphere * Random.Range(0.35f, 1f) * Random.Range(0f, r);
                offset.y = 0f;
                var p = basePos + offset;
                p = ClampInsideBounds(p, spawn);
                if (!exclusion.Contains(p))
                    return p;
            }

            for (var ix = 0; ix <= GridResolution; ix++)
            for (var iy = 0; iy <= GridResolution; iy++)
            for (var iz = 0; iz <= GridResolution; iz++)
            {
                var p = new Vector3(
                    Mathf.Lerp(spawn.min.x, spawn.max.x, ix / (float)GridResolution),
                    Mathf.Lerp(spawn.min.y, spawn.max.y, iy / (float)GridResolution),
                    Mathf.Lerp(spawn.min.z, spawn.max.z, iz / (float)GridResolution));
                var q = basePos + (p - spawn.center) * 0.35f;
                q = ClampInsideBounds(q, spawn);
                if (!exclusion.Contains(q))
                    return q;
            }

            return ClampInsideBounds(basePos, spawn);
        }

        private static Vector3 ClampInsideBounds(Vector3 p, Bounds b)
        {
            return new Vector3(
                Mathf.Clamp(p.x, b.min.x, b.max.x),
                Mathf.Clamp(p.y, b.min.y, b.max.y),
                Mathf.Clamp(p.z, b.min.z, b.max.z));
        }
    }
}