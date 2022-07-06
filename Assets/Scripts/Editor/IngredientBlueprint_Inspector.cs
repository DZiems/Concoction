using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(IngredientBlueprint))]
public class IngredientBlueprint_Inspector : Editor
{
    private const string probabilityStr = "probability";
    private const string valueStr = "value";
    private const string isEnabledStr = "isEnabled";
    private const string isUniqueStr = "isUnique";
    private const string isGuaranteedStr = "isGuaranteed";

    private const string amountSelectedStr = "amountSelected";
    private const string contentStr = "content";

    private const int commonIndex = 0;
    private const int uncommonIndex = 1;
    private const int rareIndex = 2;
    private const int epicIndex = 3;
    private const int fabledIndex = 4;

    private ChanceObject<EffectBlueprint> defaultEffect = new(null);

    IngredientBlueprint blueprint;

    SerializedProperty rarityTierTableContent;

    SerializedProperty commonProbability;
    SerializedProperty commonEnabled;
    SerializedProperty uncommonProbability;
    SerializedProperty uncommonEnabled;
    SerializedProperty rareProbability;
    SerializedProperty rareEnabled;
    SerializedProperty epicProbability;
    SerializedProperty epicEnabled;
    SerializedProperty fabledProbability;
    SerializedProperty fabledEnabled;

    SerializedProperty baseTable;
    SerializedProperty baseTableAmount;
    SerializedProperty baseTableContent;
    ReorderableList baseTableROList;

    SerializedProperty commonTable;
    SerializedProperty commonTableAmount;
    SerializedProperty commonTableContent;
    ReorderableList commonTableROList;

    SerializedProperty uncommonTable;
    SerializedProperty uncommonTableAmount;
    SerializedProperty uncommonTableContent;
    ReorderableList uncommonTableROList;

    SerializedProperty rareTable;
    SerializedProperty rareTableAmount;
    SerializedProperty rareTableContent;
    ReorderableList rareTableROList;

    SerializedProperty epicTable;
    SerializedProperty epicTableAmount;
    SerializedProperty epicTableContent;
    ReorderableList epicTableROList;

    SerializedProperty fabledTable;
    SerializedProperty fabledTableAmount;
    SerializedProperty fabledTableContent;
    ReorderableList fabledTableROList;

