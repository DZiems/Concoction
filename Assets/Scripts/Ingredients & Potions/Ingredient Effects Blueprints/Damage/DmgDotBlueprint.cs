using UnityEngine;

[CreateAssetMenu(fileName = "New DotDamage Blueprint", menuName = "Substance Effect Blueprints/Damage/DoT")]
public class DmgDotBlueprint : ImpairEffectBlueprint
{
    public Damage flatDamageConvertedToDotMin;
    public Damage flatDamageConvertedToDotMax;

    [Header("Remaining DoT Attributes")]
    public float durationMin;
    public float durationMax;

    public float tickTimeMin;
    public float tickTimeMax;

    public DotDamage GenerateDotDamage()
    {
        float duration = Random.Range(durationMin, durationMax);
        float tickTime = Random.Range(tickTimeMin, tickTimeMax);

        DotDamage min = DotDamage.FlatToDotConversion(flatDamageConvertedToDotMin, duration, tickTime);
        DotDamage max = DotDamage.FlatToDotConversion(flatDamageConvertedToDotMax, duration, tickTime);

        return new DotDamage(
            Random.Range(min.physical, max.physical),
            Random.Range(min.magical, max.magical),
            Random.Range(min.trueDmg, max.trueDmg),
            duration,
            tickTime
            );
    }

    public override Impairment Generate()
    {
        return new DmgDot(this);
    }
}
