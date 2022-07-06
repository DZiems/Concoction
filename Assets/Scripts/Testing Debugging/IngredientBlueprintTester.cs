using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBlueprintTester : MonoBehaviour
{
    public IngredientBlueprint tester;

    [ContextMenu("Generate Rarity Tiers")]
    public void GenerateRarityTiers()
    {
        int rarityProb = 0;

        if (tester.rarityRoller.Common.isEnabled)
            rarityProb += (int)tester.rarityRoller.Common.probability;

        if (tester.rarityRoller.Uncommon.isEnabled)
            rarityProb += (int)tester.rarityRoller.Uncommon.probability;

        if (tester.rarityRoller.Rare.isEnabled)
            rarityProb += (int)tester.rarityRoller.Rare.probability;

        if (tester.rarityRoller.Epic.isEnabled)
            rarityProb += (int)tester.rarityRoller.Epic.probability;

        if (tester.rarityRoller.Fabled.isEnabled)
            rarityProb += (int)tester.rarityRoller.Fabled.probability;

        rarityProb *= 100;


        for (int i = 0; i < rarityProb; i++)
        {
            Debug.Log(tester.rarityRoller.Roll());
        }
    }
    public void GenerateIngredientEffects(RarityTier rarityTier)
    {
        var result = tester.effectRoller.Roll(rarityTier);

        foreach (var enhancement in result.Item1)
        {
            Debug.Log(enhancement.Type());
        }
        foreach (var impairment in result.Item2)
        {
            Debug.Log(impairment.Type());
        }

    }
    [ContextMenu("Generate Common Ingredient Effects 1000x")]
    public void GenerateCommon()
    {
        for (int i = 0; i < 1000; i++)
            GenerateIngredientEffects(RarityTier.Common);
    }

    [ContextMenu("Generate Uncommon Ingredient Effects 1000x")]
    public void GenerateUncommon()
    {
        for (int i = 0; i < 1000; i++)
            GenerateIngredientEffects(RarityTier.Uncommon);
    }

    [ContextMenu("Generate Rare Ingredient Effects 1000x")]
    public void GenerateRare()
    {
        for (int i = 0; i < 1000; i++)
            GenerateIngredientEffects(RarityTier.Rare);
    }

    [ContextMenu("Generate Epic Ingredient Effects 1000x")]
    public void GenerateEpic()
    {
        for (int i = 0; i < 1000; i++)
            GenerateIngredientEffects(RarityTier.Epic);
    }

    [ContextMenu("Generate Fabled Ingredient Effects 1000x")]
    public void GenerateFabled()
    {
        for (int i = 0; i < 1000; i++)
            GenerateIngredientEffects(RarityTier.Fabled);
    }

}
