using UnityEngine;

[CreateAssetMenu(fileName = "New FlatDamage Blueprint", menuName = "Substance Effect Blueprints/Damage/Flat")]
public class DmgFlatBlueprint : ImpairEffectBlueprint
{
    public Damage damageMin;
    public Damage damageMax; 

    public Damage GenerateFlatDamage()
    { 
        return new Damage(
            Random.Range(damageMin.physical, damageMax.physical),
            Random.Range(damageMin.magical, damageMax.magical),
            Random.Range(damageMin.trueDmg, damageMax.trueDmg));
    }

    public override Impairment Generate()
    {
        return new DmgFlat(this);
    }
}
