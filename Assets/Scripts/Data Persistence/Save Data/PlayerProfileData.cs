using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfileData
{
    public PlayerCharacterData playerCharacterData;
    public HomeBaseData homeBaseData;
    public InventoryData inventoryData;
    public string profileName;
    public string mostRecentScene;

    public PlayerProfileData(string profileName)
    {
        this.profileName = profileName;
        mostRecentScene = GameManager.SceneHomeBase;

        playerCharacterData = new PlayerCharacterData();
        homeBaseData = new HomeBaseData();
        inventoryData = new InventoryData();
    }

    public PlayerProfileData(string profileName, string mostRecentScene, PlayerCharacterData playerCharacterData, HomeBaseData homeBaseData, InventoryData inventoryData)
    {
        this.profileName = profileName;
        this.mostRecentScene = mostRecentScene;

        this.playerCharacterData = playerCharacterData;
        this.homeBaseData = homeBaseData;
        this.inventoryData = inventoryData;
    }

}
