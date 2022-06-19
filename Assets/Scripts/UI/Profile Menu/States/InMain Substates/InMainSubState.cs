using TMPro;
using UnityEngine;

public class InMainSubState : UIState
{
    protected Controller controller;
    protected InMainState parentState;
    protected ProfileMenu profileMenu;
    protected Animator buttonAnim;
    protected TextMeshProUGUI buttonText;

    public InMainSubState(ProfileMenu profileMenu, InMainState parentState, Controller controller, UIFiniteStateMachine stateMachine, Animator buttonAnim, TextMeshProUGUI buttonText) : base(stateMachine)
    {
        this.controller = controller;
        this.profileMenu = profileMenu;
        this.parentState = parentState;
        this.buttonAnim = buttonAnim;
        this.buttonText = buttonText;

        StopHover();
    }

    public override void Enter()
    {
        base.Enter();
        StartHover();
    }

    public override void Exit()
    {
        base.Exit();

        StopHover();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public void StartHover()
    {
        buttonAnim.SetBool("IsHovered", true);
        buttonText.color = ColorPallete.selectedColor;
    }
    public void StopHover()
    {
        buttonAnim.SetBool("IsHovered", false);
        buttonText.color = ColorPallete.unselectedColor;
    }
}
