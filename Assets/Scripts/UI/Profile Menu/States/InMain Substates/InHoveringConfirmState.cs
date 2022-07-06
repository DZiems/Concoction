using TMPro;
using UnityEngine;

public class InHoveringConfirmState : InMainSubState
{
    public InHoveringConfirmState(InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, UIHoverableItem button) : base(parentState, controller, stateMachine, button)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (controller.VerticalDownPress)
        {
            stateMachine.ChangeState(parentState.hoveringGoBackState);
        }
        else if (controller.VerticalUpPress)
        {
            stateMachine.ChangeState(parentState.hoveringDropdownState);
        }
        else if (controller.InteractPress)
        {
            parentState.EnterConfirmedState();
        }
        else if (controller.Slot0Press)
        {
            stateMachine.ChangeState(parentState.hoveringGoBackState);
        }
    }
}
