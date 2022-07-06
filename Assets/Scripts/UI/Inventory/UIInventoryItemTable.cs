using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryItemTable : MonoBehaviour
{
    [SerializeField] private UIHoverableItem tableItemPrefab;
    private UIHoverableItem CurrentItem => TableItems[currentItemIndex];
    private int currentItemIndex;
    private int currentNumItems;

    public UIHoverableItem[] TableItems { get; private set; }
    private bool isInitialized;
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
        TableItems = new UIHoverableItem[numItems];
        float itemHeight = rect.height / numItems;
        for (int i = 0; i < numItems; i++)
        {
            TableItems[i] = Instantiate(tableItemPrefab, this.transform);
            var itemRectTransform = TableItems[i].GetComponent<RectTransform>();
            itemRectTransform.sizeDelta = new Vector2(rect.width, itemHeight);

            itemRectTransform.anchorMin = Vector2.up;
            itemRectTransform.anchorMax = Vector2.up;
            itemRectTransform.pivot = Vector2.up;

            itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * i);

            TableItems[i].Unhover();
        }
        currentNumItems = 0;
    }

    public void Build(List<Ingredient> ingredients)
    {
        if (ingredients.Count > TableItems.Length)
            Debug.LogError("UIInventoryTable passed a larger ingredients list than can be built out in Build(). Cancelling build.");

        int i = 0; 
        currentNumItems = ingredients.Count;
        foreach (var ingredient in ingredients)
        {
            TableItems[i].SetText(ingredient.Id.stringId);
            TableItems[i].Unhover();
            i++;
        }
        while (i < TableItems.Length)
        {
            TableItems[i].Deactivate();
        }

        if (currentNumItems > currentItemIndex)
            currentItemIndex = 0;

        CurrentItem.Hover();

    }


    public void ScrollDown()
    {
        CurrentItem.Unhover();

        currentItemIndex++;
        if (currentItemIndex > currentNumItems - 1)
            currentItemIndex = 0;

        CurrentItem.Hover();
        Debug.Log("display item details on details page");
    }
    public void ScrollUp()
    {
        CurrentItem.Unhover();

        currentItemIndex--;
        if (currentItemIndex < 0)
            currentItemIndex = currentNumItems - 1;

        CurrentItem.Hover();
        Debug.Log("display item details on details page");
    }
}
