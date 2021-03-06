using System.Collections;
using UnityEngine;

public class EnhShield : Enhancement
{
    Shield shield;
    float duration;

    public override EffectId Type() => EffectId.Shield;

    public override SerializableDictionary<string, float> DataParameters() => 
        new SerializableDictionary<string, float>()
        {
            {shieldAmountKey, shield.amount},
            { shieldDefenseKey, shield.defense},
            { shieldResistanceKey, shield.resistance},
            { shieldRechargeSpeedKey, shield.rechargeSpeed},
            { shieldRechargeDelayKey, shield.rechargeDelay},
            { durationKey, duration}
        };

public EnhShield(EnhShieldBlueprint blueprint)
    {
        shield = blueprint.GenerateShield();
        duration = blueprint.GenerateDuration();
    }

    public EnhShield(SerializableDictionary<string, float> parameters)
    {
        System.Diagnostics.Debug.Assert(parameters.ContainsKey(shieldAmountKey) && parameters.ContainsKey(shieldDefenseKey) && parameters.ContainsKey(shieldResistanceKey) && parameters.ContainsKey(shieldRechargeSpeedKey) && parameters.ContainsKey(shieldRechargeDelayKey) && parameters.ContainsKey(durationKey));

        shield.amount = parameters[shieldAmountKey];
        shield.defense = parameters[shieldDefenseKey];
        shield.resistance = parameters[shieldResistanceKey];
        shield.rechargeSpeed = parameters[shieldRechargeSpeedKey];
        shield.rechargeDelay = parameters[shieldRechargeDelayKey];
        duration = parameters[durationKey];
    }

    private IEnumerator RunShield(Entity entity)
    {
        entity.Health.AddShield(shield);
        Debug.Log($"Added shield: ({shield.amount}, {shield.defense}, {shield.resistance}), shield total is now:  ({entity.Health.CurrentShield.amount}, {entity.Health.CurrentShield.defense}, {entity.Health.CurrentShield.resistance})");

        yield return new WaitForSeconds(duration);

        entity.Health.SubtractShield(shield);
        Debug.Log($"Subtracted shield: ({shield.amount}, {shield.defense}, {shield.resistance}), shield total is now:  ({entity.Health.CurrentShield.amount}, {entity.Health.CurrentShield.defense}, {entity.Health.CurrentShield.resistance})");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunShield(receiver));
    }
    public override string ToString()
    {
        return $"{Type()}: {shield} for {duration}";
    }


}