    public void OnEnable()
    {
        blueprint = target as IngredientBlueprint;

        rarityTierTableContent = serializedObject.FindProperty("rarityRoller.tierTable.content");
        commonProbability = rarityTierTableContent.GetArrayElementAtIndex(commonIndex).FindPropertyRelative(probabilityStr);
        commonEnabled = rarityTierTableContent.GetArrayElementAtIndex(commonIndex).FindPropertyRelative(isEnabledStr);

        uncommonProbability = rarityTierTableContent.GetArrayElementAtIndex(uncommonIndex).FindPropertyRelative(probabilityStr);
        uncommonEnabled = rarityTierTableContent.GetArrayElementAtIndex(uncommonIndex).FindPropertyRelative(isEnabledStr);

        rareProbability = rarityTierTableContent.GetArrayElementAtIndex(rareIndex).FindPropertyRelative(probabilityStr);
        rareEnabled = rarityTierTableContent.GetArrayElementAtIndex(rareIndex).FindPropertyRelative(isEnabledStr);

        epicProbability = rarityTierTableContent.GetArrayElementAtIndex(epicIndex).FindPropertyRelative(probabilityStr);
        epicEnabled = rarityTierTableContent.GetArrayElementAtIndex(epicIndex).FindPropertyRelative(isEnabledStr);

        fabledProbability = rarityTierTableContent.GetArrayElementAtIndex(fabledIndex).FindPropertyRelative(probabilityStr);
        fabledEnabled = rarityTierTableContent.GetArrayElementAtIndex(fabledIndex).FindPropertyRelative(isEnabledStr);



        baseTable = serializedObject.FindProperty("effectRoller.baseTable");
        baseTableAmount = baseTable.FindPropertyRelative(amountSelectedStr);
        baseTableContent = baseTable.FindPropertyRelative(contentStr);
        baseTableROList = new ReorderableList(serializedObject, baseTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawBaseEffectsListHeader,
            drawElementCallback = DrawBaseEffectsListItems,
            onAddCallback = AddDefaultBaseEffect
        };

        commonTable = serializedObject.FindProperty("effectRoller.commonTable");
        commonTableAmount = commonTable.FindPropertyRelative(amountSelectedStr);
        commonTableContent = commonTable.FindPropertyRelative(contentStr);
        commonTableROList = new ReorderableList(serializedObject, commonTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawCommonEffectsListHeader,
            drawElementCallback = DrawCommonEffectsListItems,
            onAddCallback = AddDefaultCommonEffect
        };

        uncommonTable = serializedObject.FindProperty("effectRoller.uncommonTable");
        uncommonTableAmount = uncommonTable.FindPropertyRelative(amountSelectedStr);
        uncommonTableContent = uncommonTable.FindPropertyRelative(contentStr);
        uncommonTableROList = new ReorderableList(serializedObject, uncommonTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawUncommonEffectsListHeader,
            drawElementCallback = DrawUncommonEffectsListItems,
            onAddCallback = AddDefaultUncommonEffect
        };

        rareTable = serializedObject.FindProperty("effectRoller.rareTable");
        rareTableAmount = rareTable.FindPropertyRelative(amountSelectedStr);
        rareTableContent = rareTable.FindPropertyRelative(contentStr);
        rareTableROList = new ReorderableList(serializedObject, rareTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawRareEffectsListHeader,
            drawElementCallback = DrawRareEffectsListItems,
            onAddCallback = AddDefaultRareEffect
        };

        epicTable = serializedObject.FindProperty("effectRoller.epicTable");
        epicTableAmount = epicTable.FindPropertyRelative(amountSelectedStr);
        epicTableContent = epicTable.FindPropertyRelative(contentStr);
        epicTableROList = new ReorderableList(serializedObject, epicTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawEpicEffectsListHeader,
            drawElementCallback = DrawEpicEffectsListItems,
            onAddCallback = AddDefaultEpicEffect
        };

        fabledTable = serializedObject.FindProperty("effectRoller.fabledTable");
        fabledTableAmount = fabledTable.FindPropertyRelative(amountSelectedStr);
        fabledTableContent = fabledTable.FindPropertyRelative(contentStr);
        fabledTableROList = new ReorderableList(serializedObject, fabledTableContent, true, true, true, true)
        {
            elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
            drawHeaderCallback = DrawFabledEffectsListHeader,
            drawElementCallback = DrawFabledEffectsListItems,
            onAddCallback = AddDefaultFabledEffect
        };
    }


    public override void OnInspectorGUI()
    {
        // Load the real class values into the serialized copy
        serializedObject.Update();


        ShowScriptReference();

        EditorGUILayout.Space(24);

        MakeHeader("Ingredient Id Info", 0);
        ShowIngredientId();

        EditorGUILayout.Space(12);

        using (new EditorGUILayout.HorizontalScope())
        {
            MakeHeader("Rarity Tier Probabilities", 250);
            ShowRarityResetButton();
        }
        ShowRarityChances();

        EditorGUILayout.Space(12);
        MakeHeader("Base Effect Options", 0);
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(baseTableAmount);
        baseTableROList.DoLayoutList();


        MakeHeader("More Effects by Rarity (Higher Rarity Tiers Inherit from Those Lower)", 0);

        EditorGUILayout.PropertyField(commonTableAmount);
        commonTableROList.DoLayoutList();
        EditorGUILayout.PropertyField(uncommonTableAmount);
        uncommonTableROList.DoLayoutList();
        EditorGUILayout.PropertyField(rareTableAmount);
        rareTableROList.DoLayoutList();
        EditorGUILayout.PropertyField(epicTableAmount);
        epicTableROList.DoLayoutList();
        EditorGUILayout.PropertyField(fabledTableAmount);
        fabledTableROList.DoLayoutList();


        EditorGUILayout.Space(12);

        ApplyEffectsConstraints();

        // Write back changed values and evtl mark as dirty and handle undo/redo
        serializedObject.ApplyModifiedProperties();


    }







