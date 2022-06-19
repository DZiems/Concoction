using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BaseHealthData
{
    public float maxHealth = 100;
    public Shield baseShield;
    public float baseDefense = 0;
    public float baseResistance = 0;
}
public class HealthSystem
{
    BaseHealthData healthData;

    public HealthSystem(BaseHealthData healthData)
    {
        this.healthData = healthData;

        CurrentHealth = MaxHealth;
        CurrentShield = healthData.baseShield;
        CurrentDefense = healthData.baseDefense;
        CurrentResistance = healthData.baseResistance;
    }


    public float CurrentHealth { get; private set; }
    //TODO: implement shielding with a shield bar that appears just above health bar
    public Shield CurrentShield { get; private set; }
    public float CurrentDefense { get; private set; }
    public float CurrentResistance { get; private set; }

    public float MaxHealth => healthData.maxHealth;

    public event Action onHealthChanged;
    public event Action onFlatDamaged;
    public event Action onDotDamaged;
    public event Action onHealed;
    public event Action onNewMaxHealth;
    public event Action onKilled;



    public void TakeDamage(Damage damage, Entity damageDealer)
    {
        if (damage is DotDamage)
        {
            Debug.Log("Health.TakeDamage() received a dotDamage");
            GameManager.Instance.StartCoroutine(TakeDotDamage(damage as DotDamage));
        }
        else
        {
            Debug.Log("Health.TakeDamage() received a flat damage");
            SubtractHealth(damage.EvaluateActualDamageAmount(CurrentDefense, CurrentResistance));

            onFlatDamaged?.Invoke();
        }
    }

    private void SubtractHealth(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
            onKilled?.Invoke();

        onHealthChanged?.Invoke();
    }

    private IEnumerator TakeDotDamage(DotDamage dot)
    {
        int currentTicks = 0;
        //tick out all damage
        while (currentTicks < dot.numTicks)
        {
            yield return new WaitForSeconds(dot.tickTime);
            currentTicks++;

            Debug.Log($"Dealing DoT tick: amount before armor reductions: {dot}");
            SubtractHealth(dot.EvaluateActualDamageAmount(CurrentDefense, CurrentResistance));

            onDotDamaged?.Invoke();
        }
        //tick out any damage remainder
        if (dot.dmgRemainder > 0.1f)
        {
            yield return new WaitForSeconds(dot.tickTime);

            Debug.Log($"Dealing DoT REMAINDER DMG: amount before armor reductions: {dot.dmgRemainder}");
            SubtractHealth(dot.dmgRemainder.EvaluateActualDamageAmount(CurrentDefense, CurrentResistance));

            onDotDamaged?.Invoke();
        }
    }


    public void Heal(float amount)
    {
        if (CurrentHealth == MaxHealth) return;

        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        onHealed?.Invoke();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        healthData.maxHealth = newMaxHealth;
        if (newMaxHealth < CurrentHealth)
        {
            CurrentHealth = newMaxHealth;
        }

        onNewMaxHealth?.Invoke();
    }

    public void AddShield(Shield shield)
    {
        CurrentShield += shield;
    }
    public void SubtractShield(Shield shield)
    {
        CurrentShield -= shield; 
    }




    public void AddDefense(float amount)
    {
        CurrentDefense += amount;
    }
    public void MultiplyDefense(float amount)
    {
        CurrentDefense *= amount;
    }
    public void AddResistance(float amount)
    {
        CurrentResistance += amount;
    }
    public void MultiplyResistance(float amount)
    {
        CurrentResistance *= amount;
    }
}
