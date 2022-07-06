using UnityEngine;

[CreateAssetMenu(fileName = "New Foresight Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Foresight")]
public class EnhForesightBlueprint : EffectBlueprint
{

    public float durationMin = 4.0f;
    public float durationMax = 12.0f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}