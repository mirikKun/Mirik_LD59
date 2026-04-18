using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GamePlay.Level.LevelObjects.PickUp.Behaviours
{
    public class AbilityPickUp:BaseInteractable
    {
        [SerializeField] private BaseAbilityItem _abilityItem;
        [SerializeField] private Image _icon;
        

        [Inject]
        private void Construct()
        {
        }
        
        private void Start()
        {
            if (_abilityItem)
            {
                _icon.sprite = _abilityItem.Icon;
            }
        }

        public override void Interact(BaseEntity entity)
        {
            //_collectionSystem.TryPickAbility(AbilityItemData.FromConfig(_abilityItem, (int)IdProvider.ConstId.None));
            PlayInteractSound();
            Destroy(gameObject);
            OnInteracted();
        }

        public void SetAbilityItem(BaseAbilityItem abilityItem)
        {
            _abilityItem = abilityItem;
            _icon.sprite = _abilityItem.Icon;

        }
    }
}