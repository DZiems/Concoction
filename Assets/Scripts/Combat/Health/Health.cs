using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    static readonly float ConsecutiveHitsTimeRequirement = 5.0f;

    [SerializeField] private UIHealthBar UIHealthBar;

    [SerializeField] private int numHitsToCombo = 3;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int defaultShielding = 0;

    public int CurrentHealth { get; private set; }
    public int CurrentShielding { get; private set; }
    public int MaxHealth => maxHealth;


    public event Action onDamaged;
    public event Action onComboed;
    public event Action onHealed;
    public event Action onKilled;


    private float timeSinceDamaged;
    private int numConsecutiveHits;

    private bool isInvulnerable;


    private void Awake()
    {
        CurrentHealth = maxHealth;
        CurrentShielding = defaultShielding;

    }

    private void Start()
    {
        UIHealthBar.Initialize(maxHealth);
    }

    [ContextMenu("Take 20 Damage")]
    public void Take20Damage()
    {
        TakeDamage(20);
    }

    [ContextMenu("Heal 20 Damage")]
    public void Heal20Health()
    {
        Heal(20);
    }

    [ContextMenu("Add 20 Max HP")]
    public void Add20MaxHealth()
    {
        SetMaxHealth(maxHealth + 20);
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;


        HandleHitComboTrigger();


        CurrentHealth -= amount;
        UIHealthBar.UpdateCurrentHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            if (onKilled == null) 
                Debug.LogError($"onKilled is null for {gameObject.name}");
            else
                onKilled();
            return;
        }

        if (onDamaged == null)
            Debug.LogError($"onDamaged is null for {gameObject.name}");
        else
            onDamaged();
        //call update and pass in current health, then check whether health delta is higher or lower, then animate damage or heal
    }

    private void HandleHitComboTrigger()
    {
        if (timeSinceDamaged <= ConsecutiveHitsTimeRequirement)
        {
            numConsecutiveHits++;
            if (numConsecutiveHits >= numHitsToCombo)
            {
                if (onComboed != null)
                    onComboed();
                numConsecutiveHits = 0;
            }
        }
        else
        {
            numConsecutiveHits = 1;
        }
        timeSinceDamaged = 0;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
            if (onHealed == null) 
                Debug.LogError($"onHealed is null for {gameObject.name}");
            else
                onHealed();
        }
        UIHealthBar.UpdateCurrentHealth(CurrentHealth);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        if (newMaxHealth < CurrentHealth)
        {
            CurrentHealth = newMaxHealth;
        }

        UIHealthBar.UpdateMaxHealth(newMaxHealth, CurrentHealth);
    }

    public void MakeInvulnerable(float time)
    {
        StartCoroutine(RunInvulnerability(time));
    }

    private IEnumerator RunInvulnerability(float time)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
    }

    private void Update()
    {
        timeSinceDamaged += Time.deltaTime;
    }
}
