using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //set by the ControllerManager
    public int Id { get; set; }
    public bool IsAssigned { get; set; }

    //Inputs
    /// <summary>  Default (A) </summary>
    public bool InteractPress { get; private set; }

    /// <summary>  Default (B) </summary>
    public bool Slot0Press { get; private set; }

    /// <summary>  Default (X) </summary>
    public bool Slot1Press { get; private set; }

    /// <summary>  Default (Y) </summary>
    public bool Slot2Press { get; private set; }

    /// <summary>  Default (RB) </summary>
    public bool DashPress { get; private set; }

    /// <summary>  Default (LB) </summary>
    public bool SpecialPress { get; private set; }

    /// <summary>  (start) </summary>
    public bool StartPress { get; private set; }

    /// <summary>  (back) </summary>
    public bool BackPress { get; private set; }

    /// <summary>  (left stick x-axis) </summary>
    public float Horizontal { get; private set; }

    /// <summary>  (left stick x-axis, first rightward press) </summary>
    public bool HorizontalRightPress { get; private set; }

    /// <summary>  (left stick x-axis, first leftward press) </summary>
    public bool HorizontalLeftPress { get; private set; }

    /// <summary>  (left stick y-axis) </summary>
    public float Vertical { get; private set; }

    /// <summary>  (left stick y-axis, first upward press) </summary>
    public bool VerticalUpPress { get; private set; }

    /// <summary>  (left stick y-axis, first downward press) </summary>
    public bool VerticalDownPress { get; private set; }

    /// <summary>  (right stick x-axis) </summary>
    public float AimHorizontal { get; private set; }

    /// <summary>  (right stick y-axis) </summary>
    public float AimVertical { get; private set; }

    private const string interactID = "Interact";      //a
    private const string slot0ID = "Slot0";         //b
    private const string slot1ID = "Slot1";         //x
    private const string slot2ID = "Slot2";         //y
    private const string dashID = "Dash";          //rb
    private const string specialID = "Special";       //lb
    private const string startID = "Start";         //start
    private const string backID = "Back";           //back
    private const string horizontalID = "Horizontal";    //l stick
    private const string verticalID = "Vertical";      //l stick
    private const string aimHorizontalID = "AimHorizontal"; //r stick
    private const string aimVerticalID = "AimVertical";   //r stick

    private bool isHorizontalPressPossible;
    private bool isVerticalPressPossible;
    private float axisPressedThreshold = 0.5f;

    private void Awake()
    {
        IsAssigned = false;
        isHorizontalPressPossible = true;
        isVerticalPressPossible = true;
    }

    // Update is called once per frame
    void Update()
    {
        InteractPress = Input.GetButtonDown(interactID);
        Slot0Press = Input.GetButtonDown(slot0ID);
        Slot1Press = Input.GetButtonDown(slot1ID);
        Slot2Press = Input.GetButtonDown(slot2ID);

        DashPress = Input.GetButtonDown(dashID);
        SpecialPress = Input.GetButtonDown(specialID);
        StartPress = Input.GetButtonDown(startID);
        BackPress = Input.GetButtonDown(backID);

        Horizontal = Input.GetAxis(horizontalID);
        DetermineHorizontalPress();

        Vertical = Input.GetAxis(verticalID);
        DetermineVerticalPress();

        AimHorizontal = Input.GetAxis(aimHorizontalID);
        AimVertical = Input.GetAxis(aimVerticalID);

        DebugButtons(false);
    }

    private void DebugButtons(bool on)
    {
        if (!on) return;
        if (InteractPress)
            Debug.Log("Interact (A)");
        if (Slot0Press)
            Debug.Log("Slot 0 (B)");
        if (Slot1Press)
            Debug.Log("Slot 1 (X)");
        if (Slot2Press)
            Debug.Log("Slot 2 (Y)");
        if (DashPress)
            Debug.Log("Roll (RB)");
        if (SpecialPress)
            Debug.Log("Special (LB)");
        if (StartPress)
            Debug.Log("Start (start)");
        if (BackPress)
            Debug.Log("Back (back)");
        if (HorizontalLeftPress)
            Debug.Log("Left (LStick)");
        if (HorizontalRightPress)
            Debug.Log("Right (LStick)");
        if (VerticalUpPress)
            Debug.Log("Up (LStick)");
        if (VerticalDownPress)
            Debug.Log("Down (LStick)");

    }

    private void DetermineHorizontalPress()
    {
        //if flag is down and there's activity
        if (isHorizontalPressPossible)
        {
            HorizontalRightPress = Horizontal >= axisPressedThreshold;
            HorizontalLeftPress = Horizontal <= -axisPressedThreshold;
            
            isHorizontalPressPossible = !(HorizontalRightPress || HorizontalLeftPress);
        }
        else
        {
            isHorizontalPressPossible = Mathf.Abs(Horizontal) < axisPressedThreshold;
            HorizontalLeftPress = false;
            HorizontalRightPress = false;
        }
    }

    private void DetermineVerticalPress()
    {
        //if flag is down and there's activity
        if (isVerticalPressPossible)
        {
            VerticalUpPress = Vertical >= axisPressedThreshold;
            VerticalDownPress = Vertical <= -axisPressedThreshold;

            isVerticalPressPossible = !(VerticalUpPress || VerticalDownPress);
        }
        else
        {
            isVerticalPressPossible = Mathf.Abs(Vertical) < axisPressedThreshold;
            VerticalUpPress = false;
            VerticalDownPress = false;
        }
    }


    internal Vector2 GetDirection()
    {
        return new Vector3(Horizontal, Vertical, 0);
    }

    internal Vector2 GetAimDirection()
    {
        return new Vector3(AimHorizontal, AimVertical, 0);
    }

    //TODO: expand this out for more cases
    public bool AnyButtonDown()
    {
        return 
            InteractPress ||
            Slot0Press ||
            Slot1Press ||
            Slot2Press ||
            SpecialPress ||
            DashPress ||
            StartPress ||
            HorizontalLeftPress ||
            HorizontalRightPress ||
            VerticalUpPress ||
            VerticalDownPress;
    }

}
