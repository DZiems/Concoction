using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataPersistence
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerCharacter baseCharacterPrefab;
    [SerializeField] private AimReticle baseAimReticlePrefab;

    public Player Player { get; private set; }
    public event Action onPlayerJoined;

    public PlayerCharacter BaseCharacterPrefab => baseCharacterPrefab;
    public AimReticle BaseAimReticlePrefab => baseAimReticlePrefab;

    private bool isLoaded;

    public Dictionary<string, PlayerProfileData> allPlayerProfileDatas { get; private set; }

    public void LoadData(GameData data)
    {
        if (allPlayerProfileDatas != null)
            allPlayerProfileDatas.Clear();
        else
            allPlayerProfileDatas = new Dictionary<string, PlayerProfileData>();


        foreach (var kvp in data.allPlayerProfileDatas)
            allPlayerProfileDatas.Add(kvp.Key, kvp.Value);

        isLoaded = true;
    }

    public void SaveData(GameData data)
    {
        //for each existing key, just overwrite the data; for all new keys, add.
        foreach (var kvp in allPlayerProfileDatas)
        {
            if (!data.allPlayerProfileDatas.ContainsKey(kvp.Key))
                data.allPlayerProfileDatas.Add(kvp.Key, kvp.Value);
            else 
                data.allPlayerProfileDatas[kvp.Key] = kvp.Value;
        }
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
        DontDestroyOnLoad(Instance);

        isLoaded = false;
        Player = GetComponentInChildren<Player>();
    }


    internal void AssignController(Controller controller)
    {
        if (!isLoaded)
        {
            Debug.LogError("PlayerManager AssignControllerAndPromptProfile() could not be called because save data isn't yet loaded");
            return;

        }
        
        Player.AssignController(controller);
        onPlayerJoined?.Invoke();
    }


    public void RemovePlayerFromGame()
    {
        Player.RemoveFromGame();
    }


    public void SpawnPlayer()
    {
        Player.SpawnCharacter();
    }



    public void AssignPlayerAProfile(string profileName)
    {
        if (allPlayerProfileDatas.ContainsKey(profileName))
        {
            Debug.Log("ASSIGNINGPLAYERPROFILE");
            Player.AssignProfile(allPlayerProfileDatas[profileName]);
        }
        else
        {
            Debug.LogError($"Attempted to assign player a nonexistent profile: {profileName}");
        }
    }

}
