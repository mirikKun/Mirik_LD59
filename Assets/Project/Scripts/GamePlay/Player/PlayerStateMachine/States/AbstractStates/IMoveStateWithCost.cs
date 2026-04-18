namespace Project.Scripts.GamePlay.Player.PlayerStateMachine.States.AbstractStates
{
    public interface IMoveStateWithCost
    {
        bool CanPayCost { get; }
    }
}