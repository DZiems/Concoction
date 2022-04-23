using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterData
{
    public string profileName;

    public PlayerCharacterData(string profileName)
    {
        this.profileName = profileName;
    }

    public PlayerCharacterData(Player player, Character character)
    {
        this.profileName = character.ProfileName;
    }
}
