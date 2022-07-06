using System;
using System.Collections.Generic;

[System.Serializable]
public class EffectRoller
{
    public ChanceTable<EffectBlueprint> baseTable = new(1, new ChanceObject<EffectBlueprint>[0]);

    public ChanceTable<EffectBlueprint> commonTable = new(0, new ChanceObject<EffectBlueprint>[0]);
    public ChanceTable<EffectBlueprint> uncommonTable = new(0, new ChanceObject<EffectBlueprint>[0]);
    public ChanceTable<EffectBlueprint> rareTable = new(0, new ChanceObject<EffectBlueprint>[0]);
    public ChanceTable<EffectBlueprint> epicTable = new(0, new ChanceObject<EffectBlueprint>[0]);
    public ChanceTable<EffectBlueprint> fabledTable = new(0, new ChanceObject<EffectBlueprint>[0]);

    public Tuple<List<Enhancement>, List<Impairment>> Roll(RarityTier rarityTier)
    {
        //initialize blueprints list with the base effect
        List<EffectBlueprint> rolledBlueprints = new List<EffectBlueprint>()
        {
            //this override of Chance<> method returns one item.
            Chance<EffectBlueprint>.Roll(baseTable.content)
        };

        //each higher rarity tier's effects are additive onto those lower.
        rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(commonTable));

        if (rarityTier == RarityTier.Uncommon)
        {
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(uncommonTable));
        }

        else if (rarityTier == RarityTier.Rare)
        {
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(uncommonTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(rareTable));
        }

        else if (rarityTier == RarityTier.Epic)
        {
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(uncommonTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(rareTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(epicTable));
        }

        else if (rarityTier == RarityTier.Fabled)
        {
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(uncommonTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(rareTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(epicTable));
            rolledBlueprints.AddRange(Chance<EffectBlueprint>.Roll(fabledTable));
        }


        List<Enhancement> enhancements = new();
        List<Impairment> impairments = new();
        foreach (var blueprint in rolledBlueprints)
        {
            var effect = blueprint.Generate();
            if (effect is Enhancement enhancement)
                enhancements.Add(enhancement);

            if (effect is Impairment impairment)
                impairments.Add(impairment);
        }


        return new Tuple<List<Enhancement>, List<Impairment>>(enhancements, impairments);
    }
}


