using UnityEngine;

[CreateAssetMenu(fileName = "New BecomeApparition Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Become Shade")]
public class EnhBecomeShadeBlueprint : EnhanceEffectBlueprint
{ 
    public float contactDmgPercentReductionMin = 0.2f;
    public float contactDmgPercentReductionMax = 0.4f;

    public float durationMin = 5f;
    public float durationMax = 10f;

    public float GenerateContactDmgPercentReduction()
    {
        return Random.Range(contactDmgPercentReductionMin, contactDmgPercentReductionMax);
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