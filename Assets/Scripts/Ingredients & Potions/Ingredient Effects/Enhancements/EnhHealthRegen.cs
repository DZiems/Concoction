using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhHealthRegen : Enhancement
{
    float healAmount;
    float tickTime;
    float duration;

    public EnhHealthRegen(EnhHealthRegenBlueprint blueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_HealthRegen;

        healAmount = blueprint.GenerateHealAmount();
        tickTime = blueprint.GenerateHealPeriod();
        duration = blueprint.GenerateDuration();
    }

    public EnhHealthRegen(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_HealthRegen;

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
        return $"{Group}: {Type}: {healAmount} each {tickTime} for {duration}";
    }

    public override EffectData GetData()
    {
        var parameters = new SerializableDictionary<string, float>()
        {
            {healAmountKey, healAmount},
            {tickTimeKey, tickTime},
            {durationKey, duration},
        };

        return new EffectData(Group, Type, parameters);
    }

}
