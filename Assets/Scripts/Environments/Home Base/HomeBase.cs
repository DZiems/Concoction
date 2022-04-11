using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HomeBase : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 6;
    [SerializeField] private float cellSize = 1f;

    //make a dictionary from these
    [SerializeField] private Tile[] tilePrefabs;
    [SerializeField] private Station[] stationPrefabs;

    private Dictionary<string, Tile> tilesDictionary;
    private Dictionary<string, Station> stationsDictionary;

    private Station[] stations;

    private FloorTilemap floorTilemap;
    private WallTilemap wallTilemap;
    private bool isBuilt;

    public int Width => width;
    public int Height => height;
    public Vector3 Origin => origin;
    public float CellSize => cellSize;
    public Station[] Stations => stations;


    private void Awake()
    {
        isBuilt = false;
        floorTilemap = GetComponentInChildren<FloorTilemap>();
        wallTilemap = GetComponentInChildren<WallTilemap>();
        InitializeDictionaries();
    }
    private void InitializeDictionaries()
    {
        stationsDictionary = new Dictionary<string, Station>();
        tilesDictionary = new Dictionary<string, Tile>();
        foreach (var station in stationPrefabs)
        {
            var stationName = station.gameObject.name;
            if (!String.IsNullOrEmpty(stationName))
                stationsDictionary.Add(stationName, station);
            else
                Debug.LogError($"HomeBase stationPrefabs has an unnamed station: {stationName}");
        }
        foreach (var tile in tilePrefabs)
        {
            var tileName = tile.gameObject.name;
            if (!String.IsNullOrEmpty(tileName))
                tilesDictionary.Add(tileName, tile);
            else
                Debug.LogError($"HomeBase tilePrefabs has an unnamed tile: {tileName}");
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
    }

    public void SaveData(GameData data)
    {
        data.HomeBaseData = new HomeBaseData(this);
    }


    private void Build(StationData[] stationData)
    {
        BuildStations(stationData);
        bool showDebug = false;
        BuildFloors(showDebug);
        BuildWalls(showDebug);

        isBuilt = true;
    }

    private void BuildWalls(bool showDebug)
    {
        wallTilemap.BuildRect(width + 2, height + 2, cellSize, origin - new Vector3(cellSize, cellSize), showDebug,
                    tilesDictionary["HomeBase_Wall_Stone"],
                    tilesDictionary["HomeBase_WallCorner_Stone"]);
    }

    private void BuildFloors(bool showDebug)
    {
        floorTilemap.BuildRect(width, height, cellSize, origin, showDebug,
                    tilesDictionary["HomeBase_Floor_Wood"]);
    }

    private void BuildStations(StationData[] stationData)
    {
        stations = new Station[stationData.Length];
        int i = 0;
        foreach (var stationDatum in stationData)
        {
            if (stationsDictionary.ContainsKey(stationDatum.id))
            {
                if (!stationDatum.isUnlocked)
                    Debug.Log($"{stationDatum.id} not unlocked.");
                if (!stationDatum.isPlaced)
                    Debug.Log($"{stationDatum.id} not placed.");

                stations[i] = Instantiate(stationsDictionary[stationDatum.id], this.transform);
                stations[i].transform.Rotate(new Vector3(stationDatum.rotationEulers[0], stationDatum.rotationEulers[1], stationDatum.rotationEulers[2]));

                stations[i].transform.position = GetWorldPosition(stationDatum.tiledPosition[0], stationDatum.tiledPosition[1]);
                i++;
            }
        }

    }

    private void Update()
    {
        if (isBuilt)
        {
            foreach (var station in stations)
            {
                if (station.isWithinWalls(Width, Height, GetTiledXY(station.transform.position)))
                    station.SpriteRenderer.color = Color.white;
                else
                    station.SpriteRenderer.color = new Color(1, 0.5f, 0.5f);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    public Tuple<int, int> GetTiledXY(Vector3 worldPosition)
    {
        var x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        var y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
        return new Tuple<int, int>(x, y);
    }

   
}