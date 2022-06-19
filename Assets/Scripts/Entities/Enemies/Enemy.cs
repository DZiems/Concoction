using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Entity
{
    [Header("Aggro")]
    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float deAggroRange = 10f;


    [Header("Contact Damage")]
    [SerializeField] private float contactDamageMultiplier = 0.5f;
    [SerializeField] private float timeBetweenContactDamage = 1.0f;
    private bool canDealContactDamage;
    private Damage contactDamage;


    [Header("Idling and Wandering")]
    [SerializeField] private float wanderTimeMin = 2f;
    [SerializeField] private float wanderTimeMax = 5f;
    [SerializeField] private float idleTimeMin = 2f;
    [SerializeField] private float idleTimeMax = 5f;


    [Header("Attacks")]
    [SerializeField] private EnemyAttack[] attacks;
    public Dictionary<string, EnemyAttack> AttacksCatalogue { get; private set; }
    public EnemyAttack LastUsedAttack { get; set; }


    //targets
    public PlayerCharacter Target { get; protected set; }
    public Vector3 TargetPos => Target.transform.position;
    public Vector3 AttackPoint { get; protected set; }
    public bool HasTarget => Target != null;



    protected override void Awake()
    {
        base.Awake();


        //if combat multiplier is 0, just avoid all contact from the start
        canDealContactDamage = contactDamageMultiplier > 0f;
        if (canDealContactDamage)
            contactDamage = CurrentDamage * contactDamageMultiplier;

        SetupAttackCatalogue();
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

        IncrementAttackCooldowns();
    }

    //deal contact damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canDealContactDamage) return;

        var playerCharacter = collision.collider.GetComponent<PlayerCharacter>();
        if (playerCharacter == null) return;

        playerCharacter.Health.TakeDamage(contactDamage, this);
        StartCoroutine(AllowContact());
    }
    private IEnumerator AllowContact()
    {
        yield return new WaitForSeconds(timeBetweenContactDamage);
        canDealContactDamage = true;
    }

    private void SetupAttackCatalogue()
    {
        AttacksCatalogue = new Dictionary<string, EnemyAttack>();
        foreach (var attack in attacks)
        {
            attack.Initialize(this);
            AttacksCatalogue.Add(attack.AttackName, attack);
        }
    }

    private void IncrementAttackCooldowns()
    {
        foreach (var attack in AttacksCatalogue.Values)
        {
            attack.DecrementCooldown();
        }
    }

    public void SetAggroTarget()
    {
        PlayerCharacter playerCharacter = PlayerManager.Instance.Player.Character;
        if (playerCharacter == null)
        {
            Target = null;
            return;
        }

        float sqTargetDelta;
        float sqAggroRange = aggroRange * aggroRange;
        Vector3 pcPos = playerCharacter.transform.position;

        sqTargetDelta =
            ((pcPos.x - transform.position.x) * (pcPos.x - transform.position.x)) +
            ((pcPos.y - transform.position.y) * (pcPos.y - transform.position.y));

        if (sqTargetDelta <= sqAggroRange)
        {
            Target = playerCharacter;
        }
        else
            return;
    }
    public void HandleDeAggroCheck()
    {
        PlayerCharacter playerCharacter = PlayerManager.Instance.Player.Character;
        if (playerCharacter == null)
        {
            Target = null;
            return;
        }

        float sqTargetDelta;
        float sqDeAggroRange = deAggroRange * deAggroRange;
        Vector3 pcPos = playerCharacter.transform.position;

        sqTargetDelta =
            ((pcPos.x - transform.position.x) * (pcPos.x - transform.position.x)) +
            ((pcPos.y - transform.position.y) * (pcPos.y - transform.position.y));

        if (sqTargetDelta > sqDeAggroRange)
        {
            Target = null;
        }
        else
            return;
    }

    public void SetAttackPoint(Vector3 attackPoint)
    {
        AttackPoint = attackPoint;
    }

    public float ChooseIdleTime()
    {
        return UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
    }
    public float ChooseWanderTime()
    {
        return UnityEngine.Random.Range(wanderTimeMin, wanderTimeMax);
    }

    public void SetRandomWanderDirection()
    {
        SetFacingDirection(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized);
    }


    public virtual void FaceTarget()
    {
        if (!HasTarget) return;

        SetFacingDirection((TargetPos - transform.position).normalized);
    }

    public virtual void MoveAggro()
    {
        Vector2 movement = FacingDirection * CurrentMoveSpeed * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y, 0);
    }

    public virtual void MovePassive()
    {
        Vector2 movement = FacingDirection * CurrentMoveSpeed * Time.deltaTime * 0.2f;
        transform.position += new Vector3(movement.x, movement.y, 0);
    }


    public bool CheckWithinSqDistAmount(Vector3 position, float sqDistAmount)
    {
        Vector2 positionDelta = position - transform.position;

        float sqDistanceRemaining = Vector2.SqrMagnitude(positionDelta);

        return sqDistanceRemaining <= sqDistAmount;
    }


}




