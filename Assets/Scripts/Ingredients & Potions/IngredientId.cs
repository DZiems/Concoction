using UnityEngine;

[CreateAssetMenu(fileName = "New IngredientId", menuName = "Ingredients/Id")]
public class IngredientId : ScriptableObject
{
    [Header("Referencing")]
    public string stringId;
    public Sprite icon;

    [Header("Synergy Properties")]
    public Region region;
    public Taxonomy taxonomy;

}