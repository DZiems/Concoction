
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//station placement sensitivity:
// - id has to be the same string from loaded file as the home base stationPrefabs[i] gameObject.name

[System.Serializable]
public class StationData
{
    public string id;
    public int level;
    public bool isUnlocked;
    public bool isPlaced;
    public int[] tiledPosition;
    public float[] rotationEulers;


    public StationData(string id, int level, bool isUnlocked, bool isPlaced, Tuple<int, int> tiledPosition, Vector3 rotationEulers)
    {
        this.id = id;
        this.level = level;

        this.tiledPosition = new int[2];
        this.tiledPosition[0] = tiledPosition.Item1;
        this.tiledPosition[1] = tiledPosition.Item2;


        this.rotationEulers = new float[3];
        this.rotationEulers[0] = rotationEulers.x;
        this.rotationEulers[1] = rotationEulers.y;
        this.rotationEulers[2] = rotationEulers.z;
    }
}
