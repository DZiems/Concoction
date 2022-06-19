using UnityEngine;

[CreateAssetMenu(fileName = "New Overheal Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Overheal")]
public class EnhOverhealBlueprint : EnhanceEffectBlueprint
{
    public float overhealCapMin = 10f;
    public float overhealCapMax = 30f;

    public float durationMin = 20.0f;
    public float durationMax = 45.0f;

    public float GenerateOverhealCap()
    {
        return Random.Range(overhealCapMin, overhealCapMax);
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
