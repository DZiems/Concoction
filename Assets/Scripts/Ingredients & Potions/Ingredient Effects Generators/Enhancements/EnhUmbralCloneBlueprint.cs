using UnityEngine;

[CreateAssetMenu(fileName = "New UmbralClone Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Umbral Clone")]
public class EnhUmbralCloneBlueprint : EffectBlueprint
{
    public float percentOfDamageMin = 0.5f;
    public float percentOfDamageMax = 0.7f;

    public float durationMin = 5.0f;
    public float durationMax = 10.0f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GeneratePercentOfDamage()
    {
        return Random.Range(percentOfDamageMin, percentOfDamageMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}
