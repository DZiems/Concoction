using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewProfilePanel : MonoBehaviour
{
    [SerializeField] private GameObject deleteField;
    [SerializeField] private GameObject selectField;
    [SerializeField] private GameObject shiftField;


    private TextMeshProUGUI profileLabel;
    public AlphabetField alphabetField { get; private set; }
    private CanvasGroup canvasGroup;

    private string profileName;
    private const string enterNamePrompt = "Enter a name...";


    public string ProfileName => profileName;

    private void Awake()
    {

        profileLabel = GetComponentInChildren<TextMeshProUGUI>();
        alphabetField = GetComponentInChildren<AlphabetField>();
        canvasGroup = GetComponent<CanvasGroup>();

        profileName = string.Empty;

    }

    private void Start()
    {
        Hide();
    }

    public void AddLetter()
    {
        profileName += alphabetField.CurrentLetter;

        UpdateLabel();
    }

    public void DeleteLetter()
    {
        if (!string.IsNullOrEmpty(profileName))
            profileName = profileName.Remove(profileName.Length - 1);

        UpdateLabel();
    }

    public void UpdateLabel()
    {
        if (string.IsNullOrEmpty(profileName))
        {
            profileLabel.SetText(enterNamePrompt);
            profileLabel.color = ColorPallete.inactiveColor;
        }
        else
        {
            profileLabel.SetText(profileName);
            profileLabel.color = ColorPallete.selectedColor;
        }
    }

    public void Show()
    {
        alphabetField.Reset();
        UpdateLabel();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        profileName = string.Empty;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void FlashDelete()
    {
        StartCoroutine(PressButtonCoroutine(deleteField));
    }
    public void FlashSelect()
    {
        StartCoroutine(PressButtonCoroutine(selectField));
    }

    public void FlashShift()
    {
        StartCoroutine(PressButtonCoroutine(shiftField));
    }

    private IEnumerator PressButtonCoroutine(GameObject buttonField)
    {
        buttonField.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(0.1f);
        buttonField.transform.localScale = Vector3.one;
    }

}
