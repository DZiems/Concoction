using UnityEngine;

[CreateAssetMenu(fileName = "New HealthRegen Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Health Regen")]
public class EnhHealthRegenBlueprint : EnhanceEffectBlueprint
{
    public float healAmountMin = 0.5f;
    public float healAmountMax = 0.7f;

    public float healTickTimeMin = 0.5f;
    public float healTickTimeMax = 0.7f;

    public float durationMin = 5f;
    public float durationMax = 10f;

    public float GenerateHealAmount()
    {
        return Random.Range(healAmountMin, healAmountMax);
    }

    public float GenerateHealPeriod() {
        return Random.Range(healTickTimeMin, healTickTimeMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        return new EnhHealthRegen(this);
    }
}
