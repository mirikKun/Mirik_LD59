using Project.Scripts.GamePlay.Player.PlayerStateMachine.StateConfigs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities.Configs
{
    [CreateAssetMenu(fileName = "MovingAbility", menuName = "Configs/Abilities/MovingAbility")]
    public class MovingAbilityConfig : BaseAbilityConfig
    {
        [field: SerializeField] public BaseMoveStateConfig MovementMoveStateConfig { get; private set; }
        
    }
}