using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyDropdownField : MonoBehaviour
{
    [SerializeField] private GameObject deletionField;

    public float Height { get; private set; }

    public TextMeshProUGUI ProfileTxt { get; private set; }
    public Image BackgroundImg { get; private set; }
    public Animator Anim { get; private set; }
    public TextMeshProUGUI DeletePressedText { get; private set; }
    public GameObject DeletionField => deletionField;

    private void Awake()
    {
        Height = GetComponent<RectTransform>().rect.height;


        ProfileTxt = GetComponentInChildren<TextMeshProUGUI>();
        BackgroundImg = GetComponent<Image>();
        Anim = GetComponent<Animator>();

        DeletePressedText = deletionField.GetComponentInChildren<TextMeshProUGUI>();
    }


}
