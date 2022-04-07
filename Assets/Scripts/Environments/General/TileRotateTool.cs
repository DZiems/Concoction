using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileRotateTool
{
    public static Vector3 TopRot => Vector3.forward * 0f;
    public static Vector3 RightRot => Vector3.forward * 270f;
    public static Vector3 BottomRot => Vector3.forward * 180f;
    public static Vector3 LeftRot => Vector3.forward * 90f;

    public enum Dir
    {
        Top, Right, Bottom, Left
    }

    public static Vector3 GetRotation(Dir dir)
    {
        switch (dir)
        {
            case Dir.Top:
                return TopRot;
            case Dir.Right:
                return RightRot;
            case Dir.Bottom:
                return BottomRot;
            case Dir.Left:
                return LeftRot;
            default:
                return TopRot;
        }
    }

    public static Dir GetDir(Vector3 rotation)
    {
        if (rotation == RightRot)
            return Dir.Right;
        else if (rotation == BottomRot)
            return Dir.Bottom;
        else if (rotation == LeftRot)
            return Dir.Left;
        else
            return Dir.Top;
    } 
}
