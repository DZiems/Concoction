using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Level
    {
        HomeBase
    }
    public Level currentLevel { get; private set; }
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnApplicationQuit()
    {
        //SaveSystem.Save(currentLevel);
    }


}
