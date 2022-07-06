using UnityEngine;

[CreateAssetMenu(fileName = "New MagicalDamage Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Magical Damage")]
public class EnhMagicalDamageBlueprint : EffectBlueprint
{
    public float multiplierMin = 0.8f;
    public float multiplierMax = 1.2f;

    public float durationMin = 8f;
    public float durationMax = 12f;

    public override Effect Generate()
    {
        return new EnhMagicalDamage(this);
    }


    public float GenerateMultiplier()
    {
        return Random.Range(multiplierMin, multiplierMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}
