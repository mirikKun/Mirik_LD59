using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Levels.Enum;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.Systems
{
    
    [Serializable]
    public sealed class PlayerSpawnPointBinding
    {
        [SerializeField] private int _id;
        [SerializeField] private string _displayName;
        [SerializeField] private Transform _point;
        [SerializeField] private Scenes _targetScene;

        public int Id => _id;
        public string DisplayName => _displayName;
        public Transform Point => _point;
        public Scenes TargetScene => _targetScene;
    }

    [Serializable]
    public sealed class PlayerSpawnPointsInfo
    {
        [SerializeField] private PlayerSpawnPointBinding _defaultSpawnPoint;
        [SerializeField] private List<PlayerSpawnPointBinding> _additionalSpawnPoints = new();
        
        public PlayerSpawnPointBinding DefaultSpawnPoint => _defaultSpawnPoint;
        public IReadOnlyList<PlayerSpawnPointBinding> AdditionalSpawnPoints => _additionalSpawnPoints;

        public PlayerSpawnPointBinding GetSpawnPoint(int id)
        {
            if(_additionalSpawnPoints.Count<= id) return _defaultSpawnPoint;
            return _additionalSpawnPoints[id];
        }
    }
}
