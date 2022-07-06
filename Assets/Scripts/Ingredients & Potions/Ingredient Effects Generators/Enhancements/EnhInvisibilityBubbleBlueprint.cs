using UnityEngine;

[CreateAssetMenu(fileName = "New InvisibilityBubble Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Invisibility Bubble")]
public class EnhInvisibilityBubbleBlueprint : EffectBlueprint
{
    public float durationMin = 2.0f;
    public float durationMax = 6.0f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}