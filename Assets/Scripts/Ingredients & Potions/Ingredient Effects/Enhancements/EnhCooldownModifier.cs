using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhCooldownModifier : Enhancement
{
    float multiplier;
    float duration;

    private float amountChanged;
    
    public EnhCooldownModifier(EnhCooldownModifierBlueprint blueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_CooldownModifier;

        multiplier = blueprint.GenerateMultiplier();
        duration = blueprint.GenerateDuration();

        amountChanged = 0f;
    }

    public EnhCooldownModifier(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_CooldownModifier;

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(multiplierKey) && parameters.ContainsKey(durationKey));

        multiplier = parameters[multiplierKey];
        duration = parameters[durationKey];


        amountChanged = 0f;
    }

    private IEnumerator RunCooldownModifier(Entity entity)
    {
        amountChanged = entity.CurrentCooldownModifier * multiplier - entity.CurrentCooldownModifier;
        Debug.Log($"Added cooldown modifier: ({amountChanged}), cooldown modifier total is now:  ({entity.CurrentCooldownModifier})");

        entity.CurrentCooldownModifier += amountChanged;

        yield return new WaitForSeconds(duration);
        entity.CurrentCooldownModifier -= amountChanged;
        Debug.Log($"Cooldown modifier enhancement ended, modifier total is now:  ({entity.CurrentCooldownModifier})");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunCooldownModifier(receiver));
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
