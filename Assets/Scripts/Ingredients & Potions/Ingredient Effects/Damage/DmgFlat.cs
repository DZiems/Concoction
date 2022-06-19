

public class DmgFlat : Impairment
{

    public Damage damage;
    public DmgFlat(DmgFlatBlueprint blueprint)
    {
        Group = EffectGroup.Impairment;
        Type = EffectID.Dmg_Flat;

        damage = blueprint.GenerateFlatDamage();
    }

    public DmgFlat(SerializableDictionary<string, float> parameters)
    {
        Group = EffectGroup.Impairment;
        Type = EffectID.Dmg_Flat;

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(damagePhysicalKey) && parameters.ContainsKey(damageMagicalKey) && parameters.ContainsKey(damageTrueKey));

        
        damage = new Damage(parameters[damagePhysicalKey], parameters[damageMagicalKey], parameters[damageTrueKey]);
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.Health.TakeDamage(damage, dealer);
    }
    public override string ToString()
    {
        return $"{Group}: {Type}: {damage}";
    }

    public override EffectData GetData()
    {
        var parameters = new SerializableDictionary<string, float>()
        {
            {damagePhysicalKey, damage.physical},
            {damageMagicalKey, damage.magical},
            {damageTrueKey, damage.trueDmg},
        };

        return new EffectData(Group, Type, parameters);
    }
}
