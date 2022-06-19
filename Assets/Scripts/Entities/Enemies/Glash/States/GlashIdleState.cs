using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlashIdleState : State
{
    private Glash glash;

    private float idleTime;
    private bool isIdleTimeElapsed;
    private bool isTargetFound;

    public GlashIdleState(Glash glash, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();

        glash.Anim.SetBool("Threaten", false);

        idleTime = glash.ChooseIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        isTargetFound = false;
        isIdleTimeElapsed = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        glash.SetAggroTarget();
        isTargetFound = glash.HasTarget;

        isIdleTimeElapsed = Time.time > idleTime + startTime;

        //state changes:

        if (isTargetFound)
            stateMachine.ChangeState(glash.ChaseState);


        else if (isIdleTimeElapsed)
            stateMachine.ChangeState(glash.WanderState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
