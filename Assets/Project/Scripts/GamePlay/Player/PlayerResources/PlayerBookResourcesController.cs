using System;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{
    public class PlayerBookResourcesController : EntityResourceController
    {
        [SerializeField] private int _maxPagesCount = 3;
        [SerializeField] private int _maxStealChargesCount = 4;
        [SerializeField] private float _stealChargesRestoreRate = 0.1f;


        private float _currentPagesCount;
        private float _currentStealChargesCount;
        public float CurrentPagesCount => _currentPagesCount;
        public float CurrentStealChargesCount => _currentStealChargesCount;
        public int MaxStealChargesCount => _maxStealChargesCount;

        public event Action<float> CurrentPagesCountChanged;
        public event Action<float> CurrentStealChargesCountChanged;
        public event Action<int> MaxStealChargesCountChanged;

        public override void StartEntity()
        {
            _currentPagesCount = _maxPagesCount;
            _currentStealChargesCount = _maxPagesCount;

            CurrentPagesCountChanged?.Invoke(_currentPagesCount);
            CurrentStealChargesCountChanged?.Invoke(_currentStealChargesCount);
        }

        public override void Tick(float deltaTime)
        {
            _currentStealChargesCount += deltaTime * _stealChargesRestoreRate;
            _currentStealChargesCount = Math.Clamp(_currentStealChargesCount, 0, _maxStealChargesCount);
            CurrentStealChargesCountChanged?.Invoke(_currentStealChargesCount);
        }

        public bool TrySpendPage(int count = 1)
        {
            if (_currentPagesCount >= count)
            {
                _currentPagesCount -= count;
                return true;
            }
            return false;
        }

        public bool TrySpendStealCharge(int count = 1)
        {
            if (_currentStealChargesCount >= count)
            {
                _currentStealChargesCount -= count;
                CurrentStealChargesCountChanged?.Invoke(_currentStealChargesCount);

                return true;
            }
            return false;
        }
        public void AddPage(float count = 1)
        {
            _currentPagesCount += count;
            _currentPagesCount= Math.Clamp(_currentPagesCount, 0, _maxPagesCount);
            CurrentPagesCountChanged?.Invoke(_currentPagesCount);
        }

        public void AddStealCharge(float count = 1)
        {
            _currentStealChargesCount += count;
            _currentStealChargesCount= Math.Clamp(_currentStealChargesCount, 0, _maxStealChargesCount);
            CurrentStealChargesCountChanged?.Invoke(_currentStealChargesCount);
        }
    }
}