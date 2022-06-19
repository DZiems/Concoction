using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterIdleState : State
{

    PlayerCharacter character;

    bool shouldGoToMove;
    public PlayerCharacterIdleState(PlayerCharacter playerCharacter, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.character = playerCharacter;
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
        shouldGoToMove = character.Controller.GetDirection().magnitude != 0;

        if (shouldGoToMove)
            stateMachine.ChangeState(character.moveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
