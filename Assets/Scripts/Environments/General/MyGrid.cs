using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MyGrid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;

    private TGridObject[,] gridArray;
    private TextMeshPro[,] debugTextArray;

    public event Action<int, int> OnGridValueChanged;

    public bool showDebug;

    /*
    parameter createGridObject is a delegate, like action, except it has a return val (TGridObject).
    for an int, () => { return 0; }
    for a custom object, () => { return new TGridObject(); } 
     */

    public MyGrid(
        int width, 
        int height, 
        float cellSize, 
        Vector3 originPosition,
        bool showDebug,
        Func<int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.showDebug = showDebug;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMeshPro[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
                gridArray[x, y] = createGridObject(x, y);

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

            OnGridValueChanged += (x, y) => { 
                debugTextArray[x, y].text = gridArray[x, y]?.ToString(); 
            };
        }

    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private Tuple<int, int> GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return new Tuple<int, int>(x, y);
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

    

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (CoordinatesAreValid(x, y))
        {
            gridArray[x, y] = value;

            if (OnGridValueChanged != null) OnGridValueChanged(x, y);
        }
    }
    public TGridObject GetGridObject(int x, int y)
    {
        if (CoordinatesAreValid(x, y))
            return gridArray[x, y];
        else
            return default(TGridObject);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        var xy = GetXY(worldPosition);
        SetGridObject(xy.Item1, xy.Item2, value);
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        var xy = GetXY(worldPosition);
        return GetGridObject(xy.Item1, xy.Item2);
    }

    private bool CoordinatesAreValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}
