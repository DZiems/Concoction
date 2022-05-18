using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] protected Vector3 origin;
    [SerializeField] protected int width = 12;
    [SerializeField] protected int height = 6;
    [SerializeField] protected float cellSize = 1f;

    //make a dictionary from these
    [SerializeField] protected Tile floorPrefab;
    [SerializeField] protected Tile wallPrefab;
    [SerializeField] protected Tile wallCornerPrefab;


    protected FloorTilemap floorTilemap;
    protected WallTilemap wallTilemap;

    public int Width => width;
    public int Height => height;
    public Vector3 Origin => origin;
    public float CellSize => cellSize;


    private void Awake()
    {
        floorTilemap = GetComponentInChildren<FloorTilemap>();
        wallTilemap = GetComponentInChildren<WallTilemap>();
    }

    
    private void Start()
    {
        BuildWallsAndFloors();
    }

    protected void BuildWallsAndFloors()
    {
        bool showDebug = false;

        BuildFloors(showDebug);
        BuildWalls(showDebug);
    }

    private void BuildWalls(bool showDebug)
    {
        wallTilemap.BuildRect(width + 2, height + 2, cellSize, origin - new Vector3(cellSize, cellSize), showDebug, wallPrefab, wallCornerPrefab);
    }

    private void BuildFloors(bool showDebug)
    {
        floorTilemap.BuildRect(width, height, cellSize, origin, showDebug, floorPrefab);
    }

    protected Vector3 GetWorldPosition(int x, int y)
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
