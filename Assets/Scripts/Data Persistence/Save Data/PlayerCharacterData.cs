using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterData
{

    public static Color DefaultProfileColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    public static Color DefaultAimReticleColor = Color.red;

    public string profileName;
    public Color profileColor;
    public Color aimReticleColor;

    public PlayerCharacterData(string profileName)
    {
        this.profileName = profileName;
        this.profileColor = DefaultProfileColor;
        this.aimReticleColor = DefaultAimReticleColor;

    }
    public PlayerCharacterData(string profileName, Color profileColor, Color aimReticleColor)
    {
        this.profileName = profileName;
        this.profileColor = profileColor;
        this.aimReticleColor = aimReticleColor;
    }

    public PlayerCharacterData(Player player, Character character)
    {
        this.profileName = character.ProfileName;
    }

}
