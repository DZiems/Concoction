using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//TODO: implement confirmation and scene transition

public class ProfileMenuHandler : MonoBehaviour
{
    [SerializeField] private UIHoverableItem continueButton;
    [SerializeField] private ProfileMenu profileMenu;

    private Player player;

    private bool confirmFlag;

    private void Awake()
    {
        confirmFlag = false;
    }

    private void Start()
    {
        player = PlayerManager.Instance.Player;
        continueButton.Deactivate();
    }


    private void Update()
    {
        if (profileMenu.HasConfirmed)
        {
            if (!confirmFlag)
            {
                continueButton.Hover();
                confirmFlag = true;
            }
            //waits one frame until flag is up, that way when the player presses confirm on the profile menu, it doesn't also press this continue button in the same frame 
            else
            {
                HandleContinueButton();
            }
        }
        else
        {
            if (confirmFlag)
            {
                continueButton.Deactivate();
                confirmFlag = false;
            }
        }
    }

    private void HandleContinueButton()
    {
        if (player.Controller.InteractPress || player.Controller.StartPress)
        {
            if (profileMenu.CurrentSelectedProfile != null)
            {
                PlayerManager.Instance.AssignPlayerAProfile(profileMenu.CurrentSelectedProfile);
                SceneManager.Instance.RunLoadSceneAsync(SceneManager.SceneHomeBase);
            }
            else
            {
                Debug.LogError("Tried to assign player a profile when the ProfileMenu returned null for CurrentSelectedProfile.");
            }
        }
    }

}
