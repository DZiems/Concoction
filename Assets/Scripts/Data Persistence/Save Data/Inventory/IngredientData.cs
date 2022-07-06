using UnityEngine;

[System.Serializable]
public class IngredientData
{
    public string nameId;
    public int level;
    public RarityTier rarity;

    public SerializableDictionary<EffectId, EffectData> effectDatas;

    public IngredientData(string nameId, int level, RarityTier rarity, SerializableDictionary<EffectId, EffectData> effectDatas)
    {
        this.nameId = nameId;
        this.level = level;
        this.rarity = rarity;
        this.effectDatas = effectDatas;
    }

    public static Effect GetEffectFromData(EffectData effectData)
    {
        switch (effectData.id)
        {
            case EffectId.HealthRegen:
                return new EnhHealthRegen(effectData.parameters);
            case EffectId.MoveSpeed:
                return new EnhMoveSpeed(effectData.parameters);
            case EffectId.CooldownModifier:
                return new EnhCooldownModifier(effectData.parameters);
            case EffectId.MagicalDamage:
                return new EnhMagicalDamage(effectData.parameters);
            case EffectId.Thorns:
                return new EnhThorns(effectData.parameters);
            case EffectId.Shield:
                return new EnhShield(effectData.parameters);
            case EffectId.Immunity:
                return new EnhImmunity(effectData.parameters);
            case EffectId.FlatDmg:
                return new FlatDmg(effectData.parameters);
            case EffectId.DotDmg:
                return new DotDmg(effectData.parameters);
            default:
                Debug.LogError($"IngredientData GetEffectFromData(): case for {effectData.id} has not been implemented into the switch statement. ");
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
