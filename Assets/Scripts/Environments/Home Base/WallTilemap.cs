using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTilemap : MonoBehaviour
{
    private MyTilemap tilemap;
    public static Dictionary<string, string> TileNames { get; private set; }

    public enum Side
    {
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        TopLeft,
        None
    }

    //TODO: build based on a boolean map, and determine whether something is a corner based on having perpendicular adjacencies. Determine side as well?
    public void BuildRectWithEntrance(int width, int height, float cellSize, Vector3 originPosition, bool showDebug,Tile wallTile, Tile wallCornerTile, Side entrance, int entranceSize)
    {
        if (!DimensionsAreValid(width, height)) return;
        if (wallTile == null || wallCornerTile == null) return;

        tilemap = new MyTilemap(width, height, cellSize, originPosition, showDebug,
            (x, y) =>
            {
                Side wallSide = DetermineSide(width, height, x, y);

                if (wallSide != Side.None && !IsAnEntranceXY(entrance, entranceSize, width, height, x, y))
                {
                    var tile = Instantiate((IsACorner(wallSide) ? wallCornerTile : wallTile), this.transform);
                    tile.transform.position = DetermineWorldPosition(cellSize, originPosition, x, y);
                    tile.transform.Rotate(DetermineRotation(wallSide));
                    if (showDebug)
                        tile.gameObject.name = $"{wallSide} ({x}, {y})";

                    return tile;
                }
                else
                {
                    return null;
                }
            });
    }
    public void BuildRect(int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Tile wallTile, Tile wallCornerTile)
    {
        if (!DimensionsAreValid(width, height)) return;
        if (wallTile == null || wallCornerTile == null) return;

        tilemap = new MyTilemap(width, height, cellSize, originPosition, showDebug,
            (x, y) =>
            {
                Side wallSide = DetermineSide(width, height, x, y);

                if (wallSide != Side.None)
                {
                    var tile = Instantiate((IsACorner(wallSide) ? wallCornerTile : wallTile), this.transform);
                    tile.transform.position = DetermineWorldPosition(cellSize, originPosition, x, y);
                    tile.transform.Rotate(DetermineRotation(wallSide));
                    if (showDebug)
                        tile.gameObject.name = $"{wallSide} ({x}, {y})";

                    return tile;
                }
                else
                {
                    return null;
                }
            });
    }

    private bool DimensionsAreValid(int width, int height)
    {
        return width > 0 && height > 0;
    }
    private bool IsAnEntranceXY(Side entrance, int entranceSize, int width, int height, int x, int y)
    {
        int entranceStartX, entranceStartY;

        switch (entrance)
        {
            case Side.Top:
                entranceStartX = (width) / 2 - entranceSize / 2;
                entranceStartY = height - 1;
                return (x >= entranceStartX && x < entranceStartX + entranceSize) && y == entranceStartY;
            case Side.Right:
                entranceStartX = width - 1;
                entranceStartY = (height) / 2 - entranceSize / 2;
                return (y >= entranceStartY && y < entranceStartY + entranceSize) && x == entranceStartX;
            case Side.Bottom:
                entranceStartX = (width) / 2 - entranceSize / 2;
                entranceStartY = 0;
                return (x >= entranceStartX && x < entranceStartX + entranceSize) && y == entranceStartY;
            case Side.Left:
                entranceStartX = 0;
                entranceStartY = (height) / 2 - entranceSize / 2;
                return (y >= entranceStartY && y < entranceStartY + entranceSize) && x == entranceStartX;
            default:
                return false;
        }

    }

    private static Vector3 DetermineWorldPosition(float cellSize, Vector3 originPosition, int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition + (new Vector3(cellSize, cellSize) * 0.5f);
    }

    //find rotation based on side
    private Vector3 DetermineRotation(Side wallSide)
    {
        switch (wallSide)
        {
            case Side.Top:
            case Side.TopRight:
                return Vector3.forward * 0f;
            case Side.Right:
            case Side.BottomRight:
                return Vector3.forward * 270f;
            case Side.Bottom:
            case Side.BottomLeft:
                return Vector3.forward * 180f;
            case Side.Left:
            case Side.TopLeft:
                return Vector3.forward * 90f;
            case Side.None:
            default:
                return Vector3.forward * 0f;
        }
    }

    private Side DetermineSide(int width, int height, int x, int y)
    {
        if (x == 0) //is left
        {
            if (y == 0) //is bottom
                return Side.BottomLeft;
            else if (y == height - 1) //is top
                return Side.TopLeft;
            else //neither bottom or top
                return Side.Left;
        }
        else if (x == width - 1)
        {
            if (y == 0) //is bottom
                return Side.BottomRight;
            else if (y == height - 1) //is top
                return Side.TopRight;
            else //neither bottom or top
                return Side.Right;
        }
        else if (y == 0)
            return Side.Bottom;
        else if (y == height - 1)
            return Side.Top;
        else
            return Side.None;
    }

    private bool IsACorner(Side side)
    {
        return side == Side.BottomLeft ||
            side == Side.BottomRight ||
            side == Side.TopLeft ||
            side == Side.TopRight;
    }

    private Tile GetTile(int x, int y, string[,] mapGrid, Dictionary<string, Tile> dictionary)
    {
        return dictionary[mapGrid[x, y]];
    }


}
