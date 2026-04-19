using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.GamePlay.Player.PlayerResources;
using TMPro;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.HUD
{
    public class WoodResourceHudView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _woodCanvasGroup;
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private TextMeshProUGUI _woodCount;

        private WoodResourceController _wood;

        private void Awake()
        {
            _woodCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            if (!_playerEntity.TryGet(out WoodResourceController wood))
                return;

            _wood = wood;
            _wood.WoodAmountChanged += OnWoodAmountChanged;
            _woodCount.text = _wood.WoodAmount.ToString();
        }

        private void OnDisable()
        {
            _wood.WoodAmountChanged -= OnWoodAmountChanged;

            _woodCanvasGroup.alpha = 0f;
        }

        private void OnWoodAmountChanged(int amount)
        {
            _woodCanvasGroup.alpha = 1f;


            if (_woodCount != null)
                _woodCount.text = amount.ToString();
        }
    }
}