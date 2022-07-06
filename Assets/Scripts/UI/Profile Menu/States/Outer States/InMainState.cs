using TMPro;
using UnityEngine;

public class InMainState : ProfileMenuState
{
    public UIFiniteStateMachine innerStateMachine { get; private set; }

    public InHoveringDropdownState hoveringDropdownState { get; private set; }
    public InHoveringConfirmState hoveringConfirmState { get; private set; }
    public InHoveringGoBackState hoveringGoBackState { get; private set; }

    public InMainState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine, MyDropdown dropdown, UIHoverableItem confirmButton, UIHoverableItem goBackButton) : base(profileMenu, controller, stateMachine)
    {
        innerStateMachine = new UIFiniteStateMachine();

        hoveringDropdownState = new InHoveringDropdownState(this, controller, innerStateMachine, dropdown);
        hoveringConfirmState = new InHoveringConfirmState(this, controller, innerStateMachine, confirmButton);
        hoveringGoBackState = new InHoveringGoBackState(profileMenu, this, controller, innerStateMachine, goBackButton);

    }

    public override void Enter()
    {
        base.Enter();

        innerStateMachine.Initialize(hoveringDropdownState);
    }

    public override void Exit()
    {
        base.Exit();

        hoveringConfirmState.Exit();
        hoveringGoBackState.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        innerStateMachine.CurrentState.LogicUpdate();
    }

    public void EnterDropdownState()
    {
        stateMachine.ChangeState(profileMenu.dropdownState);
    }

    public void EnterConfirmedState()
    {
        stateMachine.ChangeState(profileMenu.confirmedState);
    }
}
