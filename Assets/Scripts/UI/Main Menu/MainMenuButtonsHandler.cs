using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//TODO: Make this menu spawn an account selection when a player registers to a controller

//controller grabbed on first player
public class MainMenuButtonsHandler : MonoBehaviour
{
    private Controller controller;
    
    [SerializeField] private UIHoverableText[] buttons;
    private const int playButtonInd = 0;
    private const int settingsButtonInd = 1;
    private const int quitButtonInd = 2;

    private int buttonsInd;



    private bool isMenuActive;

    private void Awake()
    {

        buttonsInd = playButtonInd;
    }



    private void Start()
    {
        SetMenuInactive();

        PlayerManager.Instance.onPlayerJoined += Initialize;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onPlayerJoined -= Initialize;
    }

    private void SetMenuInactive()
    {
        foreach (var button in buttons)
        {
            button.Deactivate();
        }
        isMenuActive = false;
    }

    private void Initialize()
    {
        controller = PlayerManager.Instance.Player.Controller;

        SetMenuActive();
    }

    private void SetMenuActive()
    {
        foreach (var button in buttons)
        {
            button.Unhover();
        }

        buttons[buttonsInd].Hover();

        StartCoroutine(AllowPlayerInteraction());
    }

    private IEnumerator AllowPlayerInteraction()
    {
        yield return null;

        isMenuActive = true;
    }


    void Update()
    {
        if (isMenuActive)
        {
            HandleMove();
            HandleSelectButton();
        }

    }

    private void HandleMove()
    {
        if (controller.VerticalUpPress)
        {
            buttons[buttonsInd].Unhover();

            buttonsInd--;
            if (buttonsInd < 0)
                buttonsInd = buttons.Length - 1;

            buttons[buttonsInd].Hover();
        }
        else if (controller.VerticalDownPress)
        {
            buttons[buttonsInd].Unhover();

            buttonsInd++;
            if (buttonsInd >= buttons.Length)
                buttonsInd = 0;

            buttons[buttonsInd].Hover();
        }
    }

    private void HandleSelectButton()
    {
        if (controller.InteractPress)
        {
            switch (buttonsInd)
            {
                case playButtonInd:
                    SceneManager.Instance.RunLoadSceneAsync(SceneManager.SceneProfileMenu);
                    SetMenuInactive();
                    break;
                case settingsButtonInd:
                    Debug.Log("Settings selected");
                    break;
                case quitButtonInd:
                    SceneManager.Instance.QuitGame();
                    break;
                default:
                    return;
            }
        }
    }


}
