using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//TODO: instantiate all 4 profile menus, then just call setactive instead of destroy

//TODO: implement confirmation and scene transition

public class ProfileSelectMenuHandler : MonoBehaviour
{
    [SerializeField] private ProfileSelectorMenu profileSelectorPrefab;
    [SerializeField] private TextMeshProUGUI continueText;


    private Animator continueTextAnim;
    public Dictionary<Player, ProfileSelectorMenu> currentProfileSelectors { get; private set; }
    
    public int[] currentSelectedProfiles { get; private set; }
    private int numConfirmedPlayers;

    private Player playerOne; 
    private bool allPlayersHaveConfirmed => numConfirmedPlayers == currentProfileSelectors.Count;

    private void Awake()
    {
        currentProfileSelectors = new Dictionary<Player, ProfileSelectorMenu>();
        //TODO: TOTAL_NUM_PLAYERS
        currentSelectedProfiles = new int[4];
        numConfirmedPlayers = 0;

        continueTextAnim = continueText.GetComponent<Animator>();
        continueText.color = ColorPallete.inactiveColor;
    }

    private void Start()
    {
        foreach (var player in PlayerManager.Instance.CurrentPlayers)
        {
            AddProfileMenu(player);
        }

        PlayerManager.Instance.onPlayerTwoToFourJoined += AddProfileMenu;
        playerOne = PlayerManager.Instance.PlayerOne;


    }

    private void OnDisable()
    {
        PlayerManager.Instance.onPlayerTwoToFourJoined -= AddProfileMenu;

    }

    private void Update()
    {
        if (allPlayersHaveConfirmed)
        {
            HandleAllPlayersConfirmed();
        }
    }

    private void AddProfileMenu(Player player)
    {
        if (profileSelectorPrefab == null) return;
        if (player == null)
        {
            Debug.LogError("ProfileSelectMenuHandler InstantiateProfileMenu(): player was null!");
            return;
        }
        //create profile menu
        ProfileSelectorMenu selector = Instantiate(profileSelectorPrefab, this.transform);
        selector.Initialize(player);
        currentProfileSelectors.Add(player, selector);
        //register to profile menu events
        selector.onLeaveSelected += HandlePlayerLeave;
        selector.onConfirmSelected += HandlePlayerHasConfirmed;
        selector.onConfirmCanceled += HandlePlayerHasCanceledConfirm;

        //reset continue text, as a new player has joined and so not all have confirmed
        Debug.Log($"numconfirmedplayers: {numConfirmedPlayers} out of {currentProfileSelectors.Count}");
        continueTextAnim.SetBool("IsHovered", false);
        continueText.color = ColorPallete.inactiveColor;
    }


    private void HandlePlayerLeave(Player player)
    {
        if (currentProfileSelectors.ContainsKey(player))
        {
            if (player == PlayerManager.Instance.PlayerOne)
            {
                foreach (var profileMenu in currentProfileSelectors.Values)
                    Destroy(profileMenu.gameObject);
                GameManager.Instance.ResetGameToMainMenu();
            }
            else
            {
                RemoveProfileMenu(player);
                PlayerManager.Instance.RemovePlayerFromGame(player.PlayerNumber);
            }
        }
        else
        {
            Debug.LogError($"ProfileSelectMenuHandler: tried to remove a nonexistent profile select menu for player: {player.name}");
        }
    }

    private void HandlePlayerHasConfirmed(Player player)
    {
        Debug.Log($"Player {player.PlayerNumber} has confirmed");
        numConfirmedPlayers++;
        Debug.Log($"numconfirmedplayers: {numConfirmedPlayers} out of {currentProfileSelectors.Count}");

        if (allPlayersHaveConfirmed)
        {
            continueTextAnim.SetBool("IsHovered", true);
            continueText.color = ColorPallete.selectedColor;
        }
    }

    private void HandlePlayerHasCanceledConfirm(Player player)
    {
        Debug.Log($"Player {player.PlayerNumber} has canceled their confirmation");
        numConfirmedPlayers--; 
        Debug.Log($"numconfirmedplayers: {numConfirmedPlayers} out of {currentProfileSelectors.Count}");

        continueTextAnim.SetBool("IsHovered", false);
        continueText.color = ColorPallete.inactiveColor;
    }

    private void RemoveProfileMenu(Player player)
    {
        var profileMenu = currentProfileSelectors[player];

        if (profileMenu.HasConfirmed)
        {
            numConfirmedPlayers--;
            Debug.Log($"numconfirmedplayers: {numConfirmedPlayers} out of {currentProfileSelectors.Count}");
        }

        currentProfileSelectors.Remove(player);
        Destroy(profileMenu.gameObject);
    }

    private void HandleAllPlayersConfirmed()
    {
        if (playerOne.Controller.InteractDown || playerOne.Controller.StartDown)
        {
            AssignAllPlayerAccounts();
            GameManager.Instance.GoToMostRecentScene();
        }
    }

    private void AssignAllPlayerAccounts()
    {
        foreach (var kvp in currentProfileSelectors)
        {
            PlayerManager.Instance.AssignPlayerAProfile(kvp.Key, kvp.Value.profileChosen);
        }
    }
}
