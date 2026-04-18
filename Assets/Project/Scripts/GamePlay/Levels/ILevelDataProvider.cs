using System;
using Project.Scripts.GamePlay.Level.Systems;
using Project.Scripts.GamePlay.Player.Controller;
using Unity.Cinemachine;
using UnityEngine;

namespace Project.Scripts.GamePlay.Levels
{
    public interface ILevelDataProvider
    {
        PlayerSpawnPointsInfo CurrentLevelSpawnsInfo { get; }

        void SetPlayerSpawnEntries(PlayerSpawnPointsInfo spawnPointsInfo);
        

        Transform PlayerSpawnTransform { get; }
        Transform LevelGeneratorTransform { get; }
        CinemachineCamera MainCamera { get; }
        PlayerEntity Player { get; }
        void SetStartPoint(Transform spawnTransform);
        void SetLevelGeneratorTransform(Transform levelGeneratorTransform);
        void SetCamera(CinemachineCamera mainCamera);
        void SetPlayer(PlayerEntity player);

        event Action OtherBehavioursStarted;
        void StartOtherBehaviours();
    }
}