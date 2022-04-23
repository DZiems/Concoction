using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MyDropdown : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private MyDropdownField dropdownFieldPrefab;
    [Header("Customization")]
    [SerializeField] string itemName;
    [SerializeField] string choosePrompt; 


    //main data
    public List<string> Items { get; private set; }
    public int Index { get; private set; }
    public int SelectedIndex { get; private set; }
    private const int defaultNoSelectionInd = -1;
    private const int deleteNumRequired = 5;
    private readonly string[] deleteTxt = new string[5] { "x1", "x2", "x3", "x4", "x5" };
    private int deletePressCountdown = deleteNumRequired;

    //tools & events
    public string CurrentItem => Items[Index];
    private List<string> DefaultEmpty => new List<string>() { $"New {itemName}" };
    private string DefaultCreateNew => $"New {itemName}";
    public bool IsHoveringSelected => Index == SelectedIndex;
    public int CreateNewInd => Items.Count - 1;
    public event Action onSelectCreateNew;
    public event Action onSelectionMade;
    public event Action<string> onDeletionMade;

    //dropdown references
    private MyDropdownRegion dropdownRegion;
    private CanvasGroup regionCanvasGroup;
    private Canvas regionCanvas;
    private Rect dropdownRegionRect;
    private float dropdownFieldHeight;

    //label reference
    private TextMeshProUGUI label;


    private void Awake()
    {
        Index = 0;
        Items = DefaultEmpty;
        deletePressCountdown = deleteNumRequired;
        if (string.IsNullOrEmpty(choosePrompt))
            choosePrompt = $"Choose {itemName}";

        label = GetComponentInChildren<TextMeshProUGUI>();

        //dropdown child references
        dropdownRegion = GetComponentInChildren<MyDropdownRegion>();
        regionCanvasGroup = dropdownRegion.GetComponentInChildren<CanvasGroup>();
        regionCanvas = dropdownRegion.GetComponentInChildren<Canvas>();

        //set select index to -1 (defaultUnselectedInd) and make label prompt
        ResetToUnselected();
        Hide();
    }

    private void Start()
    {
        //dimensions for a dropdown field
        dropdownRegionRect = dropdownRegion.GetComponent<RectTransform>().rect;
        dropdownFieldHeight = dropdownFieldPrefab.GetComponent<RectTransform>().rect.height;
    }

    public void Show()
    {
        regionCanvasGroup.interactable = true;
        regionCanvasGroup.alpha = 1.0f;
        regionCanvasGroup.blocksRaycasts = true;

        ResetDeleteCountdown();
        HoverIndexedField(true);
    }

    public void Hide()
    {
        regionCanvasGroup.interactable = false;
        regionCanvasGroup.alpha = 0.0f;
        regionCanvasGroup.blocksRaycasts = false;
    }


    //handle dropdown region
    private void BuildDropdown()
    {
        Debug.Log($"Dropdown Items Count: {Items.Count}");
        dropdownRegion.Clear();

        int i = 0;
        foreach (string item in Items)
        {
            InstantiateField(i, item);

            if (i == SelectedIndex)
                label.SetText(item);

            i++; 
        }
        if (SelectedIndex == defaultNoSelectionInd)
            label.SetText(choosePrompt);
    }


    private void InstantiateField(int ind, string item)
    {
        var field = Instantiate(dropdownFieldPrefab, regionCanvas.transform);
        dropdownRegion.fields.Add(field);
        StartCoroutine(ConfigureDropdownField(field, ind, item));
    }

    private IEnumerator ConfigureDropdownField(MyDropdownField field, int ind, string item)
    {
        yield return null;

        var fieldRectTransform = field.GetComponent<RectTransform>();
        fieldRectTransform.sizeDelta = new Vector2(dropdownRegionRect.width, dropdownFieldHeight);
        fieldRectTransform.anchoredPosition = new Vector2(0, -dropdownFieldHeight * (float)ind);

        field.ProfileTxt.SetText(item);

        //make sure "create new" field doesn't prompt player to delete it
        if (ind == CreateNewInd)
            field.DeletionField.SetActive(false);

        //basically the same code as HoverIndexedField()
        if (ind == Index)
        {
            field.Anim.SetBool("IsHovered", true);
            if (IsHoveringSelected)
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextSelected;
                field.BackgroundImg.color = ColorPallete.dropdownFieldSelectAndHighlight;
            }
            else
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextHighlighted;
                field.BackgroundImg.color = ColorPallete.dropdownFieldHighlighted;
            }
        }
        else if (ind == SelectedIndex)
        {
            field.Anim.SetBool("IsHovered", false);
            field.ProfileTxt.color = ColorPallete.dropdownTextSelected;
            field.BackgroundImg.color = ColorPallete.dropdownFieldSelected;
        }
        else
        {
            field.Anim.SetBool("IsHovered", false);
            field.ProfileTxt.color = ColorPallete.dropdownTextNormal;
            field.BackgroundImg.color = ColorPallete.dropdownFieldNormal;
        }
    }


    public void AddItemAndSelect(string item)
    {
        Items.Insert(Items.Count - 1, item);

        //note: don't call SelectCurrentItem() because this will call an event
        //(meaning behavior runs that is defined outside of this class definition.)
        Index = Items.Count - 2;
        UpdateSelection();

        BuildDropdown();
    }

    public void AddItems(List<string> items)
    {
        int oldCreateNewInd = CreateNewInd;
        this.Items.AddRange(items);

        //swap CreateNew field with newest added.
        string temp = items[oldCreateNewInd];
        items[oldCreateNewInd] = items[CreateNewInd];
        items[CreateNewInd] = temp;

        BuildDropdown();
    }

    public void SetItems(List<string> items)
    {
        this.Items = items;
        items.Add(DefaultCreateNew);
        BuildDropdown();
    }

    //don't allow removal of CreateNew field
    public void ClearItems()
    {
        Items = DefaultEmpty;
        BuildDropdown();
    }

    //don't allow removal of CreateNew field
    public void RemoveCurrentItem()
    {
        if (Index == CreateNewInd)
        {
            Debug.Log($"Index was at 'New Profile' field, so, not deleting");
            return;
        }
        if (onDeletionMade == null)
        {
            Debug.LogWarning("Dropdown 'onDeletionMade' method is null, which means the player can never delete an item.");
            return;
        }
        Debug.Log($"Inside Remove Item, removing {CurrentItem}");

        onDeletionMade(CurrentItem);

        Items.RemoveAt(Index);

        if (SelectedIndex == Index)
            ResetToUnselected();
        else
            SelectedIndex--;

        Index = 0;

        BuildDropdown();
    }

    private void ResetToUnselected()
    {
        SelectedIndex = defaultNoSelectionInd;
        label.SetText(choosePrompt);
    }

    public void DecrementItemDelete()
    {
        if (Index == CreateNewInd) return;

        deletePressCountdown--;
        Debug.Log($"DeletePressCount {deletePressCountdown} for item {CurrentItem} at index {Index}");
        if (deletePressCountdown <= 0)
        {
            RemoveCurrentItem();
            deletePressCountdown = deleteNumRequired;
        }
        else
        {
            var field = dropdownRegion.fields[Index];
            StartCoroutine(FlashDeleteRegion(field));
            field.DeletePressedText.SetText(deleteTxt[deletePressCountdown - 1]);
        }
    }
    private IEnumerator FlashDeleteRegion(MyDropdownField field)
    {
        field.DeletionField.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(0.1f);
        field.DeletionField.transform.localScale = Vector3.one;

    }

    private void ResetDeleteCountdown()
    {
        if (Index == CreateNewInd) return;

        if (deletePressCountdown != deleteNumRequired)
        {
            deletePressCountdown = deleteNumRequired;
            dropdownRegion.fields[Index].
                DeletePressedText.SetText(deleteTxt[deletePressCountdown - 1]);
        }
    }

    public void SelectCurrentItem()
    {
        if (Index != CreateNewInd)
        {
            UpdateSelection();

            if (onSelectionMade != null)
                onSelectionMade();
            else
                Debug.LogWarning("Dropdown 'onSelectionMade' method is null, which means the player can never select an item.");
        }
        else
        {
            if (onSelectCreateNew != null)
                onSelectCreateNew();
            else
                Debug.LogWarning("Dropdown 'onSelectCreateNew' method is null, which means the player can never create a new field.");
        }

    }

    private void UpdateSelection()
    {
        label.SetText(Items[Index]);

        //if selecting something new, reset old selection's color
        if (SelectedIndex != Index && !(SelectedIndex < 0))
        {
            Debug.Log($"Selected index on UpdateSelection(): {SelectedIndex}");
            var oldSelectedField = dropdownRegion.fields[SelectedIndex];

            oldSelectedField.ProfileTxt.color = ColorPallete.dropdownTextNormal;
            oldSelectedField.BackgroundImg.color = ColorPallete.dropdownFieldNormal;
        }

        SelectedIndex = Index;
    }

    //items are built top to bottom, so moving "down" increments downward
    public void MoveDown()
    {
        if (Items.Count <= 0) return;

        ResetDeleteCountdown();

        HoverIndexedField(false);
        if (Index >= Items.Count - 1)
            Index = 0;
        else
            Index++;

        HoverIndexedField(true);

    }

    public void MoveUp()
    {
        if (Items.Count <= 0) return;

        ResetDeleteCountdown();

        HoverIndexedField(false);

        if (Index <= 0)
            Index = Items.Count - 1;

        else
            Index--;

        HoverIndexedField(true);

    }

   


    private void HoverIndexedField(bool on)
    {
        var field = dropdownRegion.fields[Index];
        field.Anim.SetBool("IsHovered", on);

        if (on)
        {
            if (IsHoveringSelected)
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextSelected;
                field.BackgroundImg.color = ColorPallete.dropdownFieldSelectAndHighlight;
            }
            else
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextHighlighted;
                field.BackgroundImg.color = ColorPallete.dropdownFieldHighlighted;
            }
        }
        else
        {
            if (IsHoveringSelected)
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextSelected;
                field.BackgroundImg.color = ColorPallete.dropdownFieldSelected;
            }
            else
            {
                field.ProfileTxt.color = ColorPallete.dropdownTextNormal;
                field.BackgroundImg.color = ColorPallete.dropdownFieldNormal;
            }
        }
    }

}
