using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileRotateTool
{
    public static Vector3 TopRot => Vector3.forward * 0f;
    public static Vector3 RightRot => Vector3.forward * 270f;
    public static Vector3 BottomRot => Vector3.forward * 180f;
    public static Vector3 LeftRot => Vector3.forward * 90f;

    public static readonly string Top = "TopRotate";
    public static readonly string Right = "RightRotate";
    public static readonly string Bottom = "BottomRotate";
    public static readonly string Left = "LeftRotate";

    public static Vector3 GetRotation(string dir)
    {
        if (dir == Top)
            return TopRot;
        else if (dir == Right)
            return RightRot;
        else if (dir == Bottom)
            return BottomRot;
        else if (dir == Left)
            return LeftRot;
        else
            return Vector3.zero;
    }

    public static string GetDir(Vector3 rotation)
    {
        if (rotation == RightRot)
            return Right;
        else if (rotation == BottomRot)
            return Bottom;
        else if (rotation == LeftRot)
            return Left;
        else
            return Top;
    } 
}
