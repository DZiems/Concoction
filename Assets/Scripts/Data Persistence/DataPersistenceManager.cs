using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    //might eventually need to be multiple file data handlers if storing player and homebase etc. in different files. Or otherwise find a way to check for individually deserialized objects and construct their defaults.
    FileDataHandler fileDataHandler;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one DataPersistenceManager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        this.gameData = fileDataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        //Push loaded save data to all other scripts that need it
        foreach (IDataPersistence dpObj in dataPersistenceObjects)
        {
            dpObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //Pass data to other scripts so they can update it
        foreach (IDataPersistence dpObj in dataPersistenceObjects)
        {
            dpObj.SaveData(gameData);
        }
        //Save that data to a file using the data handler
        fileDataHandler.Save(gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
