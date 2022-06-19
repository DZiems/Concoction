using UnityEngine;

[System.Serializable]
public class PlayerCharacterData
{
    public static Color DefaultRobesColor = Color.white;
    public static Color DefaultSkinColor = new Color(0.75f, 0.65f, 0.58f, 1f);
    public static Color DefaultGogglesLensColor = new Color(0.7f, 1.0f, 0.7f, 1.0f);
    public static Color DefaultAimReticleColor = Color.red;

    public Color robesColor;
    public Color skinColor;
    public Color gogglesLensColor;
    public Color aimReticleColor;

    public PlayerCharacterData()
    {
        robesColor = DefaultRobesColor;
        skinColor = DefaultSkinColor;
        gogglesLensColor = DefaultGogglesLensColor;
        aimReticleColor = DefaultAimReticleColor;
    }

    public PlayerCharacterData(Color robesColor, Color skinColor, Color gogglesLensColor, Color aimReticleColor)
    {
        this.robesColor = robesColor;
        this.skinColor = skinColor;
        this.gogglesLensColor = gogglesLensColor;
        this.aimReticleColor = aimReticleColor;
    }

    public PlayerCharacterData(Player player)
    {
        robesColor = player.profileData.playerCharacterData.robesColor;
        skinColor = player.profileData.playerCharacterData.skinColor;
        gogglesLensColor = player.profileData.playerCharacterData.gogglesLensColor;
        aimReticleColor = player.profileData.playerCharacterData.aimReticleColor;
    }
}