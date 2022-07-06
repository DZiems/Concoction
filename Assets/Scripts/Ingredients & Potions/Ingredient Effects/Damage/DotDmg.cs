

using UnityEngine;

public class DotDmg : Impairment
{

    public DotDamage dotDamage;

    public override EffectId Type() => EffectId.DotDmg;

    public override SerializableDictionary<string, float> DataParameters() =>
        new SerializableDictionary<string, float>()
        {
            {damagePhysicalKey, dotDamage.physical},
            {damageMagicalKey, dotDamage.magical},
            {damageTrueKey, dotDamage.trueDmg},
            {tickTimeKey, dotDamage.tickTime},
            {durationKey, dotDamage.duration},
        };

    public DotDmg(DotDmgBlueprint blueprint)
    {
        dotDamage = blueprint.GenerateDotDamage();
    }

    public DotDmg(SerializableDictionary<string, float> parameters)
    {
        System.Diagnostics.Debug.Assert(parameters.ContainsKey(damagePhysicalKey) && parameters.ContainsKey(damageMagicalKey) && parameters.ContainsKey(damageTrueKey) && parameters.ContainsKey(durationKey) && parameters.ContainsKey(tickTimeKey));

        dotDamage = new DotDamage(parameters[damagePhysicalKey], parameters[damageMagicalKey], parameters[damageTrueKey], parameters[durationKey], parameters[tickTimeKey]);
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.Health.TakeDamage(dotDamage, dealer);
    }

    public override string ToString()
    {
        return $"{Type()}: {dotDamage}";
    }


}
