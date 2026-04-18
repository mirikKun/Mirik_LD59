using Project.Scripts.GamePlay.Player.Abilities.Factory;
using Project.Scripts.GamePlay.Player.Abilities.General;
using Project.Scripts.GamePlay.Player.Animations;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities.Configs
{
    public abstract class ActionAbilityConfig : BaseAbilityConfig
    {
        
        [field: SerializeField, Min(0f)] public float PreparationTime { get; private set; }
        [field: SerializeField, Min(0f)] public float ActiveLockDuration { get; private set; }
        [field: SerializeField, Min(0f)] public float Cooldown { get; private set; }
        [field: SerializeField] public HandAnimationType PreparationAnimation { get; private set; } = HandAnimationType.Chanting;
        [field: SerializeField] public HandAnimationType ActiveAnimation { get; private set; } = HandAnimationType.Straight;
        public abstract IAbility CreateAbility(IAbilitiesFactory abilitiesFactory);
    }
}