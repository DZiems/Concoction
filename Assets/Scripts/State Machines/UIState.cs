using UnityEngine;

public abstract class UIState
{
    protected UIFiniteStateMachine stateMachine;

    protected float startTime;

    protected UIState(UIFiniteStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }
}
