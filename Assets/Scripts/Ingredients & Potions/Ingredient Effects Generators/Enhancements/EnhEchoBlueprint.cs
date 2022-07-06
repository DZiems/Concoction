using UnityEngine;

[CreateAssetMenu(fileName = "New Echo Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Echo")]
public class EnhEchoBlueprint : EffectBlueprint
{

    public float bonusPercentOfDamageMin = 0.25f;
    public float bonusPercentOfDamageMax = 0.75f;

    public float durationMin = 5f;
    public float durationMax = 15f;

    public override Effect Generate()
    {
        throw new System.NotImplementedException();
    }

    public float GenerateBonusPercentOfDamage()
    {
        return Random.Range(bonusPercentOfDamageMin, bonusPercentOfDamageMax);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

}
