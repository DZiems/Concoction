using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhThorns : Enhancement
{
    float physicalThornsFactor;
    float magicalThornsFactor;
    float trueDmgThornsFactor;

    float duration;

    public EnhThorns(EnhThornsBlueprint blueprint)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_Thorns;

        physicalThornsFactor = blueprint.GeneratePhysicalThornsFactor();
        magicalThornsFactor = blueprint.GenerateMagicalThornsFactor();
        trueDmgThornsFactor = blueprint.GenerateTrueDmgThornsFactor();
        duration = blueprint.GenerateDuration();
    }

    public EnhThorns(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Enhancement;
        Type = EffectID.Enh_Thorns;

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(physicalThornsFactorKey) && parameters.ContainsKey(magicalThornsFactorKey) && parameters.ContainsKey(trueDmgThornsFactorKey) && parameters.ContainsKey(durationKey));

        physicalThornsFactor = parameters[physicalThornsFactorKey];
        magicalThornsFactor = parameters[magicalThornsFactorKey];
        trueDmgThornsFactor = parameters[trueDmgThornsFactorKey];

        duration = parameters[durationKey];
    }

    public static Damage GetThornsDamage(Damage damageReceived, Entity damageRecipient)
    {
        float physicalDmg =
            (damageReceived.physical + damageReceived.magical + damageReceived.trueDmg) * damageRecipient.PhysicalThornsMultiplier;
        float magicalDmg =
            (damageReceived.physical + damageReceived.magical + damageReceived.trueDmg) * damageRecipient.MagicalThornsMultiplier;
        float trueDmg =
            (damageReceived.physical + damageReceived.magical + damageReceived.trueDmg) * damageRecipient.TrueDmgThornsMultiplier;

        return new Damage(physicalDmg, magicalDmg, trueDmg);
    }

    private IEnumerator RunThorns(Entity entity)
    {
        entity.PhysicalThornsMultiplier += physicalThornsFactor;
        entity.MagicalThornsMultiplier += magicalThornsFactor;
        entity.TrueDmgThornsMultiplier += trueDmgThornsFactor;

        Debug.Log($"Added thorns: ({physicalThornsFactor}, {magicalThornsFactor}, {trueDmgThornsFactor}), thorns factors are now:  ({entity.PhysicalThornsMultiplier}, {entity.MagicalThornsMultiplier}, {entity.TrueDmgThornsMultiplier})");

        yield return new WaitForSeconds(duration);

        entity.PhysicalThornsMultiplier -= physicalThornsFactor;
        entity.MagicalThornsMultiplier -= magicalThornsFactor;
        entity.TrueDmgThornsMultiplier -= trueDmgThornsFactor;

        Debug.Log($"Ended thorns: ({physicalThornsFactor}, {magicalThornsFactor}, {trueDmgThornsFactor}), thorns factors are now:  ({entity.PhysicalThornsMultiplier}, {entity.MagicalThornsMultiplier}, {entity.TrueDmgThornsMultiplier})");
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.StartCoroutine(RunThorns(receiver));
    }
    public override string ToString()
    {
        return $"{Group}: {Type}: {physicalThornsFactor}x phys, {magicalThornsFactor}x mag, {trueDmgThornsFactor}x true, for {duration}s";
    }

    public override EffectData GetData()
    {
        var parameters = new SerializableDictionary<string, float>()
        {
            {physicalThornsFactorKey, physicalThornsFactor},
            {magicalThornsFactorKey, magicalThornsFactor},
            {trueDmgThornsFactorKey, trueDmgThornsFactor},
            {durationKey, duration}
        };

        return new EffectData(Group, Type, parameters);
    }

}
