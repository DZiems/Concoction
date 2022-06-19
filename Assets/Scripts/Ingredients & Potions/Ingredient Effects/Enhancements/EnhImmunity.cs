using System.Collections;
using UnityEngine;

public class EnhImmunity : Enhancement
{

    float percentage;
    float duration;


    public EnhImmunity(EnhImmunityBlueprint blueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_Immunity;

        percentage = blueprint.GeneratePercentImmunity();
        duration = blueprint.GenerateDuration();
    }

    public EnhImmunity(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_Immunity;

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
        return $"{Group}: {Type}: {percentage}% for {duration}";
    }

    public override EffectData GetData()
    {
        var parameters = new SerializableDictionary<string, float>()
        {
            {percentageKey, percentage},
            {durationKey, duration}
        };

        return new EffectData(Group, Type, parameters); 
    }

}