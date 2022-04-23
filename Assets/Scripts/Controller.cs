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
    public bool InteractDown { get; private set; }
    public bool Slot0Down { get; private set; }
    public bool Slot1Down { get; private set; }
    public bool Slot2Down { get; private set; }
    public bool RollDown { get; private set; }
    public bool SpecialDown { get; private set; }
    public bool StartDown { get; private set; }
    public float Horizontal { get; private set; }
    public float HorizontalDown { get; private set; }
    public float Vertical { get; private set; }
    public float VerticalDown { get; private set; }
    public float AimHorizontal { get; private set; }
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

    private bool horizontalDownFlag;
    private bool verticalDownFlag;
    private float axisDownAmount = 0.5f;

    private void Awake()
    {
        IsAssigned = false;
    }

    // Update is called once per frame
    void Update()
    {
        InteractDown = Input.GetButtonDown(interactID);
        Slot0Down = Input.GetButtonDown(slot0ID);
        Slot1Down = Input.GetButtonDown(slot1ID);
        Slot2Down = Input.GetButtonDown(slot2ID);

        RollDown = Input.GetButtonDown(rollID);
        SpecialDown = Input.GetButtonDown(specialID);
        StartDown = Input.GetButtonDown(startID);

        Horizontal = Input.GetAxis(horizontalID);
        HorizontalDown = DetermineHorizontalDown();
        Vertical = Input.GetAxis(verticalID);
        VerticalDown = DetermineVerticalDown();

        AimHorizontal = Input.GetAxis(aimHorizontalID);
        AimVertical = Input.GetAxis(aimVerticalID);

        DebugButtons(false);
    }

    private void DebugButtons(bool on)
    {
        if (!on) return;
        if (InteractDown)
            Debug.Log("Interact (A)");
        if (Slot0Down)
            Debug.Log("Slot 0 (B)");
        if (Slot1Down)
            Debug.Log("Slot 1 (X)");
        if (Slot2Down)
            Debug.Log("Slot 2 (Y)");
        if (RollDown)
            Debug.Log("Roll (RB)");
        if (SpecialDown)
            Debug.Log("Special (LB)");
        if (StartDown)
            Debug.Log("Start (start)");
    }

    private float DetermineHorizontalDown()
    {
        //if flag is down and there's activity
        if (!horizontalDownFlag && 
            (Horizontal > axisDownAmount || Horizontal < -axisDownAmount))
        {
            horizontalDownFlag = true;
            return Horizontal;
        }
        else
        {
            if (Horizontal == 0)
                horizontalDownFlag = false;
            return 0f;
        }
    }

    private float DetermineVerticalDown()
    {
        //if flag is down and there's activity
        if (!verticalDownFlag && 
            (Vertical > axisDownAmount || Vertical < -axisDownAmount))
        {
            verticalDownFlag = true;
            return Vertical;
        }
        else
        {
            if (Vertical == 0)
                verticalDownFlag = false;
            return 0f;
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
        return InteractDown || 
            Slot0Down || 
            Slot1Down || 
            Slot2Down ||
            SpecialDown || 
            RollDown || 
            StartDown ||
            (HorizontalDown != 0) || (VerticalDown != 0);
    }

}
