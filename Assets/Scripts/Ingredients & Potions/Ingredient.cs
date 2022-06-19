using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient
{
    public string identity;
    public int level { get; private set; }
    public RarityTier rarity { get; private set; }
    public Region region { get; private set; }
    public Taxonomy taxonomy { get; private set; }

    public Enhancement[] enhanceEffects;
    public Impairment[] impairEffects;

    public Ingredient(IngredientBlueprint blueprint)
    {
        identity = blueprint.identity;
        level = blueprint.level;
        rarity = blueprint.rarity;
        region = blueprint.region;
        taxonomy = blueprint.taxonomy;

        enhanceEffects = new Enhancement[blueprint.enhanceEffectBlueprints.Length];
        impairEffects = new Impairment[blueprint.impairEffectBlueprints.Length];

        int i = 0;
        foreach (var enhanceBP in blueprint.enhanceEffectBlueprints)
            enhanceEffects[i++] = enhanceBP.Generate();

        i = 0;
        foreach (var impairBP in blueprint.impairEffectBlueprints) 
            impairEffects[i++] = impairBP.Generate();

        //TODO:
        /*
         * foreach enhancement in enhanceEffects, apply level multiplier
         * foreach impairment in impairEffects, apply level multiplier
         * make apply level multiplier, make it just generic applyMultiplier(multiplier), because
         * later it can be used for synergy multipliers as well
         */
    }

    public Ingredient(IngredientData data)
    {
        identity = data.identity;
        level = data.level;
        rarity = data.rarity;
        region = data.region;
        taxonomy = data.taxonomy;

        //effect datas get parsed into collection of instantiated enhancements and impairments here.
        int enhCount = 0, impCount = 0;
        foreach (var effectData in data.effectDatas.Values)
        {
            if (effectData.group == EffectGroup.Enhancement)
                enhCount++;
            else if (effectData.group == EffectGroup.Impairment)
                impCount++;
        }

        enhanceEffects = new Enhancement[enhCount];
        impairEffects = new Impairment[impCount];
        int enhInd = 0, impInd = 0;
        foreach (var effectData in data.effectDatas.Values)
        {
            var effect = IngredientData.GetEffectFromData(effectData);
            if (effectData.group == EffectGroup.Enhancement)
                enhanceEffects[enhInd++] = (Enhancement)effect;
            else if (effectData.group == EffectGroup.Impairment)
                impairEffects[impInd++] = (Impairment)effect;
        }
    }


    public IngredientData GetData() 
    {
        var effectDatas = new SerializableDictionary<EffectID, EffectData>();
        foreach (var enhancement in enhanceEffects)
            effectDatas.Add(enhancement.Type, enhancement.GetData());

        foreach (var impairment in impairEffects)
            effectDatas.Add(impairment.Type, impairment.GetData());

        return new IngredientData(identity, level, rarity, region, taxonomy, effectDatas);
    }

    //TODO: figure out cooldown math
    public float GenerateCooldown()
    {
        throw new System.NotImplementedException();
    }

    //TODO: figure out level math
    public float GenerateLevelMultiplier()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        string theString = $"Id: {identity}, lv: {level}, rarity: {rarity}, region: {region}, taxonomy: {taxonomy}";
        theString += "\n\tenhancements: ";
        foreach (var enhancement in enhanceEffects)
        {
            theString += $"\n\t\t{enhancement}";
        }
        theString += "\n\timpairments: ";
        foreach (var impairment in impairEffects)
        {
            theString += $"\n\t\t{impairment}";
        }

        return theString;
    }
}
