using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryIngredientsTable : MonoBehaviour
{
    [SerializeField] private UIHoverableText tableItemPrefab;

    private Ingredient[] ingredients;
    private UIHoverableText[] tableItems;
    private int itemsIndex;
    private int numItems;

    private UIHoverableText CurrentTableItem => tableItems[itemsIndex];
    
    //later this will change to (1 + pageNumber) * itemsIndex
    public Ingredient CurrentHoveredIngredient => ingredients[itemsIndex];



    Rect rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>().rect;
        if (rect == null)
        {
            Debug.LogError("rect of UIInventoryItemTable is null");
        }
    }

    public void Initialize(int numItems)
    {
        tableItems = new UIHoverableText[numItems];
        float itemHeight = rect.height / numItems;
        for (int i = 0; i < numItems; i++)
        {
            tableItems[i] = Instantiate(tableItemPrefab, this.transform);
            var itemRectTransform = tableItems[i].GetComponent<RectTransform>();
            itemRectTransform.sizeDelta = new Vector2(rect.width, itemHeight);

            itemRectTransform.anchorMin = Vector2.up;
            itemRectTransform.anchorMax = Vector2.up;
            itemRectTransform.pivot = Vector2.up;

            itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * i);

            tableItems[i].Deactivate();
        }
        this.numItems = 0;
        itemsIndex = 0;
    }

    public void Build(Inventory inventory)
    {
        ingredients = inventory.Ingredients.ToArray();

        if (ingredients.Length > tableItems.Length)
        {
            Debug.LogError("UIInventoryTable passed a larger ingredients list than can be built out in Build(). LATER THIS SHOULD CHANGE WITH PAGES. FOR NOW, cancelling build.");
            return;
        }
        Debug.Log($"Ingredients count: {ingredients.Length}");

        int i = 0; 
        numItems = ingredients.Length;
        foreach (var ingredient in ingredients)
        {
            tableItems[i].SetText(ingredient.Id.stringId);
            tableItems[i].Unhover();
            i++;
        }
        while (i < tableItems.Length)
        {
            tableItems[i].Deactivate();
            i++;
        }

        if (numItems > itemsIndex)
            itemsIndex = 0;

        if (numItems > 0)
            CurrentTableItem.Hover();

    }


    public void ScrollDown()
    {
        if (numItems <= 0) return;

        CurrentTableItem.Unhover();

        itemsIndex++;
        if (itemsIndex > numItems - 1)
            itemsIndex = 0;

        CurrentTableItem.Hover();
        Debug.Log("display item details on details page");
    }
    public void ScrollUp()
    {
        if (numItems <= 0) return;

        CurrentTableItem.Unhover();

        itemsIndex--;
        if (itemsIndex < 0)
            itemsIndex = numItems - 1;

        CurrentTableItem.Hover();
        Debug.Log("display item details on details page");
    }
}
