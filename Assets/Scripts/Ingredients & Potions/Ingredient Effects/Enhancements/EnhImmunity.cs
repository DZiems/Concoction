using System.Collections;
using UnityEngine;

public class EnhImmunity : Enhancement
{

    float percentage;
    float duration;


    public override EffectId Type() => EffectId.Immunity;

    public override SerializableDictionary<string, float> DataParameters() =>
        new SerializableDictionary<string, float>()
        {
            {percentageKey, percentage},
            {durationKey, duration}
        };

    public EnhImmunity(EnhImmunityBlueprint blueprint)
    {
        percentage = blueprint.GeneratePercentImmunity();
        duration = blueprint.GenerateDuration();
    }

    public EnhImmunity(SerializableDictionary<string, float> parameters)
    {
        System.Diagnostics.Debug.Assert(parameters.ContainsKey(percentageKey) && parameters.ContainsKey(durationKey));

        percentage = parameters[percentageKey];
        duration = parameters[durationKey];
    }


    public IEnumerator RunImmunity(Entity entity)
    {
        entity.PercentStatusImmunity += percentage;
        yield return new WaitForSeconds(duration);

        entity.PercentStatusImmunity += percentage;
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunImmunity(receiver));
    }

    public override string ToString()
    {
        return $"{Type()}: {percentage}% for {duration}";
    }


}