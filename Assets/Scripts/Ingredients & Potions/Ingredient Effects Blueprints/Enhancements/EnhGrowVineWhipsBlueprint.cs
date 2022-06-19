using UnityEngine;

[CreateAssetMenu(fileName = "New GrowVineWhips Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Grow Vine Whips")]
public class EnhGrowVineWhipsBlueprint : EnhanceEffectBlueprint
{

    public Damage whipDamageMin;
    public Damage whipDamageMax;

    public float rangeMin = 2f;
    public float rangeMax = 3f;

    public bool isShoving;
    public bool isDazing;

    public float durationMin = 8f;
    public float durationMax = 14f;

    public Damage GenerateWhipDamage()
    {
        float physical = Random.Range(whipDamageMin.physical, whipDamageMax.physical);
        float magical = Random.Range(whipDamageMin.magical, whipDamageMax.magical);
        float trueDmg = Random.Range(whipDamageMin.trueDmg, whipDamageMax.trueDmg);

        return new Damage(physical, magical, trueDmg);
    }

    public float GenerateRange()
    {
        return Random.Range(rangeMin, rangeMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        throw new System.NotImplementedException();
    }
}