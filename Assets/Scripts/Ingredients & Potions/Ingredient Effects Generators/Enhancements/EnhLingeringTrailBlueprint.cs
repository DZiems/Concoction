using UnityEngine;

[CreateAssetMenu(fileName = "New LingeringTrail Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Lingering Trail")]
public class EnhLingeringTrailBlueprint : EffectBlueprint
{
    public float percentOfDamageMin = 0.1f;
    public float percentOfDamageMax = 0.25f;

    public float lingerDurationMin = 3f;
    public float lingerDurationMax = 6f;

    public float durationMin = 5f;
    public float durationMax = 15f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GeneratePercentOfDamage()
    {
        return Random.Range(percentOfDamageMin, percentOfDamageMax);
    }

    public float GenerateLingerDuration()
    {
        return Random.Range(lingerDurationMin, lingerDurationMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}