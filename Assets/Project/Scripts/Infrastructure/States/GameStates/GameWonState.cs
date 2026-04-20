using Project.Scripts.GamePlay.Windows;
using Project.Scripts.Infrastructure.States.StateInfrastructure;
using UnityEngine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class GameWonState : IState
    {
        private readonly IWindowService _windowService;

        public GameWonState(IWindowService windowService) =>
            _windowService = windowService;

        public void Enter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _windowService.Open(WindowId.Victory);
        }

        public void Exit() =>
            _windowService.Close(WindowId.Victory);
    }
}
