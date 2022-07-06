using UnityEngine;

[CreateAssetMenu(fileName = "New Dematerialize Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Dematerialize")]
public class EnhDematerializeBlueprint : EffectBlueprint
{

    public float durationMin = 60f;
    public float durationMax = 120f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}