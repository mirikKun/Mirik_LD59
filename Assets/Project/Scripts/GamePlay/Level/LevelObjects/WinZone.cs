using Project.Scripts.GamePlay.Player.Controller;
using Project.Scripts.Infrastructure.States.GameStates;
using Project.Scripts.Infrastructure.States.StateMachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Level.LevelObjects
{
    public class WinZone : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private bool _triggered;

        [Inject]
        private void Construct(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered || !other.TryGetComponent(out PlayerEntity _))
                return;

            _triggered = true;
            OnWin();
        }

        [ContextMenu("On Win")]
        private void OnWin()
        {
            _stateMachine.Enter<GameWonState>();
        }
    }
}
