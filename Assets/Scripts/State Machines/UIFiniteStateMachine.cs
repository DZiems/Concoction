public class UIFiniteStateMachine
{
    public UIState CurrentState { get; private set; }

    public void Initialize(UIState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(UIState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
