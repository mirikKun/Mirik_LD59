using System;
using Project.Scripts.Common.IdProvider;
using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Player.Abilities.General
{
    public abstract class BaseAbility : IAbility
    {
        protected ActorEntity CasterEntity { get; private set; }

        public int Id { get; protected set; }

        public event Action AbilityExecuted;
        public event Action AbilityChargesEnded;
        public event Action AbilityUnlocked;

        public virtual void Init(ActorEntity caster)
        {
            CasterEntity = caster;
            Id = GetNextId();
        }


        protected virtual int GetNextId() => IdProvider.GetNext<IAbility>();

        public abstract void OnInput(bool pressed);

        public abstract void Execute();

        protected virtual void InvokeAbilityExecuted() => AbilityExecuted?.Invoke();
        protected void InvokeAbilityChargesEnded() => AbilityChargesEnded?.Invoke();
        protected void InvokeAbilityUnlocked() => AbilityUnlocked?.Invoke();
    }
}
