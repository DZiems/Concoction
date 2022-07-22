using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPallete
{
    //FFFFFF
    public static readonly Color selectedText = Color.white;
    //BFBFBF
    public static readonly Color unselectedText = new Color(0.75f, 0.75f, 0.75f, 1f);
    //808080
    public static readonly Color inactiveText = new Color(0.5f, 0.5f, 0.5f, 0.75f);


    public static readonly Color dropdownFieldSelected = new Color(0.5f, 0.5f, 0.5f, 1);
    public static readonly Color dropdownFieldHighlighted = new Color(0.4f, 0.4f, 0.4f, 1);
    public static readonly Color dropdownFieldSelectAndHighlight = new Color(0.6666666f, 0.6666666f, 0.6666666f, 1);
    public static readonly Color dropdownFieldNormal = new Color(0.25f, 0.25f, 0.25f, 1);
                   
    public static readonly Color dropdownTextSelected = Color.white;
    public static readonly Color dropdownTextHighlighted = Color.white;
    public static readonly Color dropdownTextNormal = new Color(0.75f, 0.75f, 0.75f, 1);

    public static readonly Color alphabetFieldBackground = new Color(0.2f, 0.2f, 0.2f, 1f);

    public static readonly Color healthBarColor = new Color(1.0f, 0.2f, 0.2f, 1f);
    public static readonly Color healthBarDamageColor = new Color(1.0f, 0.7f, 0.2f, 1f);
    public static readonly Color healthBarHealColor = new Color(0.2f, 1.0f, 0.3f, 1f);

    //ECECEC
    public static readonly Color commonRarity_Editor = new Color(0.9245283f, 0.9245283f, 0.9245283f);
    //47CA5E
    public static readonly Color uncommonRarity_Editor = new Color(0.2803489f, 0.7924528f, 0.3677813f);
    //7783F5
    public static readonly Color rareRarity_Editor = new Color(0.4675152f, 0.5128092f, 0.9622642f);
    //AE57FD
    public static readonly Color epicRarity_Editor = new Color(0.6810033f, 0.3410911f, 0.990566f);
    //EC9653
    public static readonly Color fabledRarity_Editor = new Color(0.9245283f, 0.5884601f, 0.3270737f);
}
