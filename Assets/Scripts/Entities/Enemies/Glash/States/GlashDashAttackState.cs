using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlashDashAttackState : State
{
    private Glash glash;

    EnemyAttack dashAttack;
    private bool isFinishedWithAttack;
    private float animationLength;
    public GlashDashAttackState(Glash glash, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("(state entered) Glash attack");

        dashAttack = glash.AttacksCatalogue[glash.DashAttackName];
        dashAttack.Use(animationTrigger: "Attack");

        Debug.Log($"glash animation: {glash.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")}");

        animationLength = glash.Anim.GetCurrentAnimatorStateInfo(0).length;

        dashAttack = glash.AttacksCatalogue[glash.DashAttackName];
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isFinishedWithAttack = Time.time > animationLength + startTime;

        if (isFinishedWithAttack)
            stateMachine.ChangeState(glash.RestState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
