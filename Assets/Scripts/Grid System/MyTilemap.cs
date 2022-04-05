using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MyTilemap
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;

    private Tile[,] gridArray;
    private TextMeshPro[,] debugTextArray;


    public bool showDebug;

    /*
    parameter createGridObject is a delegate, like action, except it has a return val (TGridObject).
    for an int, () => { return 0; }
    for a custom object, () => { return new TGridObject(); } 
     */

    public MyTilemap(
        int width, 
        int height, 
        float cellSize, 
        Vector3 originPosition, 
        bool showDebug, 
        Func<int, int, Tile> tileCreator)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.showDebug = showDebug;

        gridArray = new Tile[width, height];
        debugTextArray = new TextMeshPro[width, height];

        PlaceTiles(tileCreator);

        if (showDebug)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    CreateDebugText(x, y, gridArray[x, y]?.ToString(), 2);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

    }

    private void PlaceTiles(Func<int, int, Tile> tileCreator)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
                gridArray[x, y] = tileCreator(x, y);
    }

    public Tile GetGridObject(int x, int y)
    {
        if (CoordinatesAreValid(x, y))
            return gridArray[x, y];
        else
            return null;
    }
    public Tile GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private void CreateDebugText(int x, int y, string value, int fontSize)
    {
        if (!CoordinatesAreValid(x, y)) return;

        GameObject txtObject = new GameObject("Grid Text", typeof(TextMeshPro));
        Vector3 offset = new Vector3(cellSize, cellSize) * 0.5f;
        txtObject.transform.localPosition = GetWorldPosition(x, y) + offset;

        TextMeshPro tmp = txtObject.GetComponent<TextMeshPro>();
        var tmpRectTransform = tmp.GetComponent<RectTransform>();
        tmpRectTransform.sizeDelta = new Vector2(cellSize, cellSize);
        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        tmp.SetText(value);
        tmp.fontSize = fontSize;
        tmp.color = Color.white;

        debugTextArray[x, y] = tmp;
    }

    private bool CoordinatesAreValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    //unused: places newly generated game object (not prefab) on the cell at x, y
    private void BuildCell(Sprite cellSprite, int x, int y)
    {
        if (!CoordinatesAreValid(x, y)) return;
        GameObject gameObject = new GameObject("Ground Tile");
        if (cellSprite != null)
        {
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.sprite = cellSprite;
        }
        Vector3 offset = new Vector3(cellSize, cellSize) * 0.5f;
        Transform transform = gameObject.transform;
        transform.localPosition = GetWorldPosition(x, y) + offset;
    }
}