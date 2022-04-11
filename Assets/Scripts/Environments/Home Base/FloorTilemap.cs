using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tilemap builds 
public class FloorTilemap : MonoBehaviour
{
    private MyTilemap tilemap;

    //TODO: another Build which allows for a tilemap and multiple tile ids
    //TODO: another Build which takes in a WallTilemap and applies a fill algorithm
    public void BuildRect(int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Tile floorTile)
    {
        if (floorTile == null) return;
        if (!DimensionsAreValid(width, height)) return;

        tilemap = new MyTilemap(width, height, cellSize, originPosition, showDebug,
            (x, y) =>
            {
                var tile = Instantiate(floorTile, this.transform);
                tile.transform.position = DetermineWorldPosition(cellSize, originPosition, x, y);
                if (showDebug)
                    tile.gameObject.name = $"Floor ({x}, {y})";
                return tile;
            });
    }


    private bool DimensionsAreValid(int width, int height)
    {
        return width > 0 && height > 0;
    }
    private bool CoordinatesAreValid(int x, int y, int width, int height)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private static Vector3 DetermineWorldPosition(float cellSize, Vector3 originPosition, int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition + (new Vector3(cellSize, cellSize) * 0.5f);
    }

    private Tile GetTile(int x, int y, string[,] mapGrid, Dictionary<string, Tile> dictionary)
    {
        return dictionary[mapGrid[x, y]];
    }

}

