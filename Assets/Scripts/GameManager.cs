using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public static readonly string SceneMainMenu = "MainMenu";
    public static readonly string SceneProfileSelectMenu = "ProfileSelectMenu";
    public static readonly string SceneHomeBase = "HomeBase";
    public static readonly string SceneAdventure = "Adventure";
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
