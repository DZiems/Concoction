public class InConfirmedState : ProfileMenuState
{

    public InConfirmedState(ProfileMenu profileMenu, Controller controller, UIFiniteStateMachine stateMachine) : base(profileMenu, controller, stateMachine)
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

        if (controller.Slot0Press)
        {
            stateMachine.ChangeState(profileMenu.mainState);
        }

    }

}
