using System;
using Project.Scripts.GamePlay.Level.LevelGenerator;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class ClifDestroyer:MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Cliff clif))
            {
                clif.OnClifShowed();
            }
        }
    }
}