using System;
using Project.Scripts.GamePlay.Level.Enums;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.Configs
{
    [CreateAssetMenu(fileName = "InteractablesConfig", menuName = "Configs/Level/InteractablesConfig", order = 1)]
    public class InteractablesConfig : ScriptableObject
    {
        [SerializeField] private InteractablePrefabEntry[] _entries;

        public GameObject GetPrefab(InteractableType type)
        {
            foreach (var entry in _entries)
            {
                if (entry.Type == type)
                    return entry.Prefab;
            }

            Debug.LogError($"InteractablesConfig: prefab for type {type} not found.");
            return null;
        }

        [Serializable]
        public class InteractablePrefabEntry
        {
            [field: SerializeField] public InteractableType Type { get; private set; }
            [field: SerializeField] public GameObject Prefab { get; private set; }
        }
    }
}
