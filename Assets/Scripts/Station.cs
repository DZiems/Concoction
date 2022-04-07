using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Station : MonoBehaviour
{
    [SerializeField] private int tiledWidth = 1;
    [SerializeField] private int tiledHeight = 1;

    //TODO: could turn into a queue where startIndex + count % capacity determines loop; helps with garbage collection
    private List<Character> charactersInsideUseSpace;

    private void Awake()
    {
        charactersInsideUseSpace = new List<Character>();
        charactersInsideUseSpace.Capacity = 4;  //TODO: replace with TOTAL_NUM_PLAYERS
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

    public Tuple<int, int>[] tilesOccupied(int originX, int originY, TileRotateTool.Dir rotationDir)
    {
        var occupanices = new Tuple<int, int>[tiledHeight * tiledWidth];

        if (rotationDir == TileRotateTool.Dir.Top)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX + i, originY + j);

        else if (rotationDir == TileRotateTool.Dir.Right)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX + i, originY - j);

        else if (rotationDir == TileRotateTool.Dir.Bottom)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX - i, originY - j);

        else if (rotationDir == TileRotateTool.Dir.Left)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX - i, originY + j);

        return occupanices;
    }

}
