using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


public class Inventory : MonoBehaviour, IDataPersistence
{
    public const int MAX_SIZE = 50;
    private bool isInitialized;

    public ulong NumIngredientsAcquired { get; private set; }

    public List<Ingredient> Ingredients {get; private set;} = new List<Ingredient>();
    public List<ulong> OrdersIngredientsAcquired { get; private set;} = new List<ulong>();

    public event Action onChanged;

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

        Initialize(data.CurrentPlayerProfileData.inventoryData);
    }
    public void SaveData(GameData data)
    {
        if (data.CurrentPlayerProfileData == null)
        {
            Debug.LogWarning("Inventory is saving before a player has chosen a profile (fine if testing from somewhere other than main menu). Returning without saving.");
            return;
        }

        OverwriteDataContents(data);

    }

    private void OverwriteDataContents(GameData data)
    {
        var ingredientDatas = new IngredientData[Ingredients.Count];
        int i = 0;
        foreach (var ingredient in Ingredients)
            ingredientDatas[i++] = ingredient.ToData();


        data.CurrentPlayerProfileData.inventoryData = new InventoryData(ingredientDatas, OrdersIngredientsAcquired.ToArray(), NumIngredientsAcquired);
    }

    public void Initialize(InventoryData data)
    {
        NumIngredientsAcquired = data.numIngredientsAcquired;

        Ingredients.Capacity = data.ingredientDatas.Length;
        OrdersIngredientsAcquired.Capacity = data.ingredientDataOrdersAcquired.Length;
        foreach (var ingData in data.ingredientDatas)
            AddItem(new Ingredient(ingData));

        foreach (ulong orderAcquired in data.ingredientDataOrdersAcquired)
            OrdersIngredientsAcquired.Add(orderAcquired);

        Debug.Log("Inventory Initialize(): Ingredients initialized:");
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Debug.Log($"Order Acquired: {OrdersIngredientsAcquired[i]}, ingredient: {Ingredients[i]}");
        }

        onChanged?.Invoke();
        isInitialized = true;
    }

    public void AddItem(Ingredient ingredient)
    {
        if (!isInitialized) return;

        Ingredients.Add(ingredient);

        NumIngredientsAcquired++;
        OrdersIngredientsAcquired.Add(NumIngredientsAcquired);

        onChanged?.Invoke();
    }

    public void RemoveIngredient(int ind)
    {
        if (!isInitialized) return;

        Ingredients.RemoveAt(ind);
        OrdersIngredientsAcquired.RemoveAt(ind);

        onChanged?.Invoke();
    }

    public void SortIngredientsByLevel()
    {
        if (!isInitialized) return;

    }
   

}
