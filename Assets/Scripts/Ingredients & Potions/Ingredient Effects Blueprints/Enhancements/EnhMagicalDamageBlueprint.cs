using UnityEngine;

[CreateAssetMenu(fileName = "New MagicalDamage Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Magical Damage")]
public class EnhMagicalDamageBlueprint : EnhanceEffectBlueprint
{
    public float multiplierMin = 0.8f;
    public float multiplierMax = 1.2f;

    public float durationMin = 8f;
    public float durationMax = 12f;

    public float GenerateMultiplier()
    {
        return Random.Range(multiplierMin, multiplierMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        return new EnhMagicalDamage(this);
    }
}
