using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Station : MonoBehaviour
{
    [SerializeField] private StationType type;
    [SerializeField] private int tiledWidth = 1;
    [SerializeField] private int tiledHeight = 1;

 
    //TODO: could turn into a queue where startIndex + count % capacity determines loop; helps with garbage collection
    private List<PlayerCharacter> charactersInsideUseSpace;
    private SpriteRenderer spriteRenderer;

    public int Level { get; private set; }
    public bool IsUnlocked { get; private set; }
    public bool IsPlaced { get; private set; }
    public int TiledWidth => tiledWidth;
    public int TiledHeight => tiledHeight;
    public StationType Type => type;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        charactersInsideUseSpace = new List<PlayerCharacter>();
        charactersInsideUseSpace.Capacity = 4;  //TODO: replace with TOTAL_NUM_PLAYERS
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<PlayerCharacter>();
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
        var character = collision.GetComponent<PlayerCharacter>();
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
                if (character.Controller.InteractPress)
                {
                    Debug.Log("Using");
                }
            }
        }
    }

    public Tuple<int, int>[] tilesOccupied(int originX, int originY, string rotationDir)
    {
        var occupanices = new Tuple<int, int>[tiledHeight * tiledWidth];

        if (rotationDir == TileRotateTool.Top)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX + i, originY + j);

        else if (rotationDir == TileRotateTool.Right)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX + i, originY - j);

        else if (rotationDir == TileRotateTool.Bottom)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX - i, originY - j);

        else if (rotationDir == TileRotateTool.Left)
            for (int i = 0; i < tiledWidth; i++)
                for (int j = 0; j < tiledHeight; j++)
                    occupanices[i] = new Tuple<int, int>(originX - i, originY + j);

        return occupanices;
    }

    public bool isWithinWalls(int width, int height, Tuple<int, int> tiledPos)
    {

        string rotateDir = TileRotateTool.GetDir(transform.rotation.eulerAngles);
        //pivot point in bottom left, draws up and right
        //x: left, add i
        //y: bottom, add j
        int i, j;
        if (rotateDir == TileRotateTool.Top)
            for (i = 0; i < TiledWidth; i++)
            {
                for (j = 0; j < TiledHeight; j++)
                {
                    if (tiledPos.Item1 + i < 0 ||
                        tiledPos.Item1 + i >= width ||
                        tiledPos.Item2 + j < 0 ||
                        tiledPos.Item2 + j >= height)
                    {
                        return false;
                    }
                }
            }
        //pivot point in top left, draws down and right
        //x: left, add i, what was width is height here since rotated
        //y: top, subtract j, what was height is width here since rotated
        //change which width or height i and j use
        else if (rotateDir == TileRotateTool.Right)
            for (i = 0; i < TiledHeight; i++)
            {
                for (j = 0; j < TiledWidth; j++)
                {
                    //note the swapped <= and > for j
                    if (tiledPos.Item1 + i < 0 ||
                        tiledPos.Item1 + i >= width ||
                        tiledPos.Item2 - j <= 0 ||
                        tiledPos.Item2 - j > height)
                    {
                        return false;
                    }
                }
            }
        //pivot point in top right, draws down and left
        //x: right, subtract i
        //y: top, subtract j
        else if (rotateDir == TileRotateTool.Bottom)
            for (i = 0; i < TiledWidth; i++)
            {
                for (j = 0; j < TiledHeight; j++)
                {
                    //note the swapped <= and > for i and j.
                    if (tiledPos.Item1 - i <= 0 ||
                        tiledPos.Item1 - i > width ||
                        tiledPos.Item2 - j <= 0 ||
                        tiledPos.Item2 - j > height)
                    {
                        return false;
                    }
                }
            }

        //pivot point in bottom right, draws up and left
        //x: right, subtract i, what was width is height here since rotated
        //y: top, add j, what was height is width here since rotated
        //change which width or height i and j use
        else if (rotateDir == TileRotateTool.Left)
            for (i = 0; i < TiledHeight; i++)
            {
                for (j = 0; j < TiledWidth; j++)
                {
                    //note the swapped <= and > for i
                    if (tiledPos.Item1 - i <= 0 ||
                        tiledPos.Item1 - i > width ||
                        tiledPos.Item2 + j < 0 ||
                        tiledPos.Item2 + j >= height)
                    {
                        return false;
                    }
                }
            }

        return true;
    }

}
