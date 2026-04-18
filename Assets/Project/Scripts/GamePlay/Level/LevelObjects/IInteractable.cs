using System;
using Project.Scripts.GamePlay.Core.Entity;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public interface IInteractable
    {
        bool NeeedToPress { get; }
        void Interact(BaseEntity entity);
        void HighLight();
        void UnHighLight();
        event Action Interacted;
    }
}

