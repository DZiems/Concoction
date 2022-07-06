using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HomeBase : Room, IDataPersistence
{
    [SerializeField] private Station[] stationPrefabs;

    public Station[] Stations { get; private set; }
    private Dictionary<StationType, Station> stationsDictionary;

    private bool isLoaded;


    private void Awake()
    {
        isLoaded = false;
        floorTilemap = GetComponentInChildren<FloorTilemap>();
        wallTilemap = GetComponentInChildren<WallTilemap>();
        InitializeDictionaries();
    }
    private void InitializeDictionaries()
    {
        stationsDictionary = new Dictionary<StationType, Station>();
        foreach (var station in stationPrefabs)
            stationsDictionary.Add(station.Type, station);
    }

    //IDataPersistence
    public void LoadData(GameData data)
    {
        HomeBaseData homeBaseData;
        if (data.CurrentPlayerProfileData == null)
        {
            Debug.LogWarning("HomeBase is loading before a player has chosen a profile (fine if testing from somewhere other than main menu). Returning a default home base.");
            homeBaseData = new HomeBaseData();
        }
        else
        {
            homeBaseData = data.CurrentPlayerProfileData.homeBaseData;
        }

        width = homeBaseData.width;
        height = homeBaseData.height;
        cellSize = homeBaseData.cellSize;
        origin = new Vector3(homeBaseData.origin[0], homeBaseData.origin[1]);

        Debug.Log($"Home Base Width: {width}, Height: {height}");

        Build(homeBaseData.stationData);


        isLoaded = true;
    }

    public void SaveData(GameData data)
    {
        if (data.CurrentPlayerProfileData == null)
        {
            Debug.LogWarning("HomeBase is saving before a player has chosen a profile (fine if testing from somewhere other than main menu). Returning without saving.");
            return;
        }

        string playerProfileName = PlayerManager.Instance.Player.profileData.profileName;

        data.allPlayerProfileDatas[playerProfileName].homeBaseData = new HomeBaseData(this);
    }

    private void Build(StationData[] stationData)
    {
        BuildStations(stationData);

        base.BuildWallsAndFloors();
    }

    private void BuildStations(StationData[] stationData)
    {
        Stations = new Station[stationData.Length];
        int i = 0;
        foreach (var stationDatum in stationData)
        {
            if (stationsDictionary.ContainsKey(stationDatum.id))
            {
                if (!stationDatum.isUnlocked)
                    Debug.Log($"{stationDatum.id} not unlocked.");
                if (!stationDatum.isPlaced)
                    Debug.Log($"{stationDatum.id} not placed.");

                Stations[i] = Instantiate(stationsDictionary[stationDatum.id], this.transform);
                Stations[i].transform.Rotate(new Vector3(stationDatum.rotationEulers[0], stationDatum.rotationEulers[1], stationDatum.rotationEulers[2]));

                Stations[i].transform.position = GetWorldPosition(stationDatum.tiledPosition[0], stationDatum.tiledPosition[1]);
                i++;
            }
        }

    }

    private void Update()
    {
        if (isLoaded)
        {
            foreach (var station in Stations)
            {
                if (station.isWithinWalls(Width, Height, GetTiledXY(station.transform.position)))
                    station.SpriteRenderer.color = Color.white;
                else
                    station.SpriteRenderer.color = new Color(1, 0.5f, 0.5f);
            }
        }
    }

  
   
}