namespace Project.Scripts.GamePlay.Core.GameplayStateMachine {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}