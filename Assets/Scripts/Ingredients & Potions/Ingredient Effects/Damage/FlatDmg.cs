

public class FlatDmg : Impairment
{

    public Damage damage;


    public override EffectId Type() => EffectId.FlatDmg;

    public override SerializableDictionary<string, float> DataParameters() =>
        new SerializableDictionary<string, float>()
        {
            {damagePhysicalKey, damage.physical},
            {damageMagicalKey, damage.magical},
            {damageTrueKey, damage.trueDmg},
        };

    public FlatDmg(FlatDmgBlueprint blueprint)
    {
        damage = blueprint.GenerateFlatDamage();
    }

    public FlatDmg(SerializableDictionary<string, float> parameters)
    {

        System.Diagnostics.Debug.Assert(parameters.ContainsKey(damagePhysicalKey) && parameters.ContainsKey(damageMagicalKey) && parameters.ContainsKey(damageTrueKey));

        
        damage = new Damage(parameters[damagePhysicalKey], parameters[damageMagicalKey], parameters[damageTrueKey]);
    }

    public override void RunEffect(Entity receiver, Entity dealer)
    {
        receiver.Health.TakeDamage(damage, dealer);
    }

}
