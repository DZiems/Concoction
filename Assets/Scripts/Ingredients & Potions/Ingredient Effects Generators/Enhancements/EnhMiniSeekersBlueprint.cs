using UnityEngine;

[CreateAssetMenu(fileName = "New MiniSeekers Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Mini Seekers")]
public class EnhMiniSeekersBlueprint : EffectBlueprint
{
    public int numSeekersMin = 5;
    public int numSeekersMax = 10;

    public float durationMin = 5f;
    public float durationMax = 10f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public int GenerateNumSeekers()
    {
        return Random.Range(numSeekersMin, numSeekersMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}
