using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
    [SerializeField] private AlphabetFieldPrompt alphabetFieldPrompt;
    [SerializeField] private MyDropdown dropdown;
    [SerializeField] private UIHoverableText confirmButton;
    [SerializeField] private UIHoverableText goBackButton;

    //state machine
    private UIFiniteStateMachine stateMachine;
    public InMainState mainState { get; private set; }
    public InDropdownState dropdownState { get; private set; }
    public InCreateProfileState createProfileState { get; private set; }
    public InConfirmedState confirmedState { get; private set; }

    public string CurrentSelectedProfile => dropdown.SelectedItem;

    public bool HasConfirmed => stateMachine.CurrentState == confirmedState;


    private void Awake()
    {
        stateMachine = new UIFiniteStateMachine();
        var controller = PlayerManager.Instance.Player.Controller;


        //states
        mainState = new InMainState(this, controller, stateMachine, dropdown, confirmButton, goBackButton);

        dropdownState = new InDropdownState(this, controller, stateMachine, dropdown);

        createProfileState = new InCreateProfileState(this, controller, stateMachine, alphabetFieldPrompt);

        confirmedState = new InConfirmedState(this, controller, stateMachine);

        stateMachine.Initialize(mainState);
    }

    //TODO: fix deletion causing wrong item to be selected.
    private void Start()
    {
        var currentProfileDatas = DataPersistenceManager.Instance.AllProfileDatas;
        dropdown.Initialize(currentProfileDatas.Keys.ToList());

        alphabetFieldPrompt.Hide();
    }

    private void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    public void HandleGoBack()
    {
        SceneManager.Instance.ResetGameToMainMenu();
    }

    public void HandleCreateProfile(string profileToAdd)
    {
        DataPersistenceManager.Instance.AddNewPlayerProfile(profileToAdd);
        dropdown.AddItemAndSelect(profileToAdd);
    }

    public void HandleDeleteProfile(string profileToDelete)
    {
        DataPersistenceManager.Instance.DeletePlayerProfile(profileToDelete);
    }
}
