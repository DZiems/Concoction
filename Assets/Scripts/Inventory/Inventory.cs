using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


public class Inventory : MonoBehaviour, IDataPersistence
{
    public const int MAX_SIZE = 50;
    private bool isInitialized;

    public List<Ingredient> Ingredients {get; private set;} = new List<Ingredient>();


    public void LoadData(GameData data)
    {
        if (data.CurrentPlayerProfileData == null)
        {
            if (!SceneManager.Instance.IsCurrentSceneAMenu())
                Debug.LogWarning("Inventory is loading before a player has chosen a profile (fine if testing from somewhere other than main menu). Inventory will be empty.");
            else
                Debug.Log("Inventory doesn't have a player yet, so LoadData() do nothing.");
            return;
        }

        InitializeFromData(data.CurrentPlayerProfileData.inventoryData);
    }
    public void SaveData(GameData data)
    {
        if (data.CurrentPlayerProfileData == null)
        {
            Debug.LogWarning("Inventory is saving before a player has chosen a profile (fine if testing from somewhere other than main menu). Returning without saving.");
            return;
        }

        OverwriteData(data);

    }

    private void OverwriteData(GameData data)
    {
        var ingredientDatas = new IngredientData[Ingredients.Count];
        int i = 0;

        foreach (var ingredient in Ingredients)
            ingredientDatas[i++] = ingredient.ToData();


        data.CurrentPlayerProfileData.inventoryData = new InventoryData(ingredientDatas);
    }

    public void InitializeFromData(InventoryData data)
    {
        Ingredients.Capacity = data.ingredientDatas.Length;

        foreach (var ingData in data.ingredientDatas)
            Ingredients.Add(new Ingredient(ingData));


        Debug.Log("Inventory Ingredients initialized:");
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Debug.Log($"ingredient: {Ingredients[i]}");
        }

        isInitialized = true;
    }

    public void AddItem(Ingredient ingredient)
    {
        if (!isInitialized) return;

        Ingredients.Add(ingredient);
    }

    public void RemoveIngredient(int ind)
    {
        if (!isInitialized) return;

        Ingredients.RemoveAt(ind);
    }

    public void SortIngredientsByLevel()
    {
        if (!isInitialized) return;

    }
   

}
