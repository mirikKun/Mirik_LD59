using System;
using System.Collections.Generic;
using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class BaseInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<GameObject> _views = new();
        [SerializeField] private int _outlineLayer = 23;
        [SerializeField] private SoundData _interactSound;

        [SerializeField] private bool _oneTimeUse;
        [SerializeField] private bool _needToPress;

        private readonly List<int> _previousViewLayers = new();
        private ISoundsSystem _soundsSystem;
        protected bool _isActive=true;
        public bool NeeedToPress=>_needToPress;



        [Inject]
        private void Construct(ISoundsSystem soundsSystem) =>
            _soundsSystem = soundsSystem;

        protected virtual void Awake()
        {
            CacheViewLayer();
        }

        protected void CacheViewLayer()
        {
            _previousViewLayers.Clear();
            if (_views == null)
                return;
            foreach (GameObject view in _views)
                _previousViewLayers.Add(view != null ? view.layer : 0);
        }

        protected virtual bool CanInteractHighlight() => _isActive;


        public virtual void Interact(BaseEntity entity)
        {
            PlayInteractSound();
            OnInteracted();
        }
        public void SetIsActive(bool isActive)
        {
            _isActive = isActive;
        }

        protected void PlayInteractSound()
        {
            if (_soundsSystem == null )
                return;
            _soundsSystem.Play(_interactSound);
        }

        public virtual void HighLight()
        {
            if (!CanInteractHighlight() || _views == null || _views.Count == 0)
                return;
            for (var i = 0; i < _views.Count; i++)
            {
                GameObject view = _views[i];
                if (view == null)
                    continue;
                view.layer = _outlineLayer;
            }
        }

        public virtual void UnHighLight()
        {
            if (!CanInteractHighlight() || _views == null || _views.Count == 0)
                return;
            for (var i = 0; i < _views.Count; i++)
            {
                GameObject view = _views[i];
                if (view == null)
                    continue;
                if (i < _previousViewLayers.Count)
                    view.layer = _previousViewLayers[i];
            }
        }

        public event Action Interacted;

        protected void OnInteracted()
        {
            Interacted?.Invoke();
            if (_oneTimeUse)
            {
                _isActive = false;
                Destroy(gameObject);
            }
        }
    }
}