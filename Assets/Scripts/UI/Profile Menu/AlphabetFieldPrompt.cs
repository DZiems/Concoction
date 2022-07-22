using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetFieldPrompt : MonoBehaviour
{
    [Header("Prompt Field")]
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private int maxCharacters = 12;

    [Header("Alphabet Field")]
    [SerializeField] private GameObject lowercaseRegion;
    [SerializeField] private GameObject uppercaseRegion;
    [SerializeField] private float selectLetterScaleFactor = 1.2f;

    [Header("Action Buttons")]
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject shiftButton;
    
    //accessed row, column (y, x)
    public string CurrentLetter => isLowercase ? lowercase[y, x] : uppercase[y, x];

    private const int width = 6;
    private const int height = 5;
    private int x, y;
    private bool isLowercase;
    private TextMeshProUGUI[,] lowercaseLetters;
    private TextMeshProUGUI[,] uppercaseLetters;

    private readonly string[,] lowercase = new string[height, width] {
            { "a", "b", "c", "d", "e", "f"},
            { "g", "h", "i", "j", "k", "l"},
            { "m", "n", "o", "p", "q", "r"},
            { "s", "t", "u", "v", "w", "x"},
            { "y", "z", ";", ":", "(", ")"} };
    private readonly string[,] uppercase = new string[height, width] {
            { "A", "B", "C", "D", "E", "F"},
            { "G", "H", "I", "J", "K", "L"},
            { "M", "N", "O", "P", "Q", "R"},
            { "S", "T", "U", "V", "W", "X"},
            { "Y", "Z", " ", "-", ".", "'"} };



    private CanvasGroup canvasGroup;
    private Animator anim;

    private string currentName;
    private const string enterNamePrompt = "Enter a name...";


    public string CurrentName => currentName;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<Animator>();

        BuildAlphabetField();

    }
    
    private void Start()
    {
        ResetUI();
    }


    private void BuildAlphabetField()
    {
        lowercaseLetters = new TextMeshProUGUI[height, width];
        int column, row = 0;
        foreach (var itemRow in lowercaseRegion.GetComponentsInChildren<HorizontalLayoutGroup>())
        {
            column = 0;
            foreach (var letterTMP in itemRow.GetComponentsInChildren<TextMeshProUGUI>())
            {
                lowercaseLetters[row, column] = letterTMP;
                letterTMP.color = ColorPallete.unselectedText;
                letterTMP.transform.localScale = Vector3.one;
                column++;
            }
            row++;
        }


        uppercaseLetters = new TextMeshProUGUI[height, width];
        row = 0;
        foreach (var itemRow in uppercaseRegion.GetComponentsInChildren<HorizontalLayoutGroup>())
        {
            column = 0;
            foreach (var letterTMP in itemRow.GetComponentsInChildren<TextMeshProUGUI>())
            {
                uppercaseLetters[row, column] = letterTMP;
                letterTMP.color = ColorPallete.unselectedText;
                letterTMP.transform.localScale = Vector3.one;
                column++;
            }
            row++;
        }
    }

    public void SelectLetter()
    {
        if (currentName.Length >= maxCharacters)
            return;

        currentName += CurrentLetter;
        FlashSelect();

        UpdateLabel();
    }

    public void DeleteLetter()
    {
        if (string.IsNullOrEmpty(currentName))
            return;

        currentName = currentName.Remove(currentName.Length - 1);
        FlashDelete();

        UpdateLabel();
    }

    public void ShiftCasing()
    {
        ToggleCasing();
        FlashShift();
    }

    public void UpdateLabel()
    {
        if (string.IsNullOrEmpty(currentName))
        {
            textField.SetText(enterNamePrompt);
            textField.color = ColorPallete.unselectedText;
        }
        else
        {
            textField.SetText(currentName);
            textField.color = ColorPallete.selectedText;
        }
    }

    public void Show()
    {
        ResetUI();
        anim.SetBool("Active", true);
    }

    public void Hide()
    {
        anim.SetBool("Active", false);
    }

    public void ResetUI()
    {
        currentName = string.Empty;
        UnhoverIndexedLetter();
        x = 0;
        y = 0;
        ActivateUppercase();

        UpdateLabel();
    }


    public void FlashDelete()
    {
        StartCoroutine(PressButtonCoroutine(deleteButton));
    }
    public void FlashSelect()
    {
        StartCoroutine(PressButtonCoroutine(selectButton));
    }

    public void FlashShift()
    {
        StartCoroutine(PressButtonCoroutine(shiftButton));
    }

    private IEnumerator PressButtonCoroutine(GameObject buttonField)
    {
        buttonField.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(0.1f);
        buttonField.transform.localScale = Vector3.one;
    }

    public void ScrollRight()
    {
        UnhoverIndexedLetter();

        x++;
        if (x > width - 1)
            x = 0;

        HoverIndexedLetter();
    }

    public void ScrollLeft()
    {
        UnhoverIndexedLetter();

        x--;
        if (x < 0)
            x = width - 1;

        HoverIndexedLetter();
    }

    public void ScrollUp()
    {
        UnhoverIndexedLetter();

        y--;
        if (y < 0)
            y = height - 1;

        HoverIndexedLetter();
    }

    public void ScrollDown()
    {
        UnhoverIndexedLetter();

        y++;
        if (y > height - 1)
            y = 0;

        HoverIndexedLetter();
    }


    private void HoverIndexedLetter()
    {
        if (isLowercase)
        {
            lowercaseLetters[y, x].color = ColorPallete.selectedText;
            lowercaseLetters[y, x].transform.localScale *= selectLetterScaleFactor;
        }
        else
        {
            uppercaseLetters[y, x].color = ColorPallete.selectedText;
            uppercaseLetters[y, x].transform.localScale *= selectLetterScaleFactor;
        }
    }

    private void UnhoverIndexedLetter()
    {
        if (isLowercase)
        {
            lowercaseLetters[y, x].color = ColorPallete.unselectedText;
            lowercaseLetters[y, x].transform.localScale = Vector3.one;
        }
        else
        {
            uppercaseLetters[y, x].color = ColorPallete.unselectedText;
            uppercaseLetters[y, x].transform.localScale = Vector3.one;
        }
    }
   

    private void ToggleCasing()
    {

        if (isLowercase)
            ActivateUppercase();
        else
            ActivateLowercase();
    }

    private void ActivateUppercase()
    {
        UnhoverIndexedLetter();
        isLowercase = false;
        lowercaseRegion.SetActive(isLowercase);
        uppercaseRegion.SetActive(!isLowercase);
        HoverIndexedLetter();
    }

    private void ActivateLowercase()
    {
        UnhoverIndexedLetter();
        isLowercase = true;
        lowercaseRegion.SetActive(isLowercase);
        uppercaseRegion.SetActive(!isLowercase);
        HoverIndexedLetter();
    }

}
