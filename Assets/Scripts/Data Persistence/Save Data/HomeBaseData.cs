
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HomeBaseData
{
    public float[] origin;
    public int width;
    public int height;
    public float cellSize;

    public StationData[] stationData;

    public HomeBaseData (HomeBase homeBase)
    {
        origin = new float[2];
        origin[0] = homeBase.Origin.x;
        origin[1] = homeBase.Origin.y;

        width = homeBase.Width;
        height = homeBase.Height;

        cellSize = homeBase.CellSize;

        stationData = new StationData[homeBase.Stations.Length];
        int i = 0;
        foreach (var station in homeBase.Stations)
            stationData[i++] = 
                new StationData(
                    station.Type, 
                    station.Level, 
                    station.IsUnlocked, 
                    station.IsPlaced, 
                    homeBase.GetTiledXY(station.transform.position),
                    station.transform.rotation.eulerAngles);
    }

    public HomeBaseData()
    {
        origin = new float[2];
        origin[0] = 0f;
        origin[1] = 0f;

        width = 10;
        height = 6;

        cellSize = 1f;

        stationData = DefaultStations();
    }

    private StationData[] DefaultStations()
    {
        stationData = new StationData[3]
        {
            new StationData(StationType.AlchemyTable, 1, true, false, new Tuple<int, int>(9, 6), TileRotateTool.RightRot),
            new StationData(StationType.Chest, 1, true, false, new Tuple<int, int>(0, 5), TileRotateTool.TopRot),
            new StationData(StationType.Door, 1, true, false, new Tuple<int, int>(0, 1), TileRotateTool.LeftRot),
        };
        return stationData;
    }

    public override string ToString()
    {
        var theString = $"HomeBase(origin: ({origin[0]}, {origin[1]}), width: {width}, height: {height}, cellSize: {cellSize}), \n\tstations:";
        foreach (var station in stationData)
        {
            theString += $"\n\t\tid: {station.id}, level: {station.level}, tiledPos: ({station.tiledPosition[0]}, {station.tiledPosition[1]}, rotation: {TileRotateTool.GetDir(new Vector3(station.rotationEulers[0], station.rotationEulers[1] , station.rotationEulers[2]))})";
        }

        return theString;
    }
}


