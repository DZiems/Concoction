using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhMagicalDamage : Enhancement
{ 
    float multiplier;
    float duration;

    private float amountChanged;

    public override EffectId Type() => EffectId.MagicalDamage;

    public override SerializableDictionary<string, float> DataParameters() => 
        new SerializableDictionary<string, float>()
        {
            {multiplierKey, multiplier},
            {durationKey, duration}
        };

    public EnhMagicalDamage(EnhMagicalDamageBlueprint magicalDamageBlueprint)
    {
        multiplier = Random.Range(magicalDamageBlueprint.multiplierMin, magicalDamageBlueprint.multiplierMax);

        duration = Random.Range(magicalDamageBlueprint.durationMin, magicalDamageBlueprint.durationMax);

        amountChanged = 0f;
    }
    public EnhMagicalDamage(SerializableDictionary<string, float> parameters)
    {
        System.Diagnostics.Debug.Assert(parameters.ContainsKey(multiplierKey) && parameters.ContainsKey(durationKey));

        multiplier = parameters[multiplierKey];
        duration = parameters[durationKey];

        amountChanged = 0f;
    }


    private IEnumerator RunMagicDamage(Entity entity)
    {
        amountChanged = entity.CurrentDamage.magical * multiplier - entity.CurrentDamage.magical;
        entity.CurrentDamage.magical += amountChanged;
        Debug.Log($"Added magic damage: ({amountChanged}), damage total is now:  ({entity.CurrentDamage})");

        yield return new WaitForSeconds(duration);

        entity.CurrentDamage.magical -= amountChanged;
        Debug.Log($"Magic damage buff ended, damage total is now: ({entity.CurrentDamage})");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunMagicDamage(receiver));
    }

    public override string ToString()
    {
        return $"{Type()}: {multiplier}x for {duration}";
    }


}
