using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MyDropdown : UIHoverableItem
{

    [Header("Prefabs")]
    [SerializeField] private MyDropdownField dropdownFieldPrefab;

    [Header("Customization")]
    [SerializeField] string newPrompt;
    [SerializeField] string choosePrompt; 

    public enum SelectResult
    {
        Item,
        CreateNew
    }

    //main data
    public List<string> Items { get; private set; }
    public int Index { get; private set; }
    public int SelectedIndex { get; private set; }
    private const int defaultNoSelectionInd = -1;
    private const int deleteNumRequired = 5;
    private readonly string[] deleteTxt = new string[5] { "x1", "x2", "x3", "x4", "x5" };
    private int deleteCounter = deleteNumRequired;

    //tools & events
    private string CurrentItem => Items[Index];
    public string SelectedItem => SelectedIndex != defaultNoSelectionInd ? Items[SelectedIndex] : null;

    private List<string> DefaultEmpty => new List<string>() { newPrompt };
    public bool IsHoveringSelected => Index == SelectedIndex;
    public int CreateNewInd => Items.Count - 1;


    //dropdown references
    private MyDropdownRegion dropdownRegion;
    private CanvasGroup regionCanvasGroup;
    private Canvas regionCanvas;

    //label reference
    private TextMeshProUGUI label;


    protected override void Awake()
    {
        base.Awake();

        label = GetComponentInChildren<TextMeshProUGUI>();

        //dropdown child references
        dropdownRegion = GetComponentInChildren<MyDropdownRegion>();
        regionCanvasGroup = dropdownRegion.GetComponentInChildren<CanvasGroup>();
        regionCanvas = dropdownRegion.GetComponentInChildren<Canvas>();

        Index = 0;
        Items = DefaultEmpty;
        deleteCounter = deleteNumRequired;

        //set select index to -1 (defaultUnselectedInd) and make label prompt
        GoUnselected();
        Hide();
    }


    public void Initialize(List<string> items)
    {
        this.Items = items;
        items.Add(newPrompt);
        BuildDropdown();
    }

    public void Show()
    {
        regionCanvasGroup.interactable = true;
        regionCanvasGroup.alpha = 1.0f;
        regionCanvasGroup.blocksRaycasts = true;

        ResetDeleteCounter();
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
        dropdownRegion.Clear();

        int ind = 0;
        foreach (string item in Items)
        {
            BuildField(ind, item);

            if (ind == SelectedIndex)
                label.SetText(item);

            ind++; 
        }
        if (SelectedIndex == defaultNoSelectionInd)
            label.SetText(choosePrompt);
    }


    private void BuildField(int ind, string item)
    {
        var field = Instantiate(dropdownFieldPrefab, regionCanvas.transform);
        dropdownRegion.fields.Add(field);
        StartCoroutine(ConfigureDropdownField(field, ind, item));
    }

    private IEnumerator ConfigureDropdownField(MyDropdownField field, int ind, string item)
    {
        yield return null;


        var fieldRectTransform = field.GetComponent<RectTransform>();
        fieldRectTransform.sizeDelta = new Vector2(dropdownRegion.Width, field.Height);
        fieldRectTransform.anchoredPosition = new Vector2(0, -field.Height * ind);


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



    private void DeleteCurrentItem()
    {
        if (SelectedIndex == Index)
            GoUnselected();
        else if (SelectedIndex > Index)
            SelectedIndex--;

        Items.RemoveAt(Index);

        Index = 0;

        BuildDropdown();
    }
    private void GoUnselected()
    {
        SelectedIndex = defaultNoSelectionInd;
        label.SetText(choosePrompt);
    }

    //don't allow removal of CreateNew field
    public string TryDelete()
    {
        if (Index == CreateNewInd) 
            return null;

        deleteCounter--;
        if (deleteCounter <= 0)
        {
            string itemToDelete = CurrentItem;
            DeleteCurrentItem();
            deleteCounter = deleteNumRequired;
            return itemToDelete;
        }
        else
        {
            var field = dropdownRegion.fields[Index];
            StartCoroutine(FlashDelete(field));
            field.DeletePressedText.SetText(deleteTxt[deleteCounter - 1]);
            return null;
        }
    }
    private IEnumerator FlashDelete(MyDropdownField field)
    {
        field.DeletionField.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(0.1f);
        field.DeletionField.transform.localScale = Vector3.one;

    }
    private void ResetDeleteCounter()
    {
        if (Index == CreateNewInd) return;

        if (deleteCounter != deleteNumRequired)
        {
            deleteCounter = deleteNumRequired;
            dropdownRegion.fields[Index].
                DeletePressedText.SetText(deleteTxt[deleteCounter - 1]);
        }
    }



    public SelectResult Select()
    {
        if (Index != CreateNewInd)
        {
            UpdateSelection();
            return SelectResult.Item;
        }
        else
        {
            return SelectResult.CreateNew;
        }

    }

    private void UpdateSelection()
    {
        label.SetText(Items[Index]);

        //if selecting something new, reset old selection's color
        if (SelectedIndex != Index && SelectedIndex != defaultNoSelectionInd)
        {
            var oldSelectedField = dropdownRegion.fields[SelectedIndex];

            oldSelectedField.ProfileTxt.color = ColorPallete.dropdownTextNormal;
            oldSelectedField.BackgroundImg.color = ColorPallete.dropdownFieldNormal;
        }

        SelectedIndex = Index;
    }

    //items are built top to bottom, so moving "down" increments downward
    public void ScrollDown()
    {
        if (Items.Count <= 0) return;

        ResetDeleteCounter();

        HoverIndexedField(false);
        if (Index >= Items.Count - 1)
            Index = 0;
        else
            Index++;

        HoverIndexedField(true);

    }

    public void ScrollUp()
    {
        if (Items.Count <= 0) return;

        ResetDeleteCounter();

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
