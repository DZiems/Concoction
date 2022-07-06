using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, PlayerProfileData> allPlayerProfileDatas;

    //default values the game starts with when there's no data
    //TODO: put homebasedata and mostrecentscene within playerprofiledata
    //TODO: build out inventory data.
    //TODO: build out character data as what playerprofiledata currently is.
    public GameData()
    {
        allPlayerProfileDatas = new SerializableDictionary<string, PlayerProfileData>();
    }

    public GameData(SerializableDictionary<string, PlayerProfileData> allPlayerCharacterData)
    {
        this.allPlayerProfileDatas = allPlayerCharacterData;
    }

    public PlayerProfileData CurrentPlayerProfileData => 
        PlayerManager.Instance.Player.profileData != null ? (allPlayerProfileDatas[PlayerManager.Instance.Player.profileData.profileName]) : (null);
}
