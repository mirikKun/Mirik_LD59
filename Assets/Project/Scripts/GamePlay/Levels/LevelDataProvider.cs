using System;
using Project.Scripts.GamePlay.Level.Systems;
using Project.Scripts.GamePlay.Player.Controller;
using Unity.Cinemachine;
using UnityEngine;

namespace Project.Scripts.GamePlay.Levels
{
    public class LevelDataProvider : ILevelDataProvider
    {
        public Transform PlayerSpawnTransform { get; private set; }

        public Transform LevelGeneratorTransform { get; private set; }

        public Transform LighthouseTarget { get; private set; }

        public PlayerSpawnPointsInfo CurrentLevelSpawnsInfo { get; private set; }


        public PlayerEntity Player { get; private set; }


        public CinemachineCamera MainCamera { get; private set; }

        public event Action OtherBehavioursStarted;


        public void SetPlayerSpawnEntries(PlayerSpawnPointsInfo spawnPointsInfo)
        {
            CurrentLevelSpawnsInfo = spawnPointsInfo;
        }


        public void SetStartPoint(Transform spawnTransform)
        {
            PlayerSpawnTransform = spawnTransform;
        }
        public void SetPlayer(PlayerEntity player)
        {
            Player = player;
        }        
        public void SetLevelGeneratorTransform(Transform levelGeneratorTransform)
        {
            LevelGeneratorTransform = levelGeneratorTransform;
        }

        public void SetLighthouseTarget(Transform lighthouseTarget)
        {
            LighthouseTarget = lighthouseTarget;
        }

        public void SetCamera(CinemachineCamera mainCamera)
        {
            MainCamera = mainCamera;
        }

        public void StartOtherBehaviours()
        {
            OtherBehavioursStarted?.Invoke();
        }
    }
}