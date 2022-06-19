using UnityEngine;

[CreateAssetMenu(fileName = "New MiniSeekers Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Mini Seekers")]
public class EnhMiniSeekersBlueprint : EnhanceEffectBlueprint
{
    public int numSeekersMin = 5;
    public int numSeekersMax = 10;

    public float durationMin = 5f;
    public float durationMax = 10f;

    public int GenerateNumSeekers()
    {
        return Random.Range(numSeekersMin, numSeekersMax);
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
