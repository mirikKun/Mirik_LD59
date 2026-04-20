using Project.Scripts.GamePlay.Common.Time;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Core.Input;
using Project.Scripts.GamePlay.Input.Service;
using Project.Scripts.Infrastructure.States.StateInfrastructure;
using UnityEngine;

namespace Project.Scripts.Infrastructure.States.GameStates
{
    public class GameLoopState : IState, IUpdateable, IFixedUpdateable, ILateUpdateable
    {
        private readonly ITimeService _timeService;
        private readonly IUpdateService _updateService;
        private IInputReader _inputService;

        public GameLoopState(ITimeService timeService, IUpdateService updateService,IInputReader inputService)
        {
            _inputService = inputService;
            _timeService = timeService;
            _updateService = updateService;
        }

        public void Enter()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            _updateService.UpdateAll(_timeService.DeltaTime);
        }

        public void FixedUpdate()
        {
            _updateService.FixedUpdateAll(_timeService.FixedDeltaTime);
        }

        public void LateUpdate()
        {
            _updateService.LateUpdateAll(_timeService.DeltaTime);
        }

        public void Exit()
        {
            _inputService.ClearPlayerActions();

        }
    }
}