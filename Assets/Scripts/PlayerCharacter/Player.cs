using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerNumber { get; set; }
    public bool HasController => Controller != null;
    public bool HasProfile => ProfileData != null;
    public bool IsCharacterSpawned => Character != null;

    public PlayerCharacterData ProfileData { get; private set; }
    public Controller Controller { get; private set; }
    public Character Character { get; private set; }

    private AimReticle aimReticle;



    public event Action onHasLeftGame;


    public void AssignController(Controller controller)
    {
        Controller = controller;
        gameObject.name += $" -- {Controller.gameObject.name}";
    }

    public void AssignProfile(PlayerCharacterData profileData)
    {
        ProfileData = profileData;
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

        Character = Instantiate(PlayerManager.Instance.BaseCharacterPrefab);
        Character.Initialize(this);

        aimReticle = Instantiate(PlayerManager.Instance.BaseAimReticlePrefab);
        aimReticle.Initialize(this);

    }
    public void SpawnCharacter(Vector2 position)
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

        Character = Instantiate(PlayerManager.Instance.BaseCharacterPrefab, position, Quaternion.identity);
        Character.Initialize(this);

        aimReticle = Instantiate(PlayerManager.Instance.BaseAimReticlePrefab);
        aimReticle.Initialize(this);

    }

    public void RemoveFromGame()
    {
        Debug.Log($"Player {PlayerNumber} RemoveFromGame() called");
        ControllerManager.Instance.UnassignController(Controller.Id);

        Controller = null;

        RemoveCharacter();

        gameObject.name = $"Player {PlayerNumber}";

        //todo: register to this from profile select menu
        if (onHasLeftGame != null)
            onHasLeftGame();
    }

    private void RemoveCharacter()
    {
        if (IsCharacterSpawned)
        {
            GameObject.Destroy(Character);
            GameObject.Destroy(aimReticle);
        }

        Character = null;
        aimReticle = null;
    }

   
}
