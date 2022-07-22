using TMPro;
using UnityEngine;

public class InHoveringDropdownState : InMainSubState
{
    public InHoveringDropdownState(InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, UIHoverableText dropdown) : base(parentState, controller, stateMachine, dropdown)
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
            stateMachine.ChangeState(parentState.hoveringConfirmState);
        }
        else if (controller.VerticalUpPress)
        {
            stateMachine.ChangeState(parentState.hoveringGoBackState);
        }
        else if (controller.InteractPress)
        {
            parentState.EnterDropdownState();
        }
        else if (controller.Slot0Press)
        {
            stateMachine.ChangeState(parentState.hoveringGoBackState);
        }
    }

}
