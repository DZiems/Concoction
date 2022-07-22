using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDropdownRegion : MonoBehaviour
{
    [SerializeField] private RectTransform dropdownRectTransform;
    public List<MyDropdownField> fields = new List<MyDropdownField>();

    public float Width { get; private set; }
    public float Height { get; private set; }


    private void Start()
    {

        StartCoroutine(SetDimensions());
    }

    private IEnumerator SetDimensions()
    {
        yield return new WaitForEndOfFrame();


        var rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -dropdownRectTransform.rect.height);

        Width = rectTransform.rect.width;
        Height = rectTransform.rect.height;
    }

    public void Clear()
    {
        foreach (var field in fields)
        {
            Destroy(field.gameObject);
        }
        fields.Clear();
    }
}
