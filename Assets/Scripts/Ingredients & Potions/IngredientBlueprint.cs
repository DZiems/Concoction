using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New IngredientBlueprint", menuName = "Ingredients/Blueprint")]
public class IngredientBlueprint : ScriptableObject
{
    [Header("Id")]
    public IngredientId id;

    [Header("Rarity Tier Chances")]
    public RarityTierRoller rarityRoller;

    [Header("Effects")]
    public EffectRoller effectRoller;

}


