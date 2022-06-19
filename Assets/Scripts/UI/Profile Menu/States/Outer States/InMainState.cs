using TMPro;
using UnityEngine;

public class InMainState : ProfileMenuState
{
    public UIFiniteStateMachine innerStateMachine { get; private set; }

    public InHoveringDropdownState hoveringDropdownState { get; private set; }
    public InHoveringConfirmState hoveringConfirmState { get; private set; }
    public InHoveringGoBackState hoveringGoBackState { get; private set; }

    public InMainState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine, Animator dropdownAnimator, TextMeshProUGUI dropdownText, Animator confirmAnimator, TextMeshProUGUI confirmText, Animator goBackAnimator, TextMeshProUGUI goBackText) : base(profileMenu, controller, stateMachine)
    {
        innerStateMachine = new UIFiniteStateMachine();

        hoveringDropdownState = new InHoveringDropdownState(profileMenu, this, controller, innerStateMachine, dropdownAnimator, dropdownText);
        hoveringConfirmState = new InHoveringConfirmState(profileMenu, this, controller, innerStateMachine, confirmAnimator, confirmText);
        hoveringGoBackState = new InHoveringGoBackState(profileMenu, this, controller, innerStateMachine, goBackAnimator, goBackText);

    }

    public override void Enter()
    {
        base.Enter();

        innerStateMachine.Initialize(hoveringDropdownState);
    }

    public override void Exit()
    {
        base.Exit();

        hoveringDropdownState.StopHover();
        hoveringConfirmState.StopHover();
        hoveringGoBackState.StopHover();
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
