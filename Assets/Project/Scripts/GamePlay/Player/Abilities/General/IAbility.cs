using System;
using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Player.Abilities.General
{
    public interface IAbility
    {
        void Init(ActorEntity caster);
        void OnInput(bool pressed);
        
        void Execute();
        public int Id { get;  }
        event Action AbilityExecuted;
        event Action AbilityChargesEnded;
        event Action AbilityUnlocked;
    }
}