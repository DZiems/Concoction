using TMPro;
using UnityEngine;

public class InMainSubState : UIState
{
    protected Controller controller;
    protected InMainState parentState;
    protected UIHoverableItem button;

    public InMainSubState(InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, UIHoverableItem button) : base(stateMachine)
    {
        this.controller = controller;
        this.parentState = parentState;
        this.button = button;

        button.Unhover();
    }

    public override void Enter()
    {
        base.Enter();

        button.Hover();
    }

    public override void Exit()
    {
        base.Exit();

        button.Unhover();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

}
