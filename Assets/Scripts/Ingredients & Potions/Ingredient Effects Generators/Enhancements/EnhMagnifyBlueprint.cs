using UnityEngine;

[CreateAssetMenu(fileName = "New Magnify Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Magnify")]
public class EnhMagnifyBlueprint : EffectBlueprint
{
    public float scaleMin = 1.2f;
    public float scaleMax = 2.0f;

    public float durationMin = 10f;
    public float durationMax = 15f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GenerateScale()
    {
        return Random.Range(scaleMin, scaleMax);
    }
    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}