    private void AddDefaultBaseEffect(ReorderableList list)
    {
        baseTableContent.arraySize++;
        var newElement = baseTableContent.GetArrayElementAtIndex(baseTableContent.arraySize - 1);

        newElement.FindPropertyRelative(probabilityStr).floatValue = defaultEffect.probability;
        newElement.FindPropertyRelative(isEnabledStr).boolValue = false;
        newElement.FindPropertyRelative(isUniqueStr).boolValue = defaultEffect.isUnique;
    }
    private void AddDefaultCommonEffect(ReorderableList list)
    {
        commonTableContent.arraySize++;
        var newElement = commonTableContent.GetArrayElementAtIndex(commonTableContent.arraySize - 1);
        SetChanceObjectDefaults(newElement);
    }
    private void AddDefaultUncommonEffect(ReorderableList list)
    {
        uncommonTableContent.arraySize++;
        var newElement = uncommonTableContent.GetArrayElementAtIndex(uncommonTableContent.arraySize - 1);
        SetChanceObjectDefaults(newElement);
    }
    private void AddDefaultRareEffect(ReorderableList list)
    {
        rareTableContent.arraySize++;
        var newElement = rareTableContent.GetArrayElementAtIndex(rareTableContent.arraySize - 1);
        SetChanceObjectDefaults(newElement);
    }
    private void AddDefaultEpicEffect(ReorderableList list)
    {
        epicTableContent.arraySize++;
        var newElement = epicTableContent.GetArrayElementAtIndex(epicTableContent.arraySize - 1);
        SetChanceObjectDefaults(newElement);
    }
    private void AddDefaultFabledEffect(ReorderableList list)
    {
        fabledTableContent.arraySize++;
        var newElement = fabledTableContent.GetArrayElementAtIndex(fabledTableContent.arraySize - 1);
        SetChanceObjectDefaults(newElement);
    }

    private void SetChanceObjectDefaults(SerializedProperty newElement)
    {
        newElement.FindPropertyRelative(probabilityStr).floatValue = defaultEffect.probability;
        newElement.FindPropertyRelative(isEnabledStr).boolValue = defaultEffect.isEnabled;
        newElement.FindPropertyRelative(isUniqueStr).boolValue = defaultEffect.isUnique;
    }





    private void DrawBaseEffectsListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Base Effect Table (One of the Following Will Be Guaranteed)", EditorStyles.boldLabel);
    }
    private void DrawCommonEffectsListHeader(Rect rect)
    {
        SetGUIColorForRarity(RarityTier.Common);
        EditorGUI.LabelField(rect, "Common Effects Table", EditorStyles.boldLabel);
        ResetGUIColor();
    }
    private void DrawUncommonEffectsListHeader(Rect rect)
    {
        SetGUIColorForRarity(RarityTier.Uncommon);
        EditorGUI.LabelField(rect, "Uncommon Effects Table", EditorStyles.boldLabel);
        ResetGUIColor();
    }
    private void DrawRareEffectsListHeader(Rect rect)
    {
        SetGUIColorForRarity(RarityTier.Rare);
        EditorGUI.LabelField(rect, "Rare Effects Table", EditorStyles.boldLabel);
        ResetGUIColor();
    }
    private void DrawEpicEffectsListHeader(Rect rect)
    {
        SetGUIColorForRarity(RarityTier.Epic);
        EditorGUI.LabelField(rect, "Epic Effects Table", EditorStyles.boldLabel);
        ResetGUIColor();
    }
    private void DrawFabledEffectsListHeader(Rect rect)
    {
        SetGUIColorForRarity(RarityTier.Fabled);
        EditorGUI.LabelField(rect, "Fabled Effects Table", EditorStyles.boldLabel);
        ResetGUIColor();
    }



    private void DrawBaseEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty element = baseTableROList.serializedProperty.GetArrayElementAtIndex(index);
        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.baseTable.content[index], true);
    }
    private void DrawCommonEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty element = commonTableROList.serializedProperty.GetArrayElementAtIndex(index);
        bool shouldDisable = blueprint.effectRoller.commonTable.amountSelected == 0;
        if (shouldDisable)
            EditorGUI.BeginDisabledGroup(true);

        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.commonTable.content[index], false);

        if (shouldDisable)
            EditorGUI.EndDisabledGroup();
    }
    private void DrawUncommonEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        bool shouldDisable = blueprint.effectRoller.uncommonTable.amountSelected == 0;
        if (shouldDisable)
            EditorGUI.BeginDisabledGroup(true);

        SerializedProperty element = uncommonTableROList.serializedProperty.GetArrayElementAtIndex(index);
        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.uncommonTable.content[index], false);

        if (shouldDisable)
            EditorGUI.EndDisabledGroup();
    }
    private void DrawRareEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty element = rareTableROList.serializedProperty.GetArrayElementAtIndex(index);

        bool shouldDisable = blueprint.effectRoller.rareTable.amountSelected == 0;
        if (shouldDisable)
            EditorGUI.BeginDisabledGroup(true);

        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.rareTable.content[index], false);

        if (shouldDisable)
            EditorGUI.EndDisabledGroup();
    }
    private void DrawEpicEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        bool shouldDisable = blueprint.effectRoller.epicTable.amountSelected == 0;
        if (shouldDisable)
            EditorGUI.BeginDisabledGroup(true);

        SerializedProperty element = epicTableROList.serializedProperty.GetArrayElementAtIndex(index);
        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.epicTable.content[index], false);

        if (shouldDisable)
            EditorGUI.EndDisabledGroup();
    }
    private void DrawFabledEffectsListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        bool shouldDisable = blueprint.effectRoller.fabledTable.amountSelected == 0;
        if (shouldDisable)
            EditorGUI.BeginDisabledGroup(true);

        SerializedProperty element = fabledTableROList.serializedProperty.GetArrayElementAtIndex(index);
        DrawChanceTableContentElement(rect, element, blueprint.effectRoller.fabledTable.content[index], false);

        if (shouldDisable)
            EditorGUI.EndDisabledGroup();
    }




    private void DrawChanceTableContentElement(Rect rect, SerializedProperty element, ChanceObject<EffectBlueprint> effectChanceObjectRef, bool isBaseTable)
    {
        SerializedProperty blueprintField = element.FindPropertyRelative(valueStr);
        SerializedProperty probabilityField = element.FindPropertyRelative(probabilityStr);
        SerializedProperty isEnabledField = element.FindPropertyRelative(isEnabledStr);
        SerializedProperty isUniqueField = element.FindPropertyRelative(isUniqueStr);
        SerializedProperty isGuaranteedField = element.FindPropertyRelative(isGuaranteedStr);

        float offsetX = 0;
        float offsetY = 0;

        if (isBaseTable)
            effectChanceObjectRef.isEnabled = !effectChanceObjectRef.IsNull;

        //ScriptableObject blueprint field
        if (isBaseTable)
            DrawField(blueprintField, rect, offsetX, offsetY, rect.width / 2);
        else
            using (new EditorGUI.DisabledScope(!effectChanceObjectRef.isEnabled))
                DrawField(blueprintField, rect, offsetX, offsetY, rect.width / 2);

        offsetX += (rect.width / 2) + 20;


        using (new EditorGUI.DisabledScope(!effectChanceObjectRef.isEnabled || effectChanceObjectRef.isGuaranteed))
        {
            //float probability field
            DrawFieldLabel("Probability: ", rect, offsetX, offsetY, 70);
            offsetX += 70;
            DrawField(probabilityField, rect, offsetX, offsetY, rect.width / 8);
            offsetX += (rect.width / 8) + 20;
        }

        offsetX = 0;
        offsetY = EditorGUIUtility.singleLineHeight * 1.1f;


        //bool isEnabled
        using (new EditorGUI.DisabledGroupScope(isBaseTable))
        {
            DrawFieldLabel("Enabled:", rect, offsetX, offsetY, 60);
            offsetX += 60;
            DrawField(isEnabledField, rect, offsetX, offsetY, rect.width / 5);
            offsetX += rect.width / 5;
        }


        //non-null elements must be unique
        using (new EditorGUI.DisabledScope(true))
        {
            //bool isUnique
            DrawFieldLabel("Unique:", rect, offsetX, offsetY, 60);
            offsetX += 60;
            DrawField(isUniqueField, rect, offsetX, offsetY, rect.width / 5);
            offsetX += rect.width / 5;

        }

        using (new EditorGUI.DisabledScope(isBaseTable || !effectChanceObjectRef.isEnabled))
        {
            //bool isGuaranteed
            DrawFieldLabel("Guaranteed:", rect, offsetX, offsetY, 80);
            offsetX += 80;
            DrawField(isGuaranteedField, rect, offsetX, offsetY, rect.width / 5);
            offsetX += rect.width / 5;
        }
;
    }

    private void DrawField(SerializedProperty field, Rect rect, float offsetX, float offsetY, float width)
    {
        EditorGUI.PropertyField(
                  new Rect(rect.x + offsetX, rect.y + offsetY, width, EditorGUIUtility.singleLineHeight),
                  field,
                  GUIContent.none
                  );
    }

    private void DrawFieldLabel(string label, Rect rect, float offsetX, float offsetY, float width)
    {
        EditorGUI.LabelField(new Rect(rect.x + offsetX, rect.y + offsetY, width, EditorGUIUtility.singleLineHeight), label);
    }

    private static void ResetGUIColor()
    {
        GUI.color = Color.white;
    }

    private void SetGUIColorForRarity(RarityTier rarityTier)
    {
        switch (rarityTier)
        {
            case RarityTier.Common:
                GUI.color = ColorPallete.commonRarity_Editor;
                break;
            case RarityTier.Uncommon:
                GUI.color = ColorPallete.uncommonRarity_Editor;
                break;
            case RarityTier.Rare:
                GUI.color = ColorPallete.rareRarity_Editor;
                break;
            case RarityTier.Epic:
                GUI.color = ColorPallete.epicRarity_Editor;
                break;
            case RarityTier.Fabled:
                GUI.color = ColorPallete.fabledRarity_Editor;
                break;
            default:
                GUI.color = Color.white;
                break;
        }
    }



    private void ApplyEffectsConstraints()
    {
        foreach (var effect in blueprint.effectRoller.baseTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }
        foreach (var effect in blueprint.effectRoller.commonTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }
        foreach (var effect in blueprint.effectRoller.uncommonTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }
        foreach (var effect in blueprint.effectRoller.rareTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }
        foreach (var effect in blueprint.effectRoller.epicTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }
        foreach (var effect in blueprint.effectRoller.fabledTable.content)
        {
            if (effect.probability < 1f)
                effect.probability = 1f;

            effect.isUnique = !effect.IsNull;
        }

        if (blueprint.effectRoller.baseTable.amountSelected < 0)
            blueprint.effectRoller.baseTable.amountSelected = 0;
        if (blueprint.effectRoller.commonTable.amountSelected < 0)
            blueprint.effectRoller.commonTable.amountSelected = 0;
        if (blueprint.effectRoller.uncommonTable.amountSelected < 0)
            blueprint.effectRoller.uncommonTable.amountSelected = 0;
        if (blueprint.effectRoller.rareTable.amountSelected < 0)
            blueprint.effectRoller.rareTable.amountSelected = 0;
        if (blueprint.effectRoller.epicTable.amountSelected < 0)
            blueprint.effectRoller.epicTable.amountSelected = 0;
        if (blueprint.effectRoller.fabledTable.amountSelected < 0)
            blueprint.effectRoller.fabledTable.amountSelected = 0;

    }


    private void ShowRarityResetButton()
    {
        if (GUILayout.Button("Reset Rarity Tier Defaults"))
            blueprint.rarityRoller = new RarityTierRoller();
    }


    //https://docs.unity3d.com/ScriptReference/EditorGUILayout.PropertyField.html
    private void ShowRarityChances()
    {
        using (new EditorGUILayout.VerticalScope())
        {
            EditorGUI.indentLevel++;
            GUILayout.FlexibleSpace();
            ShowRarityFields("Common", commonProbability, commonEnabled,
                !blueprint.rarityRoller.Common.isEnabled,
                RarityTier.Common);
            ShowRarityFields("Uncommon", uncommonProbability, uncommonEnabled,
                !blueprint.rarityRoller.Uncommon.isEnabled,
                RarityTier.Uncommon);
            ShowRarityFields("Rare", rareProbability, rareEnabled,
                !blueprint.rarityRoller.Rare.isEnabled,
                RarityTier.Rare);
            ShowRarityFields("Epic", epicProbability, epicEnabled,
                !blueprint.rarityRoller.Epic.isEnabled,
                RarityTier.Epic);
            ShowRarityFields("Fabled", fabledProbability, fabledEnabled,
                !blueprint.rarityRoller.Fabled.isEnabled,
                RarityTier.Fabled);
            EditorGUI.indentLevel--;
        }


        ApplyRarityConstraints();
    }

    private void ShowRarityFields(string title, SerializedProperty probabilityField, SerializedProperty enabledField, bool disabledCondition, RarityTier rarityTier)
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUIUtility.labelWidth = 100;
            EditorGUIUtility.fieldWidth = 100;
            SetGUIColorForRarity(rarityTier);
            using (new EditorGUI.DisabledScope(disabledCondition))
            {
                EditorGUILayout.PropertyField(probabilityField, new GUIContent(title));
            }
            ResetGUIColor();


            EditorGUIUtility.labelWidth = 80;
            EditorGUILayout.PropertyField(enabledField, new GUIContent("Enabled: "));

            EditorGUIUtility.labelWidth = 0;
            EditorGUIUtility.fieldWidth = 0;
            GUILayout.FlexibleSpace();
        }
    }

    private void ApplyRarityConstraints()
    {
        //set minimum rarity to 1
        if (blueprint.rarityRoller.Common.probability < 1f)
            blueprint.rarityRoller.Common.probability = 1f;

        if (blueprint.rarityRoller.Uncommon.probability < 1f)
            blueprint.rarityRoller.Uncommon.probability = 1f;

        if (blueprint.rarityRoller.Rare.probability < 1f)
            blueprint.rarityRoller.Rare.probability = 1f;

        if (blueprint.rarityRoller.Epic.probability < 1f)
            blueprint.rarityRoller.Epic.probability = 1f;

        if (blueprint.rarityRoller.Fabled.probability < 1f)
            blueprint.rarityRoller.Fabled.probability = 1f;

    }

    private void MakeHeader(string text, float labelWidth)
    {
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        EditorGUIUtility.labelWidth = 0;
    }

    private void ShowIngredientId()
    {
        blueprint.id = EditorGUILayout.ObjectField("Ingredient Id", blueprint.id, typeof(IngredientId), false) as IngredientId;

        if (blueprint.id != null)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.indentLevel++; ;
                EditorGUILayout.LabelField($"Name Id: \t{blueprint.id.stringId}");
                EditorGUILayout.LabelField($"Region: \t\t{blueprint.id.region}");
                EditorGUILayout.LabelField($"Taxonomy: \t{blueprint.id.taxonomy}");
                EditorGUI.indentLevel--;
            }
        }
    }


    //https://answers.unity.com/questions/1223009/how-to-show-the-standard-script-line-with-a-custom.html
    private void ShowScriptReference()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            var monoScript = MonoScript.FromScriptableObject((UnityEngine.ScriptableObject)target);
            EditorGUILayout.ObjectField("Script", monoScript, GetType(), false);
        }
    }


}

