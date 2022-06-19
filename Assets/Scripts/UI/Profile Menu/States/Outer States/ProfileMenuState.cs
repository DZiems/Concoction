using UnityEngine;

public abstract class ProfileMenuState : UIState
{
    protected Controller controller;
    protected ProfileMenu profileMenu;

    protected ProfileMenuState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.profileMenu = profileMenu;
        this.controller = controller;
    }

    public override void Enter()
    {
        startTime = Time.time;
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {

    }

}
