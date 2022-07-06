using UnityEngine;

[CreateAssetMenu(fileName = "New FlatDamage Blueprint", menuName = "Substance Effect Blueprints/Damage/Flat")]
public class FlatDmgBlueprint : EffectBlueprint
{
    public Damage damageMin;
    public Damage damageMax;

    public override Effect Generate()
    {
        return new FlatDmg(this);
    }

    public Damage GenerateFlatDamage()
    { 
        return new Damage(
            Random.Range(damageMin.physical, damageMax.physical),
            Random.Range(damageMin.magical, damageMax.magical),
            Random.Range(damageMin.trueDmg, damageMax.trueDmg));
    }

    

}
