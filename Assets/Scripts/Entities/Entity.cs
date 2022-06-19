using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Health, Defense, Resistance")]
    [SerializeField] private BaseHealthData healthData;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 2f;

    [Header("Attack")]
    [SerializeField] private float basePhysicalDmg = 10f;
    [SerializeField] private float baseMagicalDmg = 10f;

    [Header("Cooldown Modifier")]
    [SerializeField] private float baseCooldownModifier = 1f;


    public FiniteStateMachine stateMachine;

    public Vector2 FacingDirection { get; private set; }

    /*****************
    Combat Values 
    *****************/

    //base values
    public float BaseMoveSpeed => baseMoveSpeed;
    public Damage BaseDamage => new Damage(basePhysicalDmg, baseMagicalDmg, 0);
    public float BaseCooldownModifier => baseCooldownModifier;

    //current values
    public HealthSystem Health { get; private set; }
    public float CurrentMoveSpeed;
    public Damage CurrentDamage;
    public float CurrentCooldownModifier;

    /// <summary>(1.0 to 0.0) -- 1.0 meaning receive 100% of whatever status effect </summary>
    public float PercentStatusImmunity; //TODO: implement status reduction via float multiplier
    /// <summary>(Anything above 0.0) -- 1.0 means deal back 100% of damage received as physical dmg</summary>
    public float PhysicalThornsMultiplier; //TODO: implement event within health system to account for thorns status effect. 
    /// <summary>(Anything above 0.0) -- 1.0 means deal back 100% of damage received as magical dmg</summary>
    public float MagicalThornsMultiplier; //TODO: implement event within health system to account for thorns status effect. 
    /// <summary>(Anything above 0.0) -- 1.0 means deal back 100% of damage received as true dmg</summary>
    public float TrueDmgThornsMultiplier; //TODO: implement event within health system to account for thorns status effect. 


    /*****************
    Script References
    *****************/

    //script references
    public Animator Anim { get; private set; }

    //child references
    private UIHealthBar uiHealthBar;


    protected virtual void Awake()
    {
        stateMachine = new FiniteStateMachine();

        Health = new HealthSystem(healthData);
        uiHealthBar = GetComponentInChildren<UIHealthBar>();

        Anim = GetComponent<Animator>();

        if (Anim == null) Debug.LogError($"{gameObject.name} animator is null!");
        if (uiHealthBar == null) Debug.LogError($"{gameObject.name} uiHealthBar is null!");

        InitializeCurrentValues();
    }

    private void InitializeCurrentValues()
    {
        CurrentMoveSpeed = BaseMoveSpeed;
        CurrentDamage = BaseDamage;
        CurrentCooldownModifier = BaseCooldownModifier;

        PercentStatusImmunity = 1f;

    }

    protected virtual void Start()
    {
        uiHealthBar.Initialize(Health);
    }

    //if getting a NullReferenceException, make sure you have called Initialize() on stateMachine.
    protected virtual void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetFacingDirection(Vector2 direction)
    {
        FacingDirection = direction;
    }





    [ContextMenu("Take 20 Physical Damage")]
    public void Take20PhysicalDmg()
    {
        Health.TakeDamage(new Damage(20, 0, 0), this);
    }
    [ContextMenu("Take 20 Magical Damage")]
    public void Take20MagicalDmg()
    {
        Health.TakeDamage(new Damage(0, 20, 0), this);
    }
    [ContextMenu("Take 20 True Damage")]
    public void Take20TrueDmg()
    {
        Health.TakeDamage(new Damage(0, 0, 20), this);
    }
    [ContextMenu("Set Half Health")]
    public void SetHalfHealth()
    {
        Health.Heal(Health.MaxHealth);
        Health.TakeDamage(new Damage(0, 0, Health.MaxHealth / 2), this);
    }
    [ContextMenu("Set Full Health")]
    public void SetFullHealth()
    {
        Health.Heal(Health.MaxHealth);
    }
    [ContextMenu("Add 20 Max Health")]
    public void Add20MaxHealth()
    {
        Health.SetMaxHealth(Health.MaxHealth + 20f);
    }
    [ContextMenu("Add 20 Resistance")]
    public void Add20Resistance()
    {
        Health.AddResistance(20);
    }
    [ContextMenu("Add 20 Defense")]
    public void Add20Defense()
    {
        Health.AddDefense(20);
    }


}
