using UnityEngine;

[CreateAssetMenu(fileName = "New GrowStinger Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Grow Stinger")]
public class EnhGrowStingerBlueprint : EnhanceEffectBlueprint
{
    public DotDamage stingDamageMin;
    public DotDamage stingDamageMax;

    public float rangeMin = 0.8f;
    public float rangeMax = 2.0f;

    public bool isSlowing;
    public bool isArmorReducing;
    public bool isArmorDisintegrating;

    public float durationMin = 8f;
    public float durationMax = 14f;

    public DotDamage GenerateStingDotDamagee()
    {
        float dur = Random.Range(stingDamageMin.duration, stingDamageMax.duration);
        float tickTime = Random.Range(stingDamageMin.tickTime, stingDamageMax.tickTime);
        float physical = Random.Range(stingDamageMin.physical, stingDamageMax.physical);
        float magical = Random.Range(stingDamageMin.magical, stingDamageMax.magical);
        float trueDmg = Random.Range(stingDamageMin.trueDmg, stingDamageMax.trueDmg);

        return new DotDamage(physical, magical, trueDmg, tickTime, dur);
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
