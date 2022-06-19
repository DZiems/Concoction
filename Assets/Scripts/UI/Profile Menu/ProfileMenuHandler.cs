using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//TODO: implement confirmation and scene transition

public class ProfileMenuHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private ProfileMenu profileMenu;


    private Animator continueTextAnim;

    private Player player;

    private bool confirmFlag;

    private void Awake()
    {
        continueTextAnim = continueText.GetComponent<Animator>();

        confirmFlag = false;
    }

    private void Start()
    {
        player = PlayerManager.Instance.Player;
        DeactivateContinueText();
    }


    private void Update()
    {
        if (profileMenu.HasConfirmed)
        {
            if (!confirmFlag)
            {
                ActivateContinueText();
                confirmFlag = true;
            }
            //waits one frame until flag is up, that way when the player presses confirm, it doesn't also press continue with the same controller.InteractPress
            else
            {
                HandleContinueButton();
            }
        }
        else
        {
            if (confirmFlag)
            {
                DeactivateContinueText();
                confirmFlag = false;
            }
        }
    }
    private void DeactivateContinueText()
    {
        continueTextAnim.SetBool("IsHovered", false);
        continueText.color = ColorPallete.inactiveColor;
    }

    private void ActivateContinueText()
    {
        continueTextAnim.SetBool("IsHovered", true);
        continueText.color = ColorPallete.selectedColor;
    }


    private void HandleContinueButton()
    {
        if (player.Controller.InteractPress || player.Controller.StartPress)
        {
            if (profileMenu.CurrentSelectedProfile != null)
            {
                PlayerManager.Instance.AssignPlayerAProfile(profileMenu.CurrentSelectedProfile);
                GameManager.Instance.GoToMostRecentScene();
            }
            else
            {
                Debug.LogError("Tried to assign player a profile when the ProfileMenu returned null for CurrentSelectedProfile.");
            }
        }
    }

}
