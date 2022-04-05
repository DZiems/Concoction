using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTilemap : MonoBehaviour
{
    private MyTilemap tilemap;

    //for now, only use the first tile type provided.
    //Later, use a tilemap for different flooring tiles (if a floor has more than one flooring tile)
    public void Build(int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Tile floorTile)
    {
        if (floorTile == null) return;
        if (!DimensionsAreValid(width, height)) return;

        tilemap = new MyTilemap(width, height, cellSize, originPosition, showDebug,
            (x, y) => {
                var tile = Instantiate(floorTile, this.transform);
                tile.transform.position = DetermineWorldPosition(cellSize, originPosition, x, y);
                if (showDebug)
                    tile.gameObject.name = $"Floor ({x}, {y})";
                return tile;
;            });
    }

    public void Build( int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Dictionary<string, Tile> tilesDictionary, string[,] tilemapGrid)
    {
        if (tilemapGrid.GetLength(0) != width || tilemapGrid.GetLength(1) != height)
        {
            Debug.LogError($"FloorTilemap.Build(): provided tilemapGrid was not the correct dimensions. Needed: ({width}, {height}); Got: {tilemapGrid.GetLength(0)}, {tilemapGrid.GetLength(1)}");
            return;
        }
        if (tilesDictionary.Count <= 0)
        {
            Debug.LogError("FloorTilemap.Build(): provided tilesDictionary was empty!");
            return;
        }

        tilemap = new MyTilemap(width, height, cellSize, originPosition, showDebug,
            (x, y) => {
                var tilePosition = new Vector3(x, y) * cellSize + originPosition + (new Vector3(cellSize, cellSize) * 0.5f);
                var tile = GetTile(x, y, tilemapGrid, tilesDictionary);
                Instantiate(tile, this.transform);
                tile.transform.position = tilePosition;
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

    private Tile GetTile(int x, int y, string [,] mapGrid, Dictionary<string, Tile> dictionary)
    {
        return dictionary[mapGrid[x, y]];
    }

}

