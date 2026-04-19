using System;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{
    /// <summary>
    /// Simple cumulative wood counter (+1 / -1).
    /// </summary>
    public class WoodResourceController : EntityResourceController
    {
        [SerializeField] private int _wood;

        public event Action<int> WoodAmountChanged;

        public int WoodAmount => _wood;

        public void AddOne()
        {
            _wood++;
            WoodAmountChanged?.Invoke(_wood);
        }

        public bool TryRemoveOne()
        {
            if (_wood <= 0)
                return false;
            _wood--;
            WoodAmountChanged?.Invoke(_wood);
            return true;
        }

        public override void Tick(float deltaTime)
        {
        }
    }
}
