using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.Input;
using Project.Scripts.GamePlay.Core.Physic.Enums;
using Project.Scripts.GamePlay.Core.Physic.Raycast;
using Project.Scripts.GamePlay.Level.LevelObjects;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Controller
{
    public class PlayerInteractor : EntityComponent
    {
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private float _checkDistance = 2f;
        [SerializeField] private LayerMask _pickableLayer;

        private RaycastSensor _raycastSensor;
        private IInteractable _lastInteractable;
        private Collider _lastDetectedCollider;
        private IInputReader _inputReader;
        private bool _isInteractPressed;

        [Inject]
        private void Construct(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public override void StartEntity()
        {
            _raycastSensor = new RaycastSensor(_raycastOrigin);
            _raycastSensor.Layermask= _pickableLayer;
            _raycastSensor.CastLength = (_checkDistance);
            _raycastSensor.SetCastDirection(CastDirection.Forward);
            _inputReader.Interact += OnInteractInput;
        }

        private void OnInteractInput(bool pressed)
        {
            _isInteractPressed = pressed;
        }

        private void OnDestroy()
        {
            _inputReader.Interact -= OnInteractInput;
        }

        public void Tick()
        {
            _raycastSensor.Cast();
            if (_raycastSensor.HasDetectedHit())
            {
                var currentCollider = _raycastSensor.GetCollider();
                
                // Виконуємо TryGetComponent лише коли колайдер змінився
                if (_lastDetectedCollider != currentCollider)
                {
                    _lastDetectedCollider = currentCollider;
                    
                    if (currentCollider.TryGetComponent<IInteractable>(out var interactable))
                    {
                        if (_lastInteractable != interactable)
                        {
                            UnHighlightLast();
                            _lastInteractable = interactable;
                            interactable.HighLight();
                        }
                    }
                    else
                    {
                        UnHighlightLast();
                        _lastInteractable = null;
                    }
                }

                // Обробка взаємодії, якщо є активний interactable
                if (_lastInteractable != null && _isInteractPressed)
                {
                    _lastInteractable.Interact(Entity);
                    Entity.Get<PlayerController>().SetRespawnPosition(transform.position);
                    UnHighlightLast();
                }
            }
            else
            {
                if (_lastInteractable != null)
                {
                    UnHighlightLast();
                }
                _lastDetectedCollider = null;
            }
        }

        private void UnHighlightLast()
        {
            _lastInteractable?.UnHighLight();
            _lastInteractable = null;
        }
    }
}

