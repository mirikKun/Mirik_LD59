using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Entity
{
    public abstract class EntityComponent:MonoBehaviour
    {
        protected ActorEntity Entity;

  

        public virtual void InitEntity(ActorEntity entity)
        {
            Entity= entity;
        }

        public virtual void StartEntity(){}
    }
}