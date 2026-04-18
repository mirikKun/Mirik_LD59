using System;
using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.PlayerResources
{
    public class PlayerManaController : EntityResourceController
    {
        [SerializeField] private float _maxMana = 100;
        [SerializeField] private float _manaRestoreRate = 15;
        [SerializeField] private float _delayBeforeManaRecovery = 0.6f;

        private float _currentMana=1;
        private float _timeSinceLastManaSpend;
        public float CurrentMana => _currentMana;
        public event Action<float> ManaChanged;
        public event Action ManaEnded;

   
        public override void StartEntity()
        {
            _currentMana = _maxMana;
            _timeSinceLastManaSpend = _delayBeforeManaRecovery;
            ManaChanged?.Invoke(_currentMana / _maxMana);
        }

     

        public void SpendMana(float spentMana)
        {
            _timeSinceLastManaSpend = 0f;
            _currentMana -= spentMana;
            if (_currentMana < 0)
            {
                _currentMana = 0;
                ManaEnded?.Invoke();
            }

            ManaChanged?.Invoke(_currentMana / _maxMana);
        }

        public override void Tick(float deltaTime)
        {
            _timeSinceLastManaSpend += deltaTime;
            
            if (_timeSinceLastManaSpend >= _delayBeforeManaRecovery)
            {
                _currentMana += _manaRestoreRate * deltaTime;
                if (_currentMana > _maxMana) _currentMana = _maxMana;
                ManaChanged?.Invoke(_currentMana / _maxMana);
            }
        }
    }
}