using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetField : MonoBehaviour
{
    [SerializeField] private GameObject lowercaseRegion;
    [SerializeField] private GameObject uppercaseRegion;
    [SerializeField] private float selectScaleFactor = 1.2f;

    public string CurrentLetter => isLowercase? lowercase[col, row] : uppercase[col, row];

    private const int width = 4;
    private const int height = 8;
    private int row, col;
    private bool isLowercase;
    private TextMeshProUGUI[,] lowercaseLetters;
    private TextMeshProUGUI[,] uppercaseLetters;

    private readonly string[,] lowercase = new string[height, width] {
            { "a", "b", "c", "d"},
            { "e", "f", "g", "h"},
            { "i", "j", "k", "l"},
            { "m", "n", "o", "p"},
            { "q", "r", "s", "t"},
            { "u", "v", "w", "x"},
            { "y", "z", "1", "2"},
            { "3", "4", "5", "6"}};
    private readonly string[,] uppercase = new string[height, width] {
            { "A", "B", "C", "D"},
            { "E", "F", "G", "H"},
            { "I", "J", "K", "L"},
            { "M", "N", "O", "P"},
            { "Q", "R", "S", "T"},
            { "U", "V", "W", "X"},
            { "Y", "Z", "7", "8"},
            { "9", "0", "_", "^"}};

    private void Awake()
    {

        lowercaseLetters = new TextMeshProUGUI[height, width];
        int x, y = 0;
        foreach (var row in lowercaseRegion.GetComponentsInChildren<HorizontalLayoutGroup>())
        {
            x = 0;
            foreach (var letterTMP in row.GetComponentsInChildren<TextMeshProUGUI>())
            {
                lowercaseLetters[y, x] = letterTMP;
                x++;
            }
            y++;
        }
            

        uppercaseLetters = new TextMeshProUGUI[height, width];
        y = 0;
        foreach (var row in uppercaseRegion.GetComponentsInChildren<HorizontalLayoutGroup>())
        {
            x = 0;
            foreach (var letterTMP in row.GetComponentsInChildren<TextMeshProUGUI>())
            {
                uppercaseLetters[y, x] = letterTMP;
                x++;
            }
            y++;
        }

        x = 0;
        y = 0;

    }
    private void Start()
    {
        ActivateUppercase();
    }

    //TODO: implement
    private void HoverIndexedLetter()
    {
        if (isLowercase)
        {
            lowercaseLetters[col, row].color = ColorPallete.selectedColor;
            lowercaseLetters[col, row].transform.localScale = Vector3.one * selectScaleFactor;
        }
        else
        {
            uppercaseLetters[col, row].color = ColorPallete.selectedColor;
            uppercaseLetters[col, row].transform.localScale = Vector3.one * selectScaleFactor;
        }
    }

    //TODO: implement
    private void UnhoverIndexedLetter()
    {
        if (isLowercase)
        {
            lowercaseLetters[col, row].color = ColorPallete.unselectedColor;
            lowercaseLetters[col, row].transform.localScale = Vector3.one;
        }
        else
        {
            uppercaseLetters[col, row].color = ColorPallete.unselectedColor;
            uppercaseLetters[col, row].transform.localScale = Vector3.one;
        }
    }
    public void MoveRight()
    {
        UnhoverIndexedLetter();

        row++;
        if (row > width - 1)
            row = 0;

        HoverIndexedLetter();
    }

    public void MoveLeft()
    {
        UnhoverIndexedLetter();

        row--;
        if (row < 0)
            row = width - 1;

        HoverIndexedLetter();
    }

    public void MoveUp()
    {
        UnhoverIndexedLetter();

        col++;
        if (col > height - 1)
            col = 0;

        HoverIndexedLetter();
    }

    public void MoveDown()
    {
        UnhoverIndexedLetter();
        col--;
        if (col < 0)
            col = height - 1;

        HoverIndexedLetter();
    }

    

    public void ToggleCasing()
    {
        if (isLowercase)
            ActivateUppercase();
        else
            ActivateLowercase();
    }

    private void ActivateUppercase()
    {
        UnhoverIndexedLetter();
        lowercaseRegion.SetActive(false);
        uppercaseRegion.SetActive(true);
        isLowercase = false;
        HoverIndexedLetter();
    }

    private void ActivateLowercase()
    {
        UnhoverIndexedLetter();
        lowercaseRegion.SetActive(true);
        uppercaseRegion.SetActive(false);
        isLowercase = true;
        HoverIndexedLetter();
    }

    public void Reset()
    {
        UnhoverIndexedLetter();

        row = 0;
        col = 0;

        ActivateUppercase();
    }
}
