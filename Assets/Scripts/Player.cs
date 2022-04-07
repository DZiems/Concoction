using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1;
    [SerializeField] private Vector3 desiredSpawnPoint = new Vector3(2f, 2f, 0f);

    public Controller Controller { get; private set; }

    public bool HasController => Controller != null;
    public int PlayerNumber => playerNumber;

    //TODO: bring up account select panel
    public void InitializePlayer(Controller controller)
    {
        Controller = controller;
        //instantiate character account select panel
        //BringUpAccountSelectPanel();
        //move SpawnCharacter into an event that is called when AccountSelectPanel chooses a character.

        gameObject.name = $"Player {PlayerNumber} -- {Controller.gameObject.name}";

        CharacterInstantiator.Instance.SpawnCharacter($"Player {PlayerNumber}", Controller, PlayerNumber, desiredSpawnPoint);
    }


}
