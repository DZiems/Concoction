using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhMoveSpeed : Enhancement
{

    float multiplier;
    float duration;

    private float amountChanged;

    public EnhMoveSpeed(EnhMoveSpeedBlueprint moveSpeedBlueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_MagicalDamage;

        multiplier = Random.Range(moveSpeedBlueprint.multiplierMin, moveSpeedBlueprint.multiplierMax);

        duration = Random.Range(moveSpeedBlueprint.durationMin, moveSpeedBlueprint.durationMax);

        amountChanged = 0f;
    }

    public EnhMoveSpeed(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_MagicalDamage;

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(multiplierKey) && parameters.ContainsKey(durationKey));

        multiplier = parameters[multiplierKey];
        duration = parameters[durationKey];


        amountChanged = 0f;
    }


    private IEnumerator RunMoveSpeed(Entity entity)
    {
        amountChanged = entity.CurrentMoveSpeed * multiplier - entity.CurrentMoveSpeed;

        entity.CurrentMoveSpeed += amountChanged;
        Debug.Log($"Added movement speed: ({amountChanged}), movement total is now:  ({entity.CurrentMoveSpeed})");

        yield return new WaitForSeconds(duration);

        entity.CurrentMoveSpeed -= amountChanged; 
        Debug.Log($"Movement speed buff ended: movement total is now:  ({entity.CurrentMoveSpeed})");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunMoveSpeed(receiver));
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
