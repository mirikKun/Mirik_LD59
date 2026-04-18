using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{
    public class PlayerResourcesController:EntityComponent
    {
        [SerializeField] private List<EntityResourceController> _resources = new List<EntityResourceController>();

        public void Tick(float deltaTime)
        {
            foreach (var resourceController in _resources)
            {
                resourceController.Tick(deltaTime);
            }
        }
    }
}