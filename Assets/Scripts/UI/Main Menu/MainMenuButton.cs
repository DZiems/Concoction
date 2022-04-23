using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuButton : MonoBehaviour
{
    
    [SerializeField] private MainMenuHandler.Option id;

    public MainMenuHandler.Option Id => id;
    
}
