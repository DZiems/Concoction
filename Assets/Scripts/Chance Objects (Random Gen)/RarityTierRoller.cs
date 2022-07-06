using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RarityTierRoller
{
    public ChanceTable<RarityTier> tierTable = new(1,
        new ChanceObject<RarityTier>[5]
        {
            new (RarityTier.Common, 256f),
            new(RarityTier.Uncommon, 64f),
            new(RarityTier.Rare, 16f),
            new(RarityTier.Epic, 4f),
            new(RarityTier.Fabled, 1f)
        });

    public ChanceObject<RarityTier> Common => tierTable.content[0];
    public ChanceObject<RarityTier> Uncommon => tierTable.content[1];
    public ChanceObject<RarityTier> Rare => tierTable.content[2];
    public ChanceObject<RarityTier> Epic => tierTable.content[3];
    public ChanceObject<RarityTier> Fabled => tierTable.content[4];

    public RarityTier Roll()
    {
        return Chance<RarityTier>.Roll(tierTable.content);
    }

}
