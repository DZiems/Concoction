using TMPro;
using UnityEngine;

public class InHoveringGoBackState : InMainSubState
{
    ProfileMenu profileMenu;
    public InHoveringGoBackState(ProfileMenu profileMenu, InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, UIHoverableText button) : base(parentState, controller, stateMachine, button)
    {
        this.profileMenu = profileMenu;
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
            stateMachine.ChangeState(parentState.hoveringDropdownState);
        }
        else if (controller.VerticalUpPress)
        {
            stateMachine.ChangeState(parentState.hoveringConfirmState);
        }
        else if (controller.InteractPress)
        {
            profileMenu.HandleGoBack();
        }
        else if (controller.Slot0Press)
        {
            profileMenu.HandleGoBack();
        }
    }

}