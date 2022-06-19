using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glash : Enemy
{
    public string DashAttackName => "Dash Attack";

    //[Header("Strike Zone")]
    //[SerializeField] private CircleStrikeZone strikeZonePrefab;

    [Header("Dash Attack Specs")]
    [SerializeField] private float dashBaseSpeedMultiplier = 4f;


    public EnemyAttack DashAttack => AttacksCatalogue[DashAttackName];
    //public CircleStrikeZone DashAttackStrikeZone { get; private set; }

    public GlashIdleState IdleState { get; private set; }
    public GlashWanderState WanderState { get; private set; }
    public GlashChaseState ChaseState { get; private set; }
    public GlashDashingState DashingState { get; private set; }
    public GlashDashAttackState DashAttackState { get; private set; }
    public GlashRestState RestState { get; private set; }

    private SpriteRenderer spRend;
    private CircleCollider2D circleCollider2D;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new GlashIdleState(this, stateMachine);
        WanderState = new GlashWanderState(this, stateMachine);
        ChaseState = new GlashChaseState(this, stateMachine);
        DashingState = new GlashDashingState(this, stateMachine);
        DashAttackState = new GlashDashAttackState(this, stateMachine);
        RestState = new GlashRestState(this, stateMachine);


        //DashAttackStrikeZone = Instantiate(strikeZonePrefab);
        stateMachine.Initialize(IdleState);
        
        spRend = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public Vector2 Dash()
    {
        Vector2 dash = FacingDirection * CurrentMoveSpeed * Time.deltaTime * dashBaseSpeedMultiplier;
        transform.position += new Vector3(dash.x, dash.y, 0);

        return dash;
    }

    public void AnimCall_OnAttackFrame()
    {
        Strike(DashAttack, AttackPoint);
    }

    private void Strike(EnemyAttack attack, Vector3 attackPoint)
    {
        throw new NotImplementedException();
    }

    protected override void Update()
    {
        base.Update();

        spRend.flipX = FacingDirection.x < 0;
    }


    public Vector2 GetBodyOffset()
    {
        return FacingDirection * circleCollider2D.radius * transform.localScale.x;
    }


}
