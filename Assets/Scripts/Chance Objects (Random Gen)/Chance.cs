using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IMPORTANT TODO: INCLUDE FUNCTIONALITY FOR ISGUARANTEED

public static class Chance<T>
{
    public static T Roll(ChanceObject<T>[] chanceObjects) 
    {
        float hitChance = RollAHitChance_NoUniques(chanceObjects);
        //roll a ChanceObject<T> from ChanceContents
        float runningValue = 0f;
        foreach (var co in chanceObjects)
        {
            if (!CanBeRolled_NoUniques(co))
                continue;

            runningValue += co.probability;
            if (runningValue >= hitChance)
            {
                return co.Value();
            }
        }

        Debug.LogError("Reaching here means every single ChanceObject in a roll was disabled (that, or there's a bug).");
        //TODO: maybe find a better way of handling
        throw new System.ArgumentOutOfRangeException();
    }

    public static List<T> Roll(ChanceTable<T> table)
    {
        System.Diagnostics.Debug.Assert(table.content != null && table.content.Length > 0);

        List<T> result = new();
        HashSet<ChanceObject<T>> uniques = new();

        //guaranteed objects
        foreach (var co in table.content)
            if (co.isGuaranteed && co.isEnabled)
                HandleAddToResult(result, uniques, co);

        //remaining chance objects
        int numRemainingToRoll = table.amountSelected - result.Count;
        if (numRemainingToRoll <= 0)
            return result;

        float runningNumber;
        for (int i = 0; i < numRemainingToRoll; i++)
        {
            float hitNumber = RollAHitNumber(table.content, uniques); 
            if (hitNumber <= 0)
                return result;

            runningNumber = 0f;
            foreach (var co in table.content)
            {
                if (CanBeRolled(co, uniques))
                    runningNumber += co.probability;
                //chance has hit!
                if (runningNumber >= hitNumber)
                {
                    HandleAddToResult(result, uniques, co);
                    break;
                }
            }
        }


        return result;
    }





    /**********************
     * Singular Roll Case *
     **********************/

    /// <summary> Is it enabled?</summary>
    private static bool CanBeRolled_NoUniques(ChanceObject<T> chanceObject)
    {
        return chanceObject.isEnabled;
    }

    /// <summary> Random float between 0 and Sum(ChanceContents.Where(e => CanHit(e)). </summary>
    private static float RollAHitChance_NoUniques(ChanceObject<T>[] content)
    {
        float probabilitySum = 0;

        foreach (var co in content)
        {
            if (CanBeRolled_NoUniques(co))
                probabilitySum += co.probability;
        }

        return Random.Range(0f, probabilitySum);

    }


    /**********************
     * Multiple Roll Case *
     **********************/

    private static bool CanBeRolled(ChanceObject<T> co, HashSet<ChanceObject<T>> uniques)
    {
        if (!co.isEnabled)
            return false;

        //guaranteed objects will already have been taken care of
        if (co.isGuaranteed)
            return false;

        if (co.isUnique)
            return !uniques.Contains(co);
        else
            return true;
    }

    /// <summary> Random float between 0 and Sum(ChanceContents.Where(e => CanHit(e)). </summary>
    private static float RollAHitNumber(ChanceObject<T>[] content, HashSet<ChanceObject<T>> uniques)
    {
        float probabilitySum = 0;

        foreach (var co in content)
        {
            if (CanBeRolled(co, uniques))
                probabilitySum += co.probability;
        }

        return Random.Range(0f, probabilitySum);
    }


    private static void HandleAddToResult(List<T> result, HashSet<ChanceObject<T>> uniques, ChanceObject<T> co)
    {
        if (co.isUnique)
            uniques.Add(co);

        if (!co.IsNull)
            result.Add(co.Value());
    }





    /********************************
     * Recursive Multiple Roll Case *
     ********************************/

    /*
    private static void RecursiveAddToResult(List<T> result, ChanceObject<T> co)
    {
        if (co is RecursiveChanceTable<T> chanceTable)
        {
            var val = chanceTable.Value();
            if (val.Count > 0)
                result.AddRange(val);
        }
        else
        {
            if (!co.IsNull)
                result.Add(co.Value());
        }
    }
    */
}
