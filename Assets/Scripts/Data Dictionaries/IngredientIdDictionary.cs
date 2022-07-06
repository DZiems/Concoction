using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientIdDictionary : MonoBehaviour
{
    //TODO: instead of a serializeField, just find all created ScriptableObject Ingredient Ids in the project directory.
    [SerializeField] List<IngredientId> allIngredientIds;

    public Dictionary<string, IngredientId> idDictionary { get; private set; }

    private void Awake()
    {
        idDictionary = new Dictionary<string,IngredientId>();
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        foreach (var ingredientId in allIngredientIds)
            idDictionary.Add(ingredientId.stringId, ingredientId);  

        foreach (var item in idDictionary)
        {
            Debug.Log($"{item.Key} : {item.Value}");
        }
    }

    public IngredientId this[string nameId]
    {
        get { return idDictionary[nameId]; }
    }

}
