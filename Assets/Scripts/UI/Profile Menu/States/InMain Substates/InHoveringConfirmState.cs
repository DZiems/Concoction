using TMPro;
using UnityEngine;

public class InHoveringConfirmState : InMainSubState
{
    private MyDropdown dropdown;
    public InHoveringConfirmState(InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, UIHoverableText button, MyDropdown dropdown) : base(parentState, controller, stateMachine, button)
    {
        this.dropdown = dropdown;
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
            if (dropdown.SelectedItem != null)
                parentState.EnterConfirmedState();
            else
                Debug.Log("Indicate to the user that they need to select a profile still");
        }
        else if (controller.Slot0Press)
        {
            stateMachine.ChangeState(parentState.hoveringGoBackState);
        }
    }
}
