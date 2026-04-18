using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities.Configs
{
    [CreateAssetMenu(fileName = "AbilityIconColorsConfig", menuName = "Configs/Player/Abilities/AbilityIconColorsConfig")]
    public class AbilityIconColorsConfig : ScriptableObject
    {
        [SerializeField] private Color _defaultIconColor = Color.white;
        [SerializeField] private Color _limitedUseIconColor = Color.cadetBlue;

        public Color DefaultIconColor => _defaultIconColor;
        public Color LimitedUseIconColor => _limitedUseIconColor;
    }
}
