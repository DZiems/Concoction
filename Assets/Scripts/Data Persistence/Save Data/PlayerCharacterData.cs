using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterData
{

    public static Color DefaultRobesColor = Color.white;
    public static Color DefaultSkinColor = new Color(0.75f, 0.65f, 0.58f, 1f);
    public static Color DefaultGogglesLensColor = new Color(0.7f, 1.0f, 0.7f, 1.0f);
    public static Color DefaultAimReticleColor = Color.red;

    public string profileName;
    public Color robesColor;
    public Color skinColor;
    public Color gogglesLensColor;
    public Color aimReticleColor;

    public PlayerCharacterData()
    {
        profileName = "Default Name";
        robesColor = DefaultRobesColor;
        skinColor = DefaultSkinColor;
        gogglesLensColor = DefaultGogglesLensColor;
        aimReticleColor = DefaultAimReticleColor;
    }

    public PlayerCharacterData(string profileName)
    {
        this.profileName = profileName;
        this.robesColor = DefaultRobesColor; 
        this.skinColor = DefaultSkinColor;
        this.gogglesLensColor = DefaultGogglesLensColor;
        this.aimReticleColor = DefaultAimReticleColor;

    }
    public PlayerCharacterData(string profileName, Color robesColor, Color skinColor, Color gogglesLensColor, Color aimReticleColor)
    {
        this.profileName = profileName;
        this.robesColor = robesColor;
        this.skinColor = skinColor;
        this.gogglesLensColor = gogglesLensColor;
        this.aimReticleColor = aimReticleColor;
    }

    public PlayerCharacterData(Player player, Character character)
    {
        this.profileName = character.ProfileName;
    }

}
