using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhHealthRegen : Enhancement
{
    float healAmount;
    float tickTime;
    float duration;

    public override EffectId Type() => EffectId.HealthRegen;

    public override SerializableDictionary<string, float> DataParameters() =>
        new SerializableDictionary<string, float>()
        {
            {healAmountKey, healAmount},
            {tickTimeKey, tickTime},
            {durationKey, duration},
        };


    public EnhHealthRegen(EnhHealthRegenBlueprint blueprint)
    {
        healAmount = blueprint.GenerateHealAmount();
        tickTime = blueprint.GenerateHealPeriod();
        duration = blueprint.GenerateDuration();
    }

    public EnhHealthRegen(SerializableDictionary<string, float> parameters)
    {
        System.Diagnostics.Debug.Assert(parameters.ContainsKey(healAmountKey) && parameters.ContainsKey(tickTimeKey) && parameters.ContainsKey(durationKey));

        healAmount = parameters[healAmountKey];
        tickTime = parameters[tickTimeKey];
        duration = parameters[durationKey];
    }


    private IEnumerator RunHealthRegen(HealthSystem health)
    {
        Debug.Log("Running HealthRegen enhancement");

        float timePassed = 0f;
        health.Heal(healAmount);
        while (timePassed < duration)
        {
            yield return new WaitForSeconds(tickTime);
            health.Heal(healAmount);
            timePassed += tickTime;
        }

        Debug.Log("HealthRegen enhancement ended");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunHealthRegen(receiver.Health));
    }

    public override string ToString()
    {
        return $"{Type()}: {healAmount} each {tickTime} for {duration}";
    }


}
