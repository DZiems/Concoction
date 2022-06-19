using UnityEngine;

[CreateAssetMenu(fileName = "New Dematerialize Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Dematerialize")]
public class EnhDematerializeBlueprint : EnhanceEffectBlueprint
{

    public float durationMin = 60f;
    public float durationMax = 120f;
    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        throw new System.NotImplementedException();
    }
}