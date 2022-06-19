

using UnityEngine;

public class DmgDot : Impairment
{

    public DotDamage dotDamage;

    public DmgDot(DmgDotBlueprint blueprint)
    {
        Group = EffectGroup.Impairment;
        Type = EffectID.Dmg_Dot;

        dotDamage = blueprint.GenerateDotDamage();
    }

    public DmgDot(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Impairment;
        Type = EffectID.Dmg_Dot;

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(damagePhysicalKey) && parameters.ContainsKey(damageMagicalKey) && parameters.ContainsKey(damageTrueKey) && parameters.ContainsKey(durationKey) && parameters.ContainsKey(tickTimeKey));

        dotDamage = new DotDamage(parameters[damagePhysicalKey], parameters[damageMagicalKey], parameters[damageTrueKey], parameters[durationKey], parameters[tickTimeKey]);
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.Health.TakeDamage(dotDamage, dealer);
    }

    public override string ToString()
    {
        return $"{Group}: {Type}: {dotDamage}";
    }

    public override EffectData GetData()
    {

        var parameters = new SerializableDictionary<string, float>()
        {
            {damagePhysicalKey, dotDamage.physical},
            {damageMagicalKey, dotDamage.magical},
            {damageTrueKey, dotDamage.trueDmg},
            {tickTimeKey, dotDamage.tickTime},
            {durationKey, dotDamage.duration},
        };

        return new EffectData(Group, Type, parameters);
    }

    
}
