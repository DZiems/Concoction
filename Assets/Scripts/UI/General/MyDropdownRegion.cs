using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDropdownRegion : MonoBehaviour
{
    public List<MyDropdownField> fields = new List<MyDropdownField>();

    public float Width { get; private set; }
    public float Height { get; private set; }

    private void Awake()
    {
        StartCoroutine(SetDimensionAccessors());
    }

    private IEnumerator SetDimensionAccessors()
    {
        yield return new WaitForEndOfFrame();

        var rect = GetComponent<RectTransform>().rect;
        Width = rect.width;
        Height = rect.height;
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
