using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public HomeBaseData HomeBaseData;
    public SerializableDictionary<string, PlayerCharacterData> AllPlayerCharacterData;
    public string MostRecentScene;

    //default values the game starts with when there's no data
    public GameData()
    {
        HomeBaseData = new HomeBaseData();
        AllPlayerCharacterData = new SerializableDictionary<string, PlayerCharacterData>();
        MostRecentScene = GameManager.SceneHomeBase;
    }

    public GameData(string lastLoadedScene, HomeBaseData homeBaseData, SerializableDictionary<string, PlayerCharacterData> allPlayerData)
    {
        this.HomeBaseData = homeBaseData;
        this.MostRecentScene = lastLoadedScene;
        this.AllPlayerCharacterData = allPlayerData;
    }


    public override string ToString()
    {
        return $"{HomeBaseData}\n";
    }
}
