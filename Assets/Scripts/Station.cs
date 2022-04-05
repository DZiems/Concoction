using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Tile
{
    [SerializeField] private int tiledWidth = 1;
    [SerializeField] private int tiledHeight = 1;

    private List<Character> charactersInsideUseSpace;

    protected override void Awake()
    {
        base.Awake();
        charactersInsideUseSpace = new List<Character>();
        charactersInsideUseSpace.Capacity = 4;  //TOTAL_NUM_PLAYERS
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            if (charactersInsideUseSpace.Count == 0)
            {
                Debug.Log("Prompt Player to interact");
            }
            charactersInsideUseSpace.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            charactersInsideUseSpace.Remove(character);
            if (charactersInsideUseSpace.Count == 0)
            {
                Debug.Log("Remove Player interact prompt");
            }
        }


    }

    private void Update()
    {
        if (charactersInsideUseSpace.Count > 0)
        {
            foreach (var character in charactersInsideUseSpace)
            {
                if (character.Controller.InteractDown)
                {
                    Debug.Log("Using");
                }
            }
        }
    }

}
