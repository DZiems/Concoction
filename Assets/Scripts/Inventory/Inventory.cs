using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour, IDataPersistence
{
    [SerializeField] IngredientBlueprint testIngredientBlueprint;
    public const int MAX_SIZE = 50;

    private List<Ingredient> ingredients = new List<Ingredient>();
    private List<Potion> potions = new List<Potion>();

    private void Awake()
    {
        
    }

    public void AddItem(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }
    public void AddItem(Potion potion)
    {
        potions.Add(potion);
    }

    public void LoadData(GameData data)
    {
        InventoryData inventoryData = data.CurrentPlayerProfileData.inventoryData;
        foreach (var ingredientData in inventoryData.ingredientDatas)
        {
            ingredients.Add(new Ingredient(ingredientData));
        }
        Debug.Log("Ingredients loaded:");
        foreach (var ingredient in ingredients)
        {
            Debug.Log(ingredient);
        }
    }

    public void SaveData(GameData data)
    {
        InventoryData inventoryData = data.CurrentPlayerProfileData.inventoryData;
        inventoryData.ingredientDatas = new IngredientData[ingredients.Count];
        int i = 0;
        foreach (var ingredient in ingredients)
        {
            inventoryData.ingredientDatas[i++] = ingredient.GetData();
        }

        Debug.Log("Printing all ingredients to be saved.");
        foreach (var ingredient in ingredients)
        {
            Debug.Log(ingredient);
        }
    }

    [ContextMenu("Add Test Ingredient")]
    public void AddTestIngredient()
    {
        AddItem(new Ingredient(testIngredientBlueprint));
    }
}
