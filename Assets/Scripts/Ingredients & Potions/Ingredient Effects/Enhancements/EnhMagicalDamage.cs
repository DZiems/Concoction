using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhMagicalDamage : Enhancement
{ 
    float multiplier;
    float duration;

    private float amountChanged;

    public EnhMagicalDamage(EnhMagicalDamageBlueprint magicalDamageBlueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_MagicalDamage;

        multiplier = Random.Range(magicalDamageBlueprint.multiplierMin, magicalDamageBlueprint.multiplierMax);

        duration = Random.Range(magicalDamageBlueprint.durationMin, magicalDamageBlueprint.durationMax);

        amountChanged = 0f;
    }
    public EnhMagicalDamage(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_MagicalDamage;

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
        return $"{Group}: {Type}: {multiplier}x for {duration}";
    }

    public override EffectData GetData()
    {
        var parameters = new SerializableDictionary<string, float>()
        {
            {multiplierKey, multiplier},
            {durationKey, duration}
        };

        return new EffectData(Group, Type, parameters);
    }

}
