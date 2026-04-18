using System;
using UnityEngine;

namespace Project.Scripts.GamePlay.Common.Movement
{
    public interface IBaseMover
    {
        void ApplyForce(Vector3 force);
        void Teleport(Vector3 position);
        bool IsFlying { get; }
        event Action Teleported;
        
    }
}