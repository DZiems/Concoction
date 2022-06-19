using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Ingredient Blueprint", menuName = "Ingredient Blueprint")]
public class IngredientBlueprint : ScriptableObject
{
    public string identity;
    public Sprite artwork;

    [Header("Power Scaling Properties")]
    public int level = 1;
    public RarityTier rarity;

    [Header("Synergy Properties")]
    public Region region;
    public Taxonomy taxonomy;

    public EnhanceEffectBlueprint[] enhanceEffectBlueprints;
    public ImpairEffectBlueprint[] impairEffectBlueprints;

}
