using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDictionaryManager : MonoBehaviour
{
    public static DataDictionaryManager Instance { get; private set; }
    public IngredientIdDictionary IngredientIds { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one DataDictionaryManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        IngredientIds = GetComponentInChildren<IngredientIdDictionary>();

        DontDestroyOnLoad(gameObject);
    }


}
