using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1;

    public Controller Controller { get; private set; }

    public bool HasController => Controller != null;
    public int PlayerNumber => playerNumber;

    private CharacterInstantiator characterInstantiator;
    private void Awake()
    {
        characterInstantiator = GetComponent<CharacterInstantiator>();
    }

    public void InitializePlayer(Controller controller)
    {
        Controller = controller;
        //instantiate character account select panel
        //BringUpAccountSelectPanel();
        //move SpawnCharacter into an event that is called when AccountSelectPanel chooses a character.

        gameObject.name = $"Player {PlayerNumber} -- {Controller.gameObject.name}";

        characterInstantiator.SpawnCharacter($"Player {PlayerNumber}", Controller, PlayerNumber);
    }


}
