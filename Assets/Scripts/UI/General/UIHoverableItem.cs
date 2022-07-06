using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHoverableItem : MonoBehaviour
{
    protected Animator anim;
    protected TextMeshProUGUI itemText;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        itemText = GetComponentInChildren<TextMeshProUGUI>();

        Unhover();
    }

    public void Hover()
    {
        anim.SetBool("IsInactive", false);
        anim.SetBool("IsHovered", true);
    }

    public void Unhover()
    {
        anim.SetBool("IsInactive", false);
        anim.SetBool("IsHovered", false);
    }

    public void Deactivate()
    {
        anim.SetBool("IsHovered", false);
        anim.SetBool("IsInactive", true);
    }

    public void SetText(string text)
    {
        itemText.SetText(text);
    }

}
