using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//TODO: Make this menu spawn an account selection when a player registers to a controller

//controller grabbed on first player
public class MainMenuHandler : MonoBehaviour
{
    private Player player;

    //assigned to buttons via SerializeField in Class MainMenuButton.cs
    public enum Option
    {
        Play,
        Settings,
        Quit
    }

    
    private MainMenuButton[] buttons;
    private TextMeshProUGUI[] buttonTexts;
    private Animator[] buttonAnims;
    private int buttonsInd;

    private bool isActivated;

    private void Awake()
    {
        buttons = GetComponentsInChildren<MainMenuButton>();
        buttonTexts = new TextMeshProUGUI[buttons.Length];
        buttonAnims = new Animator[buttons.Length];
        int i = 0;
        foreach (var button in buttons)
        {
            buttonTexts[i] = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonAnims[i++] = button.GetComponent<Animator>();
        }

        DeactivateButtons();

        buttonsInd = 0;
    }

    private void DeactivateButtons()
    {
        int i = 0;
        foreach (var button in buttons)
        {
            buttonTexts[i].color = ColorPallete.inactiveColor;
            buttonAnims[i++].SetBool("IsHovered", false);
        }
        isActivated = false;
    }

    private void Start()
    {
        PlayerManager.Instance.onPlayerOneJoined += Initialize;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onPlayerOneJoined -= Initialize;
    }

    private void Initialize()
    {
        this.player = PlayerManager.Instance.PlayerOne;

        ActivateButtons();

        StartCoroutine(AllowPlayerInteraction());
    }

    private void ActivateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
            HoverButton(i, false);

        HoverButton(buttonsInd, true);
    }

    private IEnumerator AllowPlayerInteraction()
    {
        yield return null;
        isActivated = true;
        Debug.Log("Main Menu Initialized");
    }


    void Update()
    {
        //wait for first assigned player
        if (isActivated)
        {
            HandleMove();
            HandleSelectButton();
        }

    }


    private void HoverButton(int buttonInd, bool on)
    {
        buttonAnims[buttonInd].SetBool("IsHovered", on);

        if (on) 
            buttonTexts[buttonInd].color = ColorPallete.selectedColor;
        else 
            buttonTexts[buttonInd].color = ColorPallete.unselectedColor;
    }

    private void HandleMove()
    {
        if (player.Controller.VerticalDown > 0)
        {
            HoverButton(buttonsInd, false);

            buttonsInd--;
            if (buttonsInd < 0)
                buttonsInd = buttons.Length - 1;

            HoverButton(buttonsInd, true);
        }
        else if (player.Controller.VerticalDown < 0)
        {
            HoverButton(buttonsInd, false);

            buttonsInd++;
            if (buttonsInd >= buttons.Length)
                buttonsInd = 0;

            HoverButton(buttonsInd, true);
        }
    }

    private void HandleSelectButton()
    {
        if (player.Controller.InteractDown)
        {
            switch (buttons[buttonsInd].Id)
            {
                case Option.Play:
                    GameManager.Instance.RunLoadSceneAsync(GameManager.SceneProfileSelectMenu);
                    DeactivateButtons();
                    break;
                case Option.Settings:
                    Debug.Log("Settings selected");
                    break;
                case Option.Quit:
                    GameManager.Instance.QuitGame();
                    break;
                default:
                    return;
            }
        }
    }


}
