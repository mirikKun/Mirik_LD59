using Project.Scripts.GamePlay.Player.Abilities.AbilityTypes;
using Project.Scripts.GamePlay.Player.Abilities.Factory;
using Project.Scripts.GamePlay.Player.Abilities.General;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities.Configs
{
    [CreateAssetMenu(fileName = "TimeSlowingAbility", menuName = "Configs/Abilities/TimeSlowingAbility")]

    public class TimeSlowingAbilityConfig:ActionAbilityConfig
    {
        [field:SerializeField] public float Duration { get; private set; } = 2f;
        [field:SerializeField] public AnimationCurve TimeSlowCurve { get; private set; } = AnimationCurve.EaseInOut(0, 1, 1, 0.5f);
        
   
        public override IAbility CreateAbility(IAbilitiesFactory abilitiesFactory)
        {
            TimeSlowAbility ability=abilitiesFactory.CreateAbility<TimeSlowAbility>();
            ability.SetConfig(this);
            return ability;
        }
    }
}