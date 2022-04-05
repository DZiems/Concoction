using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private Player[] _players;

    private void Awake()
    {
        Instance = this;
        _players = FindObjectsOfType<Player>();
    }

    //find the next player (by number) to add
    //call their initialize function
    internal void AddPlayerToGame(Controller controller)
    {
        Player playerToAdd = FindNextUnassignedPlayer();
        if (playerToAdd != null)
            playerToAdd.InitializePlayer(controller);
    }

    private Player FindNextUnassignedPlayer()
    {
        Player player = null;
        int lowestPlayerNumber = 5;
        foreach (var p in _players)
        {
            if (!p.HasController)
                if (p.PlayerNumber < lowestPlayerNumber)
                {
                    lowestPlayerNumber = p.PlayerNumber;
                    player = p;
                }
        }

        return player;
    }
}
