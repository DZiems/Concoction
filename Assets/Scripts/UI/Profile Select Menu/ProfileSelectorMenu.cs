using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProfileSelectorMenu : MonoBehaviour
{
    [SerializeField] private GameObject allContents;

    //access player data for dropdown options
    private Dictionary<string, PlayerCharacterData> allPlayerCharacterData;
    //player controlling the instance
    private Player player;
    private bool isInitialized => player != null;

    //menu states
    public enum State
    {
        InMain,
        InDropdown,
        InCreateNewProfile,
        InConfigure
    }
    private State currentMenuState;
    private bool isConfirmActive;
    public bool HasConfirmed { get; private set; }

    //menu options
    public enum Option
    {
        Dropdown,
        Configure,
        Leave,
        Confirm
    }

    //menu references
    private Animator mainAnimator;
    private ProfileSelectButton[] buttons;
    private TextMeshProUGUI[] buttonTexts;
    private Animator[] buttonAnims;
    private int buttonsInd;
    private int confirmInd;
    private int leaveInd;
    private int dropdownInd;

    //profile dropdown
    private MyDropdown dropdown;

    //setting a new profile
    private CreateNewProfilePanel createNewProfilePanel;

    //events
    public event Action<Player> onLeaveSelected;
    public event Action<Player> onConfirmSelected;
    public event Action<Player> onConfirmCanceled;

    //profile
    public string profileChosen => dropdown.CurrentItem;

    private void Awake()
    {
        mainAnimator = allContents.GetComponent<Animator>();

        //get UI button references
        buttons = allContents.GetComponentsInChildren<ProfileSelectButton>();
        buttonTexts = new TextMeshProUGUI[buttons.Length];
        buttonAnims = new Animator[buttons.Length];
        isConfirmActive = false;
        HasConfirmed = false;

        int i = 0;
        foreach (var button in buttons)
        {
            if (buttons[i].Id == Option.Confirm)
                confirmInd = i;
            else if (buttons[i].Id == Option.Leave)
                leaveInd = i;
            else if (buttons[i].Id == Option.Dropdown)
                dropdownInd = i;

            buttonTexts[i] = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonAnims[i++] = button.GetComponent<Animator>();
        }

        //get bigger object references
        dropdown = allContents.GetComponentInChildren<MyDropdown>();
        createNewProfilePanel = allContents.GetComponentInChildren<CreateNewProfilePanel>();
    }

    private void Start()
    {
        allPlayerCharacterData = PlayerManager.Instance.AllPlayerCharacterData;

        //initialize dropdown data
        dropdown.SetItems(allPlayerCharacterData.Keys.ToList());

        //register to profile selection/creation events
        dropdown.onSelectCreateNew += HandleDropdown_OnSelectCreateNew;
        dropdown.onSelectionMade += HandleDropdown_OnSelectionMade;
        dropdown.onDeletionMade += HandleDropdown_OnDeletionMade;

        //initialize all buttons
        //note: ensure confirm button never starts on.
        int i = 0;
        foreach (var button in buttons)
        {
            buttonAnims[i].SetBool("IsHovered", false);
            buttonTexts[i++].color = ColorPallete.unselectedColor;
        }
        TurnConfirmOff();

        //initialize menu state and which button is first hovered
        //note: ensure this never starts as the confirm button
        currentMenuState = State.InMain;
        buttonsInd = dropdownInd;
        HoverIndexedButton(true);
    }

    public void Initialize(Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (!isInitialized) return;

        switch (currentMenuState)
        {
            case State.InMain:
                //if player has confirmed, player can only press slot0 (b)
                if (HasConfirmed)
                    HandleCancelConfirm();
                else
                    HandleMain();
                break;
            case State.InDropdown:
                HandleDropdown();
                break;
            case State.InCreateNewProfile:
                HandleCreateNewProfile();
                break;
            case State.InConfigure:
                break;
            default:
                break;
        }
    }

    private void HandleMain()
    {
        //handle movement
        if (player.Controller.VerticalDown > 0)
        {
            HoverIndexedButton(false);

            IncrementButtonsInd();
            if (buttons[buttonsInd].Id == Option.Confirm && !isConfirmActive)
                IncrementButtonsInd();

            HoverIndexedButton(true);
        }
        else if (player.Controller.VerticalDown < 0)
        {
            HoverIndexedButton(false);

            DecrementButtonsInd();
            if (buttons[buttonsInd].Id == Option.Confirm && !isConfirmActive)
                DecrementButtonsInd();

            HoverIndexedButton(true);
        }
        //selection
        else if (player.Controller.InteractDown)
        {
            HandleMain_Select();
        }
        //cancel
        else if (player.Controller.Slot0Down)
        {
            HandleMain_Cancel();
        }
    }

    private void HandleMain_Select()
    {
        switch (buttons[buttonsInd].Id)
        {
            case Option.Dropdown:
                OpenDropdown();
                break;
            case Option.Configure:
                Debug.Log("Configure selected");
                break;
            case Option.Leave:
                onLeaveSelected(player);
                break;
            case Option.Confirm:
                HandleConfirm();
                break;
        }
    }

    private void HandleMain_Cancel()
    {
        if (buttons[buttonsInd].Id != Option.Leave)
            JumpToButton(leaveInd);
        else
            onLeaveSelected(player);
    }

    private void OpenDropdown()
    {
        HoverIndexedButton(false);
        currentMenuState = State.InDropdown;

        dropdown.Show();
    }

    private void HandleDropdown()
    {
        //movement
        if (player.Controller.VerticalDown > 0)
        {
            dropdown.MoveUp();
        }
        else if (player.Controller.VerticalDown < 0)
        {
            dropdown.MoveDown();
        }
        //selection
        else if (player.Controller.InteractDown)
        {
            //sends either onSelectionMade() or onSelectCreateNew() based on whether Create New was selected
            HandleDropdown_SelectItem();
        }
        //deletion
        else if (player.Controller.Slot1Down)
        {
            HandleDropdown_DeleteItem();
        }
        // cancel
        else if (player.Controller.Slot0Down)
        {
            HandleDropdown_Cancel();
        }
    }

    private void HandleDropdown_DeleteItem()
    {
        //takes a certain number of delete presses, then event onDeletionMade() is called
        dropdown.DecrementItemDelete();
    }

    private void HandleDropdown_SelectItem()
    {
        //calls either onCreateNew or onSelectionMade
        dropdown.SelectCurrentItem();
    }

    private void HandleDropdown_OnSelectionMade()
    {
        Debug.Log($"{dropdown.CurrentItem} selected");
        dropdown.Hide();
        isConfirmActive = true;

        TurnConfirmOn();
        JumpToButton(confirmInd);

        currentMenuState = State.InMain;
    }

    private void HandleDropdown_Cancel()
    {
        dropdown.Hide();
        HoverIndexedButton(true);
        currentMenuState = State.InMain;
    }

    private void HandleDropdown_OnDeletionMade(string deletedProfileName)
    {
        Debug.Log($"Profile Selector Menu -- Deleting profile: {deletedProfileName}"); 

        //if player deletes their selected profile
        if (dropdown.IsHoveringSelected)
            TurnConfirmOff();

        PlayerManager.Instance.RemovePlayerProfile(deletedProfileName);
        
    }

    private void HandleDropdown_OnSelectCreateNew()
    {
        currentMenuState = State.InCreateNewProfile;
        createNewProfilePanel.Show();
    }

    private void HandleCreateNewProfile()
    {
        if (player.Controller.HorizontalDown > 0f)
        {
            createNewProfilePanel.alphabetField.MoveRight();
        }
        else if (player.Controller.HorizontalDown < 0f)
        {
            createNewProfilePanel.alphabetField.MoveLeft();
        }
        if (player.Controller.VerticalDown > 0f)
        {
            createNewProfilePanel.alphabetField.MoveDown();
        }
        else if (player.Controller.VerticalDown < 0f)
        {
            createNewProfilePanel.alphabetField.MoveUp();
        }

        if (player.Controller.InteractDown)
        {
            HandleCreateNewProfile_SelectLetter();
        }
        else if (player.Controller.Slot1Down)
        {
            HandleCreateNewProfile_Backspace();
        }
        else if (player.Controller.RollDown || player.Controller.SpecialDown)
        {
            HandleCreateNewProfile_ToggleCasing();
        }

        else if (player.Controller.Slot0Down)
        {
            createNewProfilePanel.Hide();
            currentMenuState = State.InDropdown;
        }
        else if (player.Controller.StartDown)
        {
            HandleCreateNewProfile_Confirm();
        }
    }

    private void HandleCreateNewProfile_ToggleCasing()
    {
        createNewProfilePanel.alphabetField.ToggleCasing();
        createNewProfilePanel.FlashShift();
    }

    private void HandleCreateNewProfile_SelectLetter()
    {
        createNewProfilePanel.FlashSelect();
        createNewProfilePanel.AddLetter();
    }

    private void HandleCreateNewProfile_Backspace()
    {
        createNewProfilePanel.FlashDelete();
        createNewProfilePanel.DeleteLetter();
    }

    private void HandleCreateNewProfile_Confirm()
    {
        dropdown.AddItemAndSelect(createNewProfilePanel.ProfileName);

        PlayerManager.Instance.AddNewPlayerProfile(createNewProfilePanel.ProfileName);

        createNewProfilePanel.Hide();
        dropdown.Hide();

        TurnConfirmOn();
        JumpToButton(dropdownInd);

        currentMenuState = State.InMain;
    }

    private void HandleConfirm()
    {
        if (onConfirmSelected != null)
            onConfirmSelected(player);
        else
            Debug.LogError("ProfileSelectorMenu onConfirmSelected() was null!");

        HasConfirmed = true;
        HoverIndexedButton(false);

        mainAnimator.SetTrigger("HasConfirmed");
    }

    private void HandleCancelConfirm()
    {
        if (player.Controller.Slot0Down)
        {
            if (onConfirmCanceled != null)
                onConfirmCanceled(player);
            else
                Debug.LogError("ProfileSelectorMenu onConfirmCanceled() was null!");

            HasConfirmed = false;

            mainAnimator.SetTrigger("HasCanceled");
            JumpToButton(confirmInd);
        }
    }

    private void HoverIndexedButton(bool on)
    {
        buttonAnims[buttonsInd].SetBool("IsHovered", on);

        if (on)
            buttonTexts[buttonsInd].color = ColorPallete.selectedColor;
        else
            buttonTexts[buttonsInd].color = ColorPallete.unselectedColor;
    }

    private void JumpToButton(int buttonInd)
    {

        if (buttonInd < 0 || buttonInd > buttons.Length - 1) return;
        if (!isConfirmActive && confirmInd == buttonInd) return;

        HoverIndexedButton(false);
        buttonsInd = buttonInd;
        HoverIndexedButton(true);

    }

    private void IncrementButtonsInd()
    {
        buttonsInd++;
        if (buttonsInd >= buttons.Length)
            buttonsInd = 0;
    }

    private void DecrementButtonsInd()
    {
        buttonsInd--;
        if (buttonsInd < 0)
            buttonsInd = buttons.Length - 1;
    }

    private void TurnConfirmOn()
    {
        isConfirmActive = true;

        buttonTexts[confirmInd].color = ColorPallete.unselectedColor;
    }

    private void TurnConfirmOff()
    {
        isConfirmActive = false;

        buttonTexts[confirmInd].color = ColorPallete.inactiveColor;
    }
}
