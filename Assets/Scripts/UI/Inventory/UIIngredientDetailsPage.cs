using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIngredientDetailsPage : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI ingredientStringId;
    [SerializeField] private TextMeshProUGUI ingredientRarity;
    [SerializeField] private TextMeshProUGUI ingredientRegion;
    [SerializeField] private TextMeshProUGUI ingredientTaxonomy;

    public void DisplayIngredientDetails(Ingredient ingredient)
    {
        ingredientStringId.SetText(ingredient.Id.stringId);
        ingredientRarity.SetText(ingredient.Rarity.ToString());
        ingredientRegion.SetText(ingredient.Id.region.ToString());
        ingredientTaxonomy.SetText(ingredient.Id.taxonomy.ToString());


    }
}
