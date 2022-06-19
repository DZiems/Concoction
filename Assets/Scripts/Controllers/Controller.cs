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
    public bool RollPress { get; private set; }

    /// <summary>  Default (LB) </summary>
    public bool SpecialPress { get; private set; }

    /// <summary>  (start) </summary>
    public bool StartPress { get; private set; }

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

    private string interactID;      //a
    private string slot0ID;         //b
    private string slot1ID;         //x
    private string slot2ID;         //y
    private string rollID;          //rb
    private string specialID;       //lb
    private string startID;         //start
    private string horizontalID;    //l stick
    private string verticalID;      //l stick
    private string aimHorizontalID; //r stick
    private string aimVerticalID;   //r stick

    private bool isHorizontalPressPossible;
    private bool isVerticalPressPossible;
    private float axisPressAmount = 0.5f;

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

        RollPress = Input.GetButtonDown(rollID);
        SpecialPress = Input.GetButtonDown(specialID);
        StartPress = Input.GetButtonDown(startID);

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
        if (RollPress)
            Debug.Log("Roll (RB)");
        if (SpecialPress)
            Debug.Log("Special (LB)");
        if (StartPress)
            Debug.Log("Start (start)");
    }

    private void DetermineHorizontalPress()
    {
        //if flag is down and there's activity
        if (isHorizontalPressPossible)
        {
            HorizontalRightPress = Horizontal >= axisPressAmount;
            HorizontalLeftPress = Horizontal <= -axisPressAmount;
            
            isHorizontalPressPossible = !(HorizontalRightPress || HorizontalLeftPress);
        }
        else
        {
            isHorizontalPressPossible = Mathf.Abs(Horizontal) < axisPressAmount;
            HorizontalLeftPress = false;
            HorizontalRightPress = false;
        }
    }

    private void DetermineVerticalPress()
    {
        //if flag is down and there's activity
        if (isVerticalPressPossible)
        {
            VerticalUpPress = Vertical >= axisPressAmount;
            VerticalDownPress = Vertical <= -axisPressAmount;

            isVerticalPressPossible = !(VerticalUpPress || VerticalDownPress);
        }
        else
        {
            isVerticalPressPossible = Mathf.Abs(Vertical) < axisPressAmount;
            VerticalUpPress = false;
            VerticalDownPress = false;
        }
    }

    internal void SetId(int id)
    {
        Id = id;
        interactID = $"P{Id}Interact";
        slot0ID = $"P{Id}Slot0";
        slot1ID = $"P{Id}Slot1";
        slot2ID = $"P{Id}Slot2";
        rollID = $"P{Id}Roll";
        specialID = $"P{Id}Special";
        startID = $"P{Id}Start";
        horizontalID = $"P{Id}Horizontal";
        verticalID = $"P{Id}Vertical";
        aimHorizontalID = $"P{Id}AimHorizontal";
        aimVerticalID = $"P{Id}AimVertical";

        gameObject.name = $"Controller {Id}";
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
            RollPress ||
            StartPress ||
            HorizontalLeftPress ||
            HorizontalRightPress ||
            VerticalUpPress ||
            VerticalDownPress;
    }

}
