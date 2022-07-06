using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerCharacter baseCharacterPrefab;
    [SerializeField] private AimReticle baseAimReticlePrefab;

    public Player Player { get; private set; }
    public event Action onPlayerJoined;

    public PlayerCharacter BaseCharacterPrefab => baseCharacterPrefab;
    public AimReticle BaseAimReticlePrefab => baseAimReticlePrefab;


    
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

        Player = GetComponentInChildren<Player>();
    }


    internal void AssignController(Controller controller)
    {
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
        var currentProfileDatas = DataPersistenceManager.Instance.AllProfileDatas;
        if (currentProfileDatas.ContainsKey(profileName))
        {
            Player.AssignProfile(currentProfileDatas[profileName]);
        }
        else
        {
            Debug.LogError($"Attempted to assign player a nonexistent profile: {profileName}");
        }
    }

}
