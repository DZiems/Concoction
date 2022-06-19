using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProfileMenu : MonoBehaviour
{
    [SerializeField] private MyDropdown dropdown;
    [SerializeField] private AlphabetFieldPrompt alphabetFieldPrompt;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject goBackButton;

    //state machine
    private UIFiniteStateMachine stateMachine;
    public InMainState mainState { get; private set; }
    public InDropdownState dropdownState { get; private set; }
    public InCreateProfileState createProfileState { get; private set; }
    public InConfirmedState confirmedState { get; private set; }

    public Animator Anim { get; private set; }

    public string CurrentSelectedProfile => dropdown.SelectedItem;

    public bool HasConfirmed => stateMachine.CurrentState == confirmedState;


    private void Awake()
    {
        Anim = GetComponent<Animator>();

        stateMachine = new UIFiniteStateMachine();
        var controller = PlayerManager.Instance.Player.Controller;

        var dropdownAnimator = dropdown.GetComponent<Animator>();
        var dropdownText = dropdown.GetComponentInChildren<TextMeshProUGUI>();

        var confirmAnimator = confirmButton.GetComponent<Animator>();
        var confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();

        var goBackAnimator = goBackButton.GetComponent<Animator>();
        var goBackText = goBackButton.GetComponentInChildren<TextMeshProUGUI>();

        //states
        mainState = new InMainState(this, controller, stateMachine, dropdownAnimator, dropdownText, confirmAnimator, confirmText, goBackAnimator, goBackText);

        dropdownState = new InDropdownState(this, controller, stateMachine, dropdown);

        createProfileState = new InCreateProfileState(this, controller, stateMachine, alphabetFieldPrompt);

        confirmedState = new InConfirmedState(this, controller, stateMachine);

        stateMachine.Initialize(mainState);

    }

    //TODO: fix deletion causing wrong item to be selected.
    private void Start()
    {
        dropdown.Initialize(PlayerManager.Instance.allPlayerProfileDatas.Keys.ToList());

        Anim.SetBool("Active", true);
    }

    private void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    public void HandleGoBack()
    {
        GameManager.Instance.ResetGameToMainMenu();
    }

    // profileMenu.Anim.SetBool("Active", true);

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
