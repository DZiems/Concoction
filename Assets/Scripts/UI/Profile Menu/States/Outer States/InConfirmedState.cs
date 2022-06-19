public class InConfirmedState : ProfileMenuState
{

    public InConfirmedState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine) : base(profileMenu, controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        profileMenu.Anim.SetBool("Active", false);
    }

    public override void Exit()
    {
        base.Exit();

        profileMenu.Anim.SetBool("Active", true);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (controller.Slot0Press)
        {
            stateMachine.ChangeState(profileMenu.mainState);
        }

    }

}
