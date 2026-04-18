using System;
using Project.Scripts.GamePlay.Level.LevelObjects.LightHouse;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelGenerator
{
    public class Clif:MonoBehaviour
    {
        public void OnClifShowed()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ClifDestroyer clifDestroyer))
            {
                Debug.Log("On hit by lighthouse");
                OnClifShowed();
            }
        }
    }
}