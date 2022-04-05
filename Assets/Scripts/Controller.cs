using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int Index { get; set; }
    public bool IsAssigned { get; set; }
    public bool InteractDown { get; private set; }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float AimHorizontal { get; private set; }
    public float AimVertical { get; private set; }

    private string interactID;
    private string horizontalID;
    private string verticalID;
    private string aimHorizontalID;
    private string aimVerticalID;

    private void Awake()
    {
        IsAssigned = false;
    }

    // Update is called once per frame
    void Update()
    {
        InteractDown = Input.GetButtonDown(interactID);
        Horizontal = Input.GetAxis(horizontalID);
        Vertical = Input.GetAxis(verticalID);
        AimHorizontal = Input.GetAxis(aimHorizontalID);
        AimVertical = Input.GetAxis(aimVerticalID);
    }

    internal void SetIndex(int index)
    {
        Index = index;
        interactID = $"P{Index}Interact";
        horizontalID = $"P{Index}Horizontal";
        verticalID = $"P{Index}Vertical";
        aimHorizontalID = $"P{Index}AimHorizontal";
        aimVerticalID = $"P{Index}AimVertical";

        gameObject.name = $"Controller {Index}";
    }

    internal Vector2 GetDirection()
    {
        return new Vector3(Horizontal, Vertical, 0);
    }

    internal Vector2 GetAimDirection()
    {
        return new Vector3(AimHorizontal, AimVertical, 0);
    }

}
