using UnityEngine;

[CreateAssetMenu(fileName = "New Immunity Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Immunity")]
public class EnhImmunityBlueprint : EnhanceEffectBlueprint
{
    public float percentImmunityMin = 0.2f;
    public float percentImmunityMax = 0.8f;

    public float durationMin = 5.0f;
    public float durationMax = 10.0f;

    public float GeneratePercentImmunity()
    {
        return Random.Range(percentImmunityMin, percentImmunityMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        return new EnhImmunity(this);
    }
}

