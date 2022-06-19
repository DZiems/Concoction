using UnityEngine;

public class GlashRestState : State
{
    private Glash glash;

    private bool isRestTimeElapsed;
    public GlashRestState(Glash glash,  FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();

        glash.Anim.SetBool("Threaten", false);

        Debug.Log($"Glash resting for: {glash.LastUsedAttack.RestTime}");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isRestTimeElapsed = Time.time > glash.LastUsedAttack.RestTime + startTime;


        if (isRestTimeElapsed)
            stateMachine.ChangeState(glash.IdleState);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
