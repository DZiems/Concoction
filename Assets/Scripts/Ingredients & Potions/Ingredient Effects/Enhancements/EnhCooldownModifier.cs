using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhCooldownModifier : Enhancement
{
    float multiplier;
    float duration;

    private float amountChanged;

    public override EffectId Type() => EffectId.CooldownModifier;

    public override SerializableDictionary<string, float> DataParameters() => 
        new SerializableDictionary<string, float>()
        {
            {multiplierKey, multiplier},
            {durationKey, duration}
        };

    public EnhCooldownModifier(EnhCooldownModifierBlueprint blueprint)
    {
        multiplier = blueprint.GenerateMultiplier();
        duration = blueprint.GenerateDuration();

        amountChanged = 0f;
    }

    public EnhCooldownModifier(SerializableDictionary<string, float> parameters)
    {
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
        return $"{Type()}: {multiplier}x for {duration}";
    }

}
