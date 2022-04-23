using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataPersistence
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private Character characterPrefab;
    [SerializeField] private AimReticle aimReticlePrefab;

    public event Action onPlayerOneJoined;
    public event Action<Player> onPlayerTwoToFourJoined;

    public Character CharacterPrefab => characterPrefab;
    public AimReticle AimReticlePrefab => aimReticlePrefab;

    private bool isLoaded;

    private Dictionary<string, PlayerCharacterData> allPlayerCharacterData;
    private Player[] allPlayerObjects;
    //TODO: consider linked list? more efficient adding/removing?
    public List<Player> CurrentPlayers { get; private set; }
    //TODO: change to => CurrentPlayers[0], where you can guarantee this player won't be deleted
    public Player PlayerOne => CurrentPlayers.Count > 0 ? CurrentPlayers[0] : null;


    public void LoadData(GameData data)
    {
        allPlayerCharacterData = data.AllPlayerCharacterData;
        isLoaded = true;
    }

    public void SaveData(GameData data)
    {
        data.AllPlayerCharacterData = new SerializableDictionary<string, PlayerCharacterData>(allPlayerCharacterData);
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one PlayerManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        isLoaded = false;

        allPlayerObjects = GetComponentsInChildren<Player>();
        CurrentPlayers = new List<Player>();
        CurrentPlayers.Capacity = 4;    //TODO: MAX_NUM_PLAYERS

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        int index = 1;
        foreach (var player in allPlayerObjects)
        {
            player.PlayerNumber = index;
            index++;
            player.gameObject.name = $"Player {player.PlayerNumber}";
        }

    }

    //find the next player (by number) to add
    //call their initialize function
    internal void AssignController(Controller controller)
    {
        if (!isLoaded)
        {
            Debug.LogError("PlayerManager AssignControllerAndPromptProfile() could not be called because save data isn't loaded");
            return;

        }
        Player playerToAdd = FindNextUnassignedPlayer();
        if (playerToAdd != null)
        {
            playerToAdd.AssignController(controller);
            CurrentPlayers.Add(playerToAdd);

            if (playerToAdd.PlayerNumber == 1)
            {
                if (onPlayerOneJoined != null)
                    onPlayerOneJoined();
                else
                    Debug.LogError("PlayerManager onPlayerOneJoined is null");
            }

            else
            {
                if (onPlayerTwoToFourJoined != null)
                    onPlayerTwoToFourJoined(playerToAdd);
                else
                    Debug.LogWarning("PlayerManager onPlayerTwoToFourJoined is null");
            }

            Debug.Log($"PlayerManager: controller assigned to player {playerToAdd.PlayerNumber}");
        }
    }


    public void RemovePlayerFromGame(int playerNumber)
    {
        Debug.Log($"RemovePlayerFromGame called for playerNumber: {playerNumber}");
        int playerIndex = playerNumber - 1;
        if (playerNumber > 0 && playerIndex < CurrentPlayers.Count)
        {
            Debug.Log($"Removing player: {playerNumber}");
            var playerToRemove = CurrentPlayers[playerIndex];
            CurrentPlayers.RemoveAt(playerIndex);
            playerToRemove.RemoveFromGame();
        }
        
        for (int i = playerIndex; i < CurrentPlayers.Count; i++)
        {
            Debug.Log($"Setting player {CurrentPlayers[i].PlayerNumber} to player {i}");
            CurrentPlayers[i].PlayerNumber = i;
        }
    }

    public void RemoveAllPlayers()
    {
        Debug.Log("Removing All Players");

        foreach (Player player in CurrentPlayers)
        {
            Debug.Log($"Removing player: {player.PlayerNumber}");
            player.RemoveFromGame();
        }

        CurrentPlayers.Clear();
    }

    private Player FindNextUnassignedPlayer()
    {
        foreach (var p in allPlayerObjects)
        {
            if (!p.HasController)
                return p;
        }
        return null;

    }
    
    public void SpawnAllActivePlayers()
    {
        foreach (var player in CurrentPlayers)
        {
            if (!player.HasController) continue;

            player.SpawnCharacter();
        }
    }

    public Dictionary<string, PlayerCharacterData> GetAllPlayerCharacterData()
    {
        return allPlayerCharacterData;
    }


    public void AddNewPlayerProfile(string profileName)
    {
        Debug.Log($"PlayerManager -- Adding Profile: {profileName}");
        allPlayerCharacterData.Add(profileName, new PlayerCharacterData(name));
    }

    public void RemovePlayerProfile(string profileName)
    {
        if (allPlayerCharacterData.ContainsKey(profileName))
        {
            Debug.Log($"PlayerManager -- Removing Profile: {profileName}");
            allPlayerCharacterData.Remove(profileName);
        }
        else
            Debug.LogError($"Attempted to remove nonexistent profile {profileName}");
    }
}
