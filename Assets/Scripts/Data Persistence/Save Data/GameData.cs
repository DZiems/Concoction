using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public HomeBaseData HomeBaseData;

    //default values the game starts with when there's no data
    public GameData()
    {
        HomeBaseData = new HomeBaseData();
    }

    public GameData(HomeBaseData homeBaseData)
    {
        this.HomeBaseData = homeBaseData;
    }

    public override string ToString()
    {
        return $"{HomeBaseData}\n";
    }
}
