using UnityEngine;

[CreateAssetMenu(fileName = "New Congtagion Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Contagion")]
public class EnhContagionBlueprint : EnhanceEffectBlueprint
{

    public int numPropagationsMin = 5;
    public int numPropagationsMax = 12;

    public float bonusPercentOfDamageMin = 0.15f;
    public float bonusPercentOfDamageMax = 0.5f;

    public float durationMin = 5.0f;
    public float durationMax = 10f;

    public int GenerateNumPropagations()
    {
        return Random.Range(numPropagationsMin, numPropagationsMax);
    }
    public float GenerateBonusPercentOfDamage()
    {
        return Random.Range(bonusPercentOfDamageMin, bonusPercentOfDamageMax);
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