using System;
using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public interface IInteractable
    {
        void Interact(BaseEntity entity);
        void HighLight();
        void UnHighLight();
        event Action Interacted;
    }
}

