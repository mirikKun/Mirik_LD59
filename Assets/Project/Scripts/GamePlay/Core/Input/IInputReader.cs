using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.GamePlay.Core.Input
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        Vector2 LookDirection { get; }
        void EnablePlayerActions();
        event UnityAction<bool> Jump;
        event UnityAction<bool> Dash;
        event UnityAction<bool> Crouch;
        event UnityAction<bool> Action1;
        event UnityAction<bool> Action2;
        event UnityAction<bool> Action3;
        event UnityAction<bool> Action4;
        event UnityAction<bool> Interact;

        event UnityAction<bool> Attack ;
        event UnityAction<bool>AttackAlt ;
        event UnityAction Esc;
        event UnityAction InventoryPressed;
    }
}