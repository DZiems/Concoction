using UnityEngine;

[System.Serializable]
public class IngredientData
{
    public string identity;
    public int level;
    public RarityTier rarity;
    public Region region;
    public Taxonomy taxonomy;

    public SerializableDictionary<EffectID, EffectData> effectDatas;

    public IngredientData(string identity, int level, RarityTier rarity, Region region, Taxonomy taxonomy, SerializableDictionary<EffectID, EffectData> effectDatas)
    {
        this.identity = identity;
        this.level = level;
        this.rarity = rarity;
        this.region = region;
        this.taxonomy = taxonomy;
        this.effectDatas = effectDatas;
    }

    public static Effect GetEffectFromData(EffectData effectData)
    {
        switch (effectData.type)
        {
            case EffectID.Enh_HealthRegen:
                return new EnhHealthRegen(effectData.parameters);
            case EffectID.Enh_MoveSpeed:
                return new EnhMoveSpeed(effectData.parameters);
            case EffectID.Enh_CooldownModifier:
                return new EnhCooldownModifier(effectData.parameters);
            case EffectID.Enh_MagicalDamage:
                return new EnhMagicalDamage(effectData.parameters);
            case EffectID.Enh_Magnify:
                throw new System.NotImplementedException();
            case EffectID.Enh_Thorns:
                return new EnhThorns(effectData.parameters);
            case EffectID.Enh_Overheal:
                throw new System.NotImplementedException();
            case EffectID.Enh_Shield:
                return new EnhShield(effectData.parameters);
            case EffectID.Enh_Foresight:
                throw new System.NotImplementedException();
            case EffectID.Enh_LingeringTrail:
                throw new System.NotImplementedException();
            case EffectID.Enh_MiniSeekers:
                throw new System.NotImplementedException();
            case EffectID.Enh_InvisibilityBubble:
                throw new System.NotImplementedException();
            case EffectID.Enh_Dematerialize:
                throw new System.NotImplementedException();
            case EffectID.Enh_BecomeShade:
                throw new System.NotImplementedException();
            case EffectID.Enh_Immunity:
                return new EnhImmunity(effectData.parameters);
            case EffectID.Enh_GrowVineWhips:
                throw new System.NotImplementedException();
            case EffectID.Enh_GrowStinger:
                throw new System.NotImplementedException();
            case EffectID.Enh_Contagion:
                throw new System.NotImplementedException();
            case EffectID.Enh_UmbralClone:
                throw new System.NotImplementedException();
            case EffectID.Enh_Echo:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Slow:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Daze:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Shove:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Blind:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Fear:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Gravitate:
                throw new System.NotImplementedException();
            case EffectID.Hnd_HealBlock:
                throw new System.NotImplementedException();
            case EffectID.Hnd_ReduceResistance:
                throw new System.NotImplementedException();
            case EffectID.Hnd_DisintegrateResistance:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Expose:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Beguile:
                throw new System.NotImplementedException();
            case EffectID.Hnd_Stinkify:
                throw new System.NotImplementedException();
            case EffectID.Dmg_Flat:
                return new DmgFlat(effectData.parameters);
            case EffectID.Dmg_Dot:
                return new DmgDot(effectData.parameters);
            case EffectID.Dmg_FlatDelayed:
                throw new System.NotImplementedException();
            case EffectID.Sph_HealthAsHealth:
                throw new System.NotImplementedException();
            case EffectID.Sph_HealthAsShield:
                throw new System.NotImplementedException();
            case EffectID.Sph_MagicDamage:
                throw new System.NotImplementedException();
            case EffectID.Sph_Defense:
                throw new System.NotImplementedException();
            case EffectID.Sph_Resistance:
                throw new System.NotImplementedException();
            case EffectID.Sph_MoveSpeed:
                throw new System.NotImplementedException();
            default:
                Debug.LogError("IngredientData GetEffectFromData(): Somehow reached default case in an enum switch statement, which means a newly added EffectID case has not been implemented");
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
