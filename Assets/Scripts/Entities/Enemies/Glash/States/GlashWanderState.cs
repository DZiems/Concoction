using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy will wander at (* 0.2) speed in a random direction.
/// -- Condition(s): isWanderTimeElapsed, isPlayerDetected
/// -- Data: float wanderTimeMin, float wanderTimeMax, float wanderBaseSpeedMultiplier, float aggroRange
/// </summary>
public class GlashWanderState : State
{
    private Glash glash;

    private float wanderTime;
    private bool isTargetFound;
    private bool isWanderTimeElapsed;

    public GlashWanderState(Glash glash, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.glash = glash;
    }

    public override void Enter()
    {
        base.Enter();
        wanderTime = glash.ChooseWanderTime();
        glash.SetRandomWanderDirection();
        glash.MovePassive();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        glash.MovePassive();

        glash.SetAggroTarget();
        isTargetFound = glash.HasTarget;

        isWanderTimeElapsed = Time.time > wanderTime + startTime;

        //state changes:

        if (isTargetFound)
            stateMachine.ChangeState(glash.ChaseState);

        else if (isWanderTimeElapsed)
            stateMachine.ChangeState(glash.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
}

