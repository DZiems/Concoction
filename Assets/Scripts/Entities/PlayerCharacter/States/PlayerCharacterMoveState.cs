using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMoveState : State
{
    private PlayerCharacter character;

    private const string directionAnim = "Direction";
    private const string speedAnim = "Speed";

    public PlayerCharacterMoveState(PlayerCharacter character, FiniteStateMachine stateMachine) : base(stateMachine)
    {
        this.character = character;
    }

    public override void Enter()
    {
        base.Enter();
        HandleMovement();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        HandleMovementAnimation();

        if (ShouldGoIdle())
            stateMachine.ChangeState(character.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleMovement();
    }

    private void HandleMovement()
    {
        character.SetFacingDirection(Vector2.ClampMagnitude(character.Controller.GetDirection(), 1));
        character.SetVelocity(character.CurrentMoveSpeed);
    }

    private void HandleMovementAnimation()
    {
        if (character.FacingDirection.magnitude > 0)
        {
            character.SetSpritesFlipX(character.FacingDirection.x < 0);
            //animator handles cases for when direction is positive, zero, or negative.
            //  Zero between these two vectors implies the player is running left or right;
            //  -1 is straight down (fwd),
            //  +1 is straight up (bckwd)
            character.Anim.SetFloat(directionAnim, Vector2.Dot(character.FacingDirection.normalized, character.transform.up));
        }
        character.Anim.SetFloat(speedAnim, character.FacingDirection.magnitude);
    }

    private bool ShouldGoIdle()
    {
        return character.FacingDirection.magnitude == 0;
    }
}
