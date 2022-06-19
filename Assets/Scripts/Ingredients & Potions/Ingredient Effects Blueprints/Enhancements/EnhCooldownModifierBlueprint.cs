using UnityEngine;

[CreateAssetMenu(fileName = "New CooldownModifier Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Cooldown Modifier")]
public class EnhCooldownModifierBlueprint : EnhanceEffectBlueprint
{
    public float multiplierMin = 0.8f;
    public float multiplierMax = 1.2f;

    public float durationMin = 5f;
    public float durationMax = 10f;

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
        return new EnhCooldownModifier(this);
    }
}