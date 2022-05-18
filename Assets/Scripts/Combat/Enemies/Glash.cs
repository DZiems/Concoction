using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glash : MonoBehaviour
{
    [Header("Aggro Parameters")]
    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float aggroDuration = 7f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Attack Parameters")]
    [SerializeField] private int attackDamage = 30;
    [SerializeField] private float timeIndicateAttack = 0.25f;
    [SerializeField] private int contactDamage = 15;

    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float strikeRadius = 1f;
    [SerializeField] private float restAfterAttackDuration = 2f;

    [Header("Prefabs")]
    [SerializeField] private CircularStrikeZone strikeZonePrefab;  

    //navigation variables
    private Character target;
    private Vector3 attackPoint;
    private Vector3 glashAttackDashPoint;

    //cached calculated values
    private Vector3 targetDelta;
    private Vector3 attackDirection;
    private Vector3 direction;
    //cached sq values
    private float sqAggroRange;
    private float sqDashDistRemaining;
    private float sqStrikeRadius;
    private float sqAttackRange;
    private float sqTargetDeltaMagn;

    //flags
    private bool contactFlag;

    //states
    private bool isStartingAttack;
    private bool isAttacking;
    private bool isResting;
    //state timer variables
    private float currentAggroDuration;
    private float currentAttackCooldown;
    private float currentRestDuration;

    //references
    private Animator animator;
    private CircularStrikeZone strikeZone;
    private SpriteRenderer spriteRenderer;
    private Health health;

    private bool hasTarget => target != null;

    private void Awake()
    {
        sqAggroRange = aggroRange * aggroRange;
        sqAttackRange = attackRange * attackRange;
        sqStrikeRadius = strikeRadius * strikeRadius;

        isStartingAttack = false;
        isAttacking = false;
        isResting = false;
        contactFlag = false;

        currentAggroDuration = 0f;
        currentAttackCooldown = 0f;
        currentRestDuration = 0f;



        strikeZone = Instantiate(strikeZonePrefab, Vector3.zero, Quaternion.identity);
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        strikeZone.Initialize(strikeRadius * 2f, attackDamage);

    }

    private void OnEnable()
    {
        health.onDamaged += TakeDamage;
        health.onComboed += TakeCombo;
        health.onKilled += Kill;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerCharacter = collision.collider.GetComponent<Character>();
        if (playerCharacter == null) return;

        if (!contactFlag) return;
        
        playerCharacter.Health.TakeDamage(contactDamage);
        StartCoroutine(AllowContact());
    }

    private IEnumerator AllowContact()
    {
        yield return new WaitForSeconds(1.0f);
        contactFlag = true;
    }

    private void TakeDamage()
    {
        Debug.Log("Glash took damage!");
    }

    private void TakeCombo()
    {
        Debug.Log("Glash was comboed on like a bitch!");
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }

    //track a player every 10s
    //if player within range
    void Update()
    {
        if (isStartingAttack) 
            return;

        currentAggroDuration += Time.deltaTime;
        if (isAttacking)
        {
            HandleAttack();
            return;
        }

        currentAttackCooldown -= Time.deltaTime;
        if (isResting)
        {
            HandleRest();
            return;
        }

        if (hasTarget)
        {
            SetDirection();
            MoveTowardTarget();

            if (IsWithinAttackRange() && IsAttackOffCooldown())
            {
                //TODO: fix attack animation to start from threaten animation
                StartCoroutine(StartAttack());
                return;
            }

            if (currentAggroDuration >= aggroDuration)
            {
                FindTarget();
                currentAggroDuration = 0f;
            }
        }
        else
        {
            FindTarget();
        }
    }


    private void HandleRest()
    {
        currentRestDuration += Time.deltaTime;
        if (currentRestDuration >= restAfterAttackDuration)
        {
            currentRestDuration = 0f;
            isResting = false;
        }
    }

    private IEnumerator StartAttack()
    {
        Debug.Log("Glash is attacking");
        isStartingAttack = true;
        attackPoint = target.transform.position;
        attackDirection = targetDelta.normalized;
        glashAttackDashPoint = attackPoint - (attackDirection * strikeRadius);

        strikeZone.SetPosition(attackPoint);
        strikeZone.Indicate();

        yield return new WaitForSeconds(timeIndicateAttack);
        isStartingAttack = false;
        isAttacking = true;
    }


    private void HandleAttack() 
    {
        var deltaDashAttackPoint = glashAttackDashPoint - transform.position;
        float sqDashDistRemaining =
            (deltaDashAttackPoint.x) * (deltaDashAttackPoint.x) +
            (deltaDashAttackPoint.y) * (deltaDashAttackPoint.y);

        if (sqDashDistRemaining >= sqStrikeRadius)
        {
            DashTowardAttackPoint();
        }
        else
        {
            StrikeAndConcludeAttack();
        }

    }
    private void StrikeAndConcludeAttack()
    {
        animator.SetTrigger("Attack");
        isAttacking = false;
        currentAttackCooldown = attackCooldown;
        StartRest();
    }

    //called by animator on specific frame
    private void OnStrikeFrame()
    {
        strikeZone.Strike();
    }


    private void StartRest()
    {
        currentRestDuration = 0f;
        isResting = true;
    }



    private void FindTarget()
    {
        float sqTargetDelta;
        float sqClosest = float.MaxValue;
        Character closestCharacter = null;
        foreach (Player player in PlayerManager.Instance.CurrentPlayers)
        {
            Character playerCharacter = player.Character;
            var pcPos = playerCharacter.transform.position;

            sqTargetDelta = ((pcPos.x - transform.position.x) * (pcPos.x - transform.position.x)) + ((pcPos.y - transform.position.y) * (pcPos.y - transform.position.y));

            if (sqTargetDelta <= sqAggroRange)
            {
                if (sqTargetDelta < sqClosest)
                {
                    sqClosest = sqTargetDelta;
                    closestCharacter = playerCharacter;
                }
            }
        }

        target = closestCharacter;

        animator.SetBool("Threaten", target != null);
    }



    private void SetDirection()
    {
        targetDelta = target.transform.position - transform.position;
        sqTargetDeltaMagn = Vector3.SqrMagnitude(targetDelta);
        direction = targetDelta.normalized;

        spriteRenderer.flipX = direction.x < 0;
    }

    private void MoveTowardTarget()
    {
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    private void DashTowardAttackPoint()
    {
        transform.position += dashSpeed * Time.deltaTime * attackDirection;
    }

    private bool IsWithinAttackRange()
    {
        return sqTargetDeltaMagn <= sqAttackRange;
    }


    private bool IsAttackOffCooldown()
    {
        return currentAttackCooldown <= 0f;
    }
  

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = new Color(1f, 0.5f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
    
}
