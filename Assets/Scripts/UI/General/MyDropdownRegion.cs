using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyDropdownRegion : MonoBehaviour
{
    public List<MyDropdownField> fields = new List<MyDropdownField>();

    public void Clear()
    {
        foreach (var field in fields)
        {
            Destroy(field.gameObject);
        }
        fields.Clear();
    }
}
