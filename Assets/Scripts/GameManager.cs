using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public const string SceneMainMenu = "MainMenu";
    public const string SceneProfileSelectMenu = "ProfileSelectMenu";
    public const string SceneHomeBase = "HomeBase";
    public const string SceneAdventure = "Adventure";
    public string CurrentScene { get; private set; }
    public string MostRecentScene { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one GameManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentScene = SceneMainMenu;
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Start()
    {
        Debug.Log($"Scene name: {SceneManager.GetActiveScene().name}");
        switch (SceneManager.GetActiveScene().name)
        {
            case SceneHomeBase:
            case SceneAdventure:
                RegisterForPlayerJoins_SpawnPlayer();
                break;
            default:
                break;
        }
    }

    private void RegisterForPlayerJoins_SpawnPlayer()
    {
        Debug.Log("Note: Registering for Player Join events from GameManager. This means game was started from somewhere other than main menu.");
        PlayerManager.Instance.onPlayerOneJoined += () =>
        {
            Debug.Log("onPlayerJoin was registered to by the GameManager. This will spawn a character.");
            if (PlayerManager.Instance.PlayerOne != null)
            {
                var playerOne = PlayerManager.Instance.PlayerOne;
                if (!playerOne.HasProfile)
                {
                    playerOne.AssignProfile(new PlayerCharacterData());
                }
                playerOne.SpawnCharacter();
            }
        };
        PlayerManager.Instance.onPlayerTwoToFourJoined += (Player player) =>
        {
            Debug.Log("onPlayerJoin was registered to by the GameManager. Spawning a character.");
            if (!player.HasProfile)
            {
                player.AssignProfile(new PlayerCharacterData());
            }
            player.SpawnCharacter();
        };
    }

    public void LoadData(GameData data)
    {
        MostRecentScene = data.MostRecentScene;
    }

    public void SaveData(GameData data)
    {
        data.MostRecentScene = CurrentScene;
    }

    public void GoToMostRecentScene()
    {
        if (IsMostRecentSceneAMenu())
            RunLoadSceneAsync(SceneHomeBase);
        else
            RunLoadSceneAsync(MostRecentScene);
    }

    public void ResetGameToMainMenu()
    {
        PlayerManager.Instance.RemoveAllPlayers();
        RunLoadSceneAsync(SceneMainMenu);
    }

    public void RunLoadSceneAsync(string scene)
    {
        CurrentScene = scene;
        SceneManager.LoadSceneAsync(CurrentScene);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        DataPersistenceManager.Instance.OnSceneLoaded();

        if (ShouldSpawnPlayers())
            PlayerManager.Instance.SpawnAllActivePlayers();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded called");
        DataPersistenceManager.Instance.SaveGame();
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game");
        Application.Quit();
    }

    private bool ShouldSpawnPlayers()
    {
        return CurrentScene != SceneMainMenu && CurrentScene != SceneProfileSelectMenu;
    }
    private bool IsMostRecentSceneAMenu()
    {
        return MostRecentScene == SceneMainMenu || MostRecentScene == SceneProfileSelectMenu;
    }

   
}
