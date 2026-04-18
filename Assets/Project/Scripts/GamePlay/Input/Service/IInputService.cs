using UnityEngine;

namespace Project.Scripts.GamePlay.Input.Service
{
    public interface IInputService
    {
        float GetVerticalAxis();
        float GetHorizontalAxis();
        bool HasAxisInput();

        bool GetLeftMouseButtonDown();
        Vector2 GetScreenMousePosition();
        Vector2 GetWorldMousePosition();
        bool GetLeftMouseButtonUp();
        bool HasSpaceInput();
    }
}