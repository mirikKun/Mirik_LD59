using Project.Scripts.GamePlay.Core.GameplayStateMachine;
using Project.Scripts.GamePlay.Windows;
using UnityEngine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class GameWonState : IState
    {
        private IWindowService _windowService;


        public GameWonState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Enter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _windowService.Open;
        }



        public void Exit()
        {
        }
    }
}