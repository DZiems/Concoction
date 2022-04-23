using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerNumber { get; set; }
    public Controller Controller { get; private set; }
    public bool HasController => Controller != null;
    public bool HasProfile => PlayerCharacter != null;

    public Character PlayerCharacter { get; private set; }
    private AimReticle playerAimReticle;
    private bool IsCharacterSpawned => PlayerCharacter != null && GameObject.Find(PlayerCharacter.gameObject.name) != null;


    public event Action onHasLeftGame;

    public void AssignController(Controller controller)
    {
        Controller = controller;
        gameObject.name += $" -- {Controller.gameObject.name}";
    }

    public void AssignProfile(Character profileCharacter)
    {
        PlayerCharacter = profileCharacter;
    }

    public void SpawnCharacter()
    {
        if (IsCharacterSpawned)
        {
            Debug.LogError($"Attempted to spawn a duplicate character for player {PlayerNumber}");
            return;
        }
        if (!HasController)
        {
            Debug.LogError($"Attempted to spawn a character before player has been assigned a controller (player {PlayerNumber})");
            return;
        }
        if (!HasProfile)
        {
            Debug.LogError($"Attempted to spawn a character before player has been assigned a profile (player {PlayerNumber})");
            return;
        }

        Instantiate(PlayerCharacter);
        PlayerCharacter.SetController(Controller);
        PlayerCharacter.gameObject.name = $"(P{PlayerNumber}: {PlayerCharacter.ProfileName}) Character";

        Instantiate(playerAimReticle);
        playerAimReticle.Initialize(Controller, PlayerCharacter.transform, PlayerNumber);
    }

    public void RemoveFromGame()
    {
        Debug.Log($"Player {PlayerNumber} RemoveFromGame() called");
        ControllerManager.Instance.UnassignController(Controller.Id);
        Controller = null;

        PlayerCharacter = null;
        gameObject.name = $"Player {PlayerNumber}";

        //todo: register to this from profile select menu
        if (onHasLeftGame != null)
            onHasLeftGame();
    }
}
