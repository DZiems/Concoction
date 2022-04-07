using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//station placement sensitivity:
// - id has to be the same string from loaded file as the home base stationPrefabs[i] gameObject.name
public struct StationPlacement
{
    public string Id;
    public Tuple<int, int> Position;
    public TileRotateTool.Dir Rotation;

    public StationPlacement(string id, Tuple<int, int> position, TileRotateTool.Dir rotation)
    {
        Id = id;
        Position = position;
        Rotation = rotation;
    }
}

public class HomeBase : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 6;
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private int doorLength = 2;
    [SerializeField] private WallTilemap.Side entranceSide = WallTilemap.Side.Left;

    //make a dictionary from these
    [SerializeField] private Tile[] tilePrefabs;
    private Dictionary<string, Tile> tilesDictionary;

    [SerializeField] private Station[] stationPrefabs;
    private Dictionary<string, Station> stationsDictionary;
    private StationPlacement[] stationPlacements;


    private FloorTilemap floorTilemap;
    private WallTilemap wallTilemap;


    private void Awake()
    {
        InitializeTilesDict();
        InitializeStationsDict();


        LoadStationPlacements();

        floorTilemap = GetComponentInChildren<FloorTilemap>();
        wallTilemap = GetComponentInChildren<WallTilemap>();
    }

    //TODO: load from a file instead of hardcoded
    private void LoadStationPlacements()
    {
        stationPlacements = new StationPlacement[stationsDictionary.Count];
        int i = 0;
        foreach (var stationEntry in stationsDictionary)
            stationPlacements[i++] = new StationPlacement(stationEntry.Key, new Tuple<int, int>(i + 2, i + 2), TileRotateTool.Dir.Top);
    }

    private void InitializeStationsDict()
    {
        stationsDictionary = new Dictionary<string, Station>();
        foreach (var station in stationPrefabs)
        {
            var stationName = station.gameObject.name;
            if (!String.IsNullOrEmpty(stationName))
                stationsDictionary.Add(stationName, station);
            else
                Debug.LogError($"HomeBase tiles array has an unnamed tile: {stationName}");
        }

        foreach (var kvp in stationsDictionary)
        {
            Debug.Log($"{kvp.Key} : {kvp.Value}");
        }
    }

    private void InitializeTilesDict()
    {
        tilesDictionary = new Dictionary<string, Tile>();
        foreach (var tile in tilePrefabs)
        {
            var tileName = tile.gameObject.name;
            if (!String.IsNullOrEmpty(tileName))
                tilesDictionary.Add(tileName, tile);
            else
                Debug.LogError($"HomeBase tiles array has an unnamed tile: {tileName}");
        }

        foreach (var kvp in tilesDictionary)
        {
            Debug.Log($"{kvp.Key} : {kvp.Value}");
        }
    }


    private void PlaceStations()
    {
        foreach (var placement in stationPlacements)
        {
            if (stationsDictionary.ContainsKey(placement.Id))
            {
                var station = Instantiate(stationsDictionary[placement.Id], this.transform);
                station.transform.Rotate(TileRotateTool.GetRotation(placement.Rotation));
                station.transform.position = GetWorldPosition(placement.Position.Item1, placement.Position.Item2);
            }
        }
    }


    private void Start()
    {
        bool showDebug = false;

        floorTilemap.BuildRect(width, height, cellSize, origin, showDebug, 
            tilesDictionary["HomeBase_Floor_Wood"]);

        wallTilemap.BuildRect(width + 2, height + 2, cellSize, origin - new Vector3(cellSize, cellSize), showDebug, 
            tilesDictionary["HomeBase_Wall_Stone"], 
            tilesDictionary["HomeBase_WallCorner_Stone"], 
            entranceSide, doorLength);

        PlaceStations();
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    private Tuple<int, int> GetXY(Vector3 worldPosition)
    {
        var x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        var y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
        return new Tuple<int, int>(x, y);
    }
}
