using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlashChaseState : State
{
    private Glash glash;

    private bool isPlayerPastDeAggroRange;
    private bool isAbleToDashAttack;
    EnemyAttack dashAttack;
    public GlashChaseState(Glash glash, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("(state entered) Glash chase");

        glash.Anim.SetBool("Threaten", true);

        glash.FaceTarget();
        glash.MoveAggro();
        dashAttack = glash.DashAttack;
    }

    public override void Exit()
    {
        base.Exit();

        isPlayerPastDeAggroRange = false;
        isAbleToDashAttack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        glash.FaceTarget();
        glash.MoveAggro();
        glash.HandleDeAggroCheck();

        isPlayerPastDeAggroRange = !glash.HasTarget;
        isAbleToDashAttack = glash.HasTarget && dashAttack.IsWithinRange(glash.TargetPos) &&
            dashAttack.IsOffCooldown();


        //state changes:
        if (isPlayerPastDeAggroRange)
            stateMachine.ChangeState(glash.IdleState);

        else if (isAbleToDashAttack)
            stateMachine.ChangeState(glash.DashingState);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

}


