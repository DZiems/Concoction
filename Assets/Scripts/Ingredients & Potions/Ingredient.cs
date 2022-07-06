using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient
{
    public IngredientId Id { get; private set; }

    public int Level { get; private set; }
    public RarityTier Rarity { get; private set; }


    public List<Enhancement> EnhanceEffects { get; private set; }
    public List<Impairment> ImpairEffects { get; private set; }

  
    public Ingredient(IngredientBlueprint blueprint, int level)
    {
        Id = blueprint.id;
        Level = level;

        Rarity = blueprint.rarityRoller.Roll();
        var effects = blueprint.effectRoller.Roll(Rarity);

        EnhanceEffects = effects.Item1;
        ImpairEffects = effects.Item2;
    }

    public Ingredient(IngredientData data)
    {
        Id = DataDictionaryManager.Instance.IngredientIds[data.nameId];
        if (Id == null)
            Debug.LogError($"Ingredient constructor failed to retrieve Id from Data Dictionary. Name: {data.nameId}");


        Level = data.level;
        Rarity = data.rarity;

        //effecs are constructed from their data and sorted between enhancements/impairments here
        EnhanceEffects = new List<Enhancement>();
        ImpairEffects = new List<Impairment>();
        foreach (var effectData in data.effectDatas.Values)
        {
            var effect = IngredientData.GetEffectFromData(effectData);

            if (effect is Enhancement enhancement)
                EnhanceEffects.Add(enhancement);

            else if (effect is Impairment impairment)
                ImpairEffects.Add(impairment);
        }
    }


    public IngredientData ToData() 
    {
        var effectDatas = new SerializableDictionary<EffectId, EffectData>();
        foreach (var enhancement in EnhanceEffects)
            effectDatas.Add(enhancement.Type(), enhancement.ToData());

        foreach (var impairment in ImpairEffects)
            effectDatas.Add(impairment.Type(), impairment.ToData());

        return new IngredientData(Id.stringId, Level, Rarity, effectDatas);
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
        string theString = $"Id: {Id.stringId}, lv: {Level}, rarity: {Rarity}, region: {Id.region}, taxonomy: {Id.taxonomy}";
        theString += "\n\tenhancements: ";
        foreach (var enhancement in EnhanceEffects)
        {
            theString += $"\n\t\t{enhancement}";
        }
        theString += "\n\timpairments: ";
        foreach (var impairment in ImpairEffects)
        {
            theString += $"\n\t\t{impairment}";
        }

        return theString;
    }
}
