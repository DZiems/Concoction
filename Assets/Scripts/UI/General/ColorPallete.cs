using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPallete
{
    public static readonly Color selectedColor = Color.white;
    public static readonly Color unselectedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
    public static readonly Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);
                   
    public static readonly Color dropdownFieldSelected = new Color(0.2666667f, 0.2f, 0.3254902f, 1);
    public static readonly Color dropdownFieldHighlighted = new Color(0.4f, 0.4f, 0.4f, 1);
    public static readonly Color dropdownFieldSelectAndHighlight = new Color(0.4666667f, 0.4f, 0.5254902f, 1);
    public static readonly Color dropdownFieldNormal = new Color(0.25f, 0.25f, 0.25f, 1);
                   
    public static readonly Color dropdownTextSelected = Color.white;
    public static readonly Color dropdownTextHighlighted = Color.white;
    public static readonly Color dropdownTextNormal = new Color(0.75f, 0.75f, 0.75f, 1);

    public static readonly Color alphabetFieldBackground = new Color(0.2f, 0.2f, 0.2f, 1f);

    public static readonly Color healthBarColor = new Color(1.0f, 0.2f, 0.2f, 1f);
    public static readonly Color healthBarDamageColor = new Color(1.0f, 0.7f, 0.2f, 1f);
    public static readonly Color healthBarHealColor = new Color(0.2f, 1.0f, 0.3f, 1f);
}
