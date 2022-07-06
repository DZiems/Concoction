using UnityEngine;

[CreateAssetMenu(fileName = "New MoveSpeed Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Move Speed")]
public class EnhMoveSpeedBlueprint : EffectBlueprint
{
    public float multiplierMin = 0.9f;
    public float multiplierMax = 1.3f;

    public float durationMin = 5f;
    public float durationMax = 12f;

    public override Effect Generate()
    {
        return new EnhMoveSpeed(this);
    }

    public float GenerateMultiplier()
    {
        return Random.Range(multiplierMin, multiplierMax);  
    }
    
    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}
