using Project.Scripts.GamePlay.Common.Time;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Project.Scripts.GamePlay.Core.Input
{
    public class InputReader : PlayerInputActions.IPlayerActions, IInputReader
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };
        public event UnityAction InventoryPressed = delegate { };
        public event UnityAction Esc = delegate { };

        public void ClearPlayerActions()
        {
            Move = delegate { };
            Look = delegate { };
            EnableMouseControlCamera = delegate { };
            DisableMouseControlCamera = delegate { };
            InventoryPressed = delegate { };
            Esc = delegate { };
            Jump = delegate { };
            Dash = delegate { };
            Crouch = delegate { };
            Action1 = delegate { };
            Action2 = delegate { };
            Action3 = delegate { };
            Attack = delegate { };
            AttackAlt = delegate { };
            Click = delegate { };
        }

        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<bool> Dash = delegate { };
        public event UnityAction<bool> Crouch = delegate { };
        public event UnityAction<bool> Action1 = delegate { };
        public event UnityAction<bool> Action2 = delegate { };
        public event UnityAction<bool> Action3 = delegate { };
        public event UnityAction<bool> Action4 = delegate { };
        public event UnityAction<bool> Interact = delegate { };
        public event UnityAction<bool> Attack = delegate { };
        public event UnityAction<bool> AttackAlt = delegate { };
        public event UnityAction<RaycastHit> Click = delegate { };

        private PlayerInputActions _inputActions;
        private ITimeService _timeService;

        public bool IsJumpKeyPressed() => _inputActions.Player.Jump.IsPressed();

        public Vector2 Direction => _inputActions.Player.Move.ReadValue<Vector2>();
        public Vector2 LookDirection => _inputActions.Player.Look.ReadValue<Vector2>();
        public const float HoldDuration = 0.2f;

        public InputReader(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void EnablePlayerActions()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerInputActions();
                _inputActions.Player.SetCallbacks(this);
            }

            _inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        bool IsDeviceMouse(InputAction.CallbackContext context)
        {
            // Debug.Log($"Device name: {context.control.device.name}");
            return context.control.device.name == "Mouse";
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (_timeService.TimeScale <= 0) return;
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Attack.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Attack.Invoke(false);
                    break;
            }
        }

        public void OnAltFire(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    AttackAlt.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    AttackAlt.Invoke(false);
                    break;
            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (IsDeviceMouse(context))
                {
                    var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if (Physics.Raycast(ray.origin, ray.direction, out var hit, 100))
                    {
                        Click.Invoke(hit);
                    }
                }
            }
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Dash.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dash.Invoke(false);
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Crouch.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Crouch.Invoke(false);
                    break;
            }
        }

        public void OnAction1(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Action1.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Action1.Invoke(false);
                    break;
            }
        }

        public void OnAction2(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Action2.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Action2.Invoke(false);
                    break;
            }
        }

        public void OnAction3(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Action3.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Action3.Invoke(false);
                    break;
            }
        }

        public void OnAction4(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Action4.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Action4.Invoke(false);
                    break;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Interact.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Interact.Invoke(false);
                    break;
            }
        }

        public void OnEsc(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Esc?.Invoke();
                    break;
            }
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                InventoryPressed?.Invoke();
            }
        }
    }
}