using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    public const string SceneMainMenu = "MainMenu";
    public const string SceneProfileMenu = "ProfileMenu";
    public const string SceneHomeBase = "HomeBase";
    public const string SceneAdventure = "Adventure";
    public string CurrentScene { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one GameManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject);
    }


    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Start()
    {
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
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
            Debug.Log("Calling SpawnCharacterForDebugging().");
            var player = PlayerManager.Instance.Player;
            player.SpawnCharacterForDebugging();
        };
    }


    public void ResetGameToMainMenu()
    {
        PlayerManager.Instance.RemovePlayerFromGame();
        RunLoadSceneAsync(SceneMainMenu);
    }


    public void RunLoadSceneAsync(string scene)
    {
        CurrentScene = scene;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(CurrentScene);
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        DataPersistenceManager.Instance.OnSceneLoaded();

        if (!IsCurrentSceneAMenu())
        {
            if (PlayerManager.Instance.Player.HasProfile)
                PlayerManager.Instance.SpawnPlayer();
            else
                Debug.LogWarning("GameManager OnSceneLoaded wants to spawn a player, but there's no profile (fine if testing from somewhere other than main menu). Returning without spawning, but player should still spawn from another process once there's a controller.");
        }
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

    public bool IsCurrentSceneAMenu()
    {
        return CurrentScene == SceneMainMenu || CurrentScene == SceneProfileMenu;
    }

   
}
