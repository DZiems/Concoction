using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HomeBase : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private int width = 12;
    [SerializeField] private int height = 6;
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private int doorLength = 2;
    [SerializeField] private WallBorderTilemap.Side doorSide = WallBorderTilemap.Side.Left;

    //hack a dictionary into the inspector as follows
    [SerializeField] private Tile[] tiles;


    private Dictionary<string, Tile> tilesDictionary;
    private FloorTilemap floorTilemap;
    private WallBorderTilemap wallTilemap;

    private void Awake()
    {
        tilesDictionary = new Dictionary<string, Tile>();
        foreach (var tile in tiles)
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

        floorTilemap = GetComponentInChildren<FloorTilemap>();
        wallTilemap = GetComponentInChildren<WallBorderTilemap>();
    }

    private void Start()
    {
        bool showDebug = false;

        floorTilemap.Build(width, height, cellSize, origin, showDebug, 
            tilesDictionary["HomeBase_Floor_Wood"]);

        wallTilemap.Build(width + 2, height + 2, cellSize, origin - new Vector3(cellSize, cellSize), showDebug, 
            tilesDictionary["HomeBase_Wall_Stone"], 
            tilesDictionary["HomeBase_WallCorner_Stone"], 
            doorSide, doorLength);


    }

}
