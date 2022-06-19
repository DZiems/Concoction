using UnityEngine;

public class GlashDashingState : State
{
    private Glash glash;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private Vector2 dashThisFrame;
    private float sqDistanceLastUpdate;

    private bool isFinishedWithDash;
    private bool isFinishedWithDashNextUpdate;

    public GlashDashingState(Glash glash, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("(state entered) Glash dashing");
        isFinishedWithDash = false;
        isFinishedWithDashNextUpdate = false;

        glash.SetAttackPoint(glash.TargetPos);
        startPosition = glash.transform.position;
        glash.SetFacingDirection((glash.AttackPoint - startPosition).normalized);

        Vector3 bodyOffset = glash.GetBodyOffset();
        //Vector3 strikeZoneOffset = glash.DashAttackStrikeZone.GetExtentsOffset(glash.FacingDirection);

        endPosition = glash.AttackPoint - bodyOffset;  // - strikeZoneOffset;

        dashThisFrame = glash.Dash();
    }

    public override void Exit()
    {
        base.Exit();

        glash.Anim.SetBool("Threaten", false);

        isFinishedWithDash = false;
        isFinishedWithDashNextUpdate = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isFinishedWithDashNextUpdate = CheckFinishedWithDash();


        //state change:
        if (isFinishedWithDash)
            stateMachine.ChangeState(glash.DashAttackState);
        else
            dashThisFrame = glash.Dash();



        if (isFinishedWithDashNextUpdate)
        {
            isFinishedWithDash = true;
            isFinishedWithDashNextUpdate = false;
        }


        
    }

    private bool CheckFinishedWithDash()
    {
        sqDistanceLastUpdate = Vector2.SqrMagnitude(dashThisFrame);
        return glash.CheckWithinSqDistAmount(endPosition, sqDistanceLastUpdate);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}

