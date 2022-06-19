using UnityEngine;

[CreateAssetMenu(fileName = "New Thorns Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Thorns")]
public class EnhThornsBlueprint : EnhanceEffectBlueprint
{
    public float physicalThornsFactorMin = 1.0f;
    public float physicalThornsFactorMax = 2.0f;

    public float magicalThornsFactorMin = 1.0f;
    public float magicalThornsFactorMax = 2.0f;

    public float TrueDmgThornsFactorMin = 1.0f;
    public float TrueDmgThornsFactorMax = 2.0f;

    public float durationMin = 10f;
    public float durationMax = 15f;

    public float GeneratePhysicalThornsFactor()
    {
        return Random.Range(physicalThornsFactorMin, physicalThornsFactorMax);
    }
    public float GenerateMagicalThornsFactor()
    {
        return Random.Range(magicalThornsFactorMin, magicalThornsFactorMax);
    }
    public float GenerateTrueDmgThornsFactor()
    {
        return Random.Range(TrueDmgThornsFactorMin, TrueDmgThornsFactorMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        return new EnhThorns(this);
    }
}
