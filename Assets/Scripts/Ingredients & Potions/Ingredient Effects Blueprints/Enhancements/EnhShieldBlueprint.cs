using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Blueprint", menuName = "Substance Effect Blueprints/Enhancements/Shield")]
public class EnhShieldBlueprint : EnhanceEffectBlueprint
{
    public Shield shieldMin;
    public Shield shieldMax;

    public float durationMin = 10f;
    public float durationMax = 20f;

    public Shield GenerateShield()
    {
        float resist = Random.Range(shieldMin.resistance, shieldMax.resistance);
        float defense = Random.Range(shieldMin.defense, shieldMax.defense);
        float amount = Random.Range(shieldMin.amount, shieldMax.amount);

        float rechargeSpeed = Random.Range(shieldMin.rechargeSpeed, shieldMax.rechargeSpeed);
        float rechargeDelay = Random.Range(shieldMin.rechargeDelay, shieldMax.rechargeDelay);

        return new Shield(amount, defense, resist, rechargeSpeed, rechargeDelay);
    }

    public float GenerateDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public override Enhancement Generate()
    {
        return new EnhShield(this);
    }
}
