using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public const string SceneMainMenu = "MainMenu";
    public const string SceneProfileMenu = "ProfileMenu";
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

        CurrentScene = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData(GameData data)
    {
        if (this == null) return;

    }

    public void SaveData(GameData data)
    {
        if (this == null) return;

        if (IsCurrentSceneAMenu()) return;

        data.CurrentPlayerProfileData.mostRecentScene = CurrentScene;
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
        switch (SceneManager.GetActiveScene().name)
        {
            case SceneHomeBase:
            case SceneAdventure:
                RegisterForPlayerJoin_SpawnPlayer();
                break;
            default:
                break;
        }
    }

    private void RegisterForPlayerJoin_SpawnPlayer()
    {
        Debug.Log("Note: Registering for Player Join events from GameManager. This means game was started from somewhere other than main menu.");
        PlayerManager.Instance.onPlayerJoined += () =>
        {
            var player = PlayerManager.Instance.Player;
            if (!PlayerManager.Instance.Player.HasProfile)
            {
                player.AssignProfile(new PlayerProfileData("Temporary Debug"));
            }
            player.SpawnCharacter();
        };
    }

    

    public void GoToMostRecentScene()
    {
        if (string.IsNullOrEmpty(MostRecentScene))
            RunLoadSceneAsync(SceneHomeBase);
        else
            RunLoadSceneAsync(MostRecentScene);
    }

    public void ResetGameToMainMenu()
    {
        PlayerManager.Instance.RemovePlayerFromGame();
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

        if (!IsCurrentSceneAMenu())
            PlayerManager.Instance.SpawnPlayer();
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

    private bool IsCurrentSceneAMenu()
    {
        return CurrentScene == SceneMainMenu || CurrentScene == SceneProfileMenu;
    }

   
}
