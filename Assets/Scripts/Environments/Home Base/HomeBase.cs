using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HomeBase : Room, IDataPersistence
{
    [SerializeField] private Station[] stationPrefabs;

    public Station[] Stations { get; private set; }
    private Dictionary<string, Station> stationsDictionary;

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
        stationsDictionary = new Dictionary<string, Station>();
        foreach (var station in stationPrefabs)
        {
            var stationName = station.gameObject.name;
            if (!string.IsNullOrEmpty(stationName))
                stationsDictionary.Add(stationName, station);
            else
                Debug.LogError($"HomeBase stationPrefabs has an unnamed station: {stationName}");
        }
    }

    //IDataPersistence
    public void LoadData(GameData data)
    {
        width = data.HomeBaseData.width;
        height = data.HomeBaseData.height;
        cellSize = data.HomeBaseData.cellSize;
        origin = new Vector3(data.HomeBaseData.origin[0], data.HomeBaseData.origin[1]);

        Build(data.HomeBaseData.stationData);

        isLoaded = true;
    }

    public void SaveData(GameData data)
    {
        data.HomeBaseData = new HomeBaseData(this);
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