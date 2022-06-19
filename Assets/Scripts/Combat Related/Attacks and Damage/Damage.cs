using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Damage
{ 
    public float physical;
    public float magical;
    public float trueDmg;

    public Damage(float physicalDmg, float magicalDmg, float trueDmg)
    {
        this.physical = physicalDmg;
        this.magical = magicalDmg;
        this.trueDmg = trueDmg;
    }

    public static Damage operator *(Damage lhs, float rhs) =>
        new Damage(lhs.physical * rhs, lhs.magical * rhs, lhs.trueDmg * rhs);

    public static Damage operator /(Damage lhs, float rhs) =>
        new Damage(lhs.physical / rhs, lhs.magical / rhs, lhs.trueDmg / rhs);

    public static Damage operator +(Damage lhs, Damage rhs) =>
        new Damage(lhs.physical + rhs.physical, lhs.magical + rhs.magical, lhs.trueDmg + rhs.trueDmg);

    public static bool operator <(Damage lhs, float rhs) => rhs >= (lhs.physical + lhs.magical + lhs.trueDmg);

    public static bool operator >(Damage lhs, float rhs) => rhs <= (lhs.physical + lhs.magical + lhs.trueDmg);


    //https://leagueoflegends.fandom.com/wiki/Damage
    //see Calculating Damage
    public float EvaluateActualDamageAmount(float defense, float resistance)
    {
        float result = 0;

        result += defense >= 0 ?
            physical * (100f / (100f + defense)) :
            physical * (2 - (100f / (100f - defense)));

        result += resistance >= 0 ?
            magical * (100f / (100f + resistance)) :
            magical * (2 - (100f / (100f - resistance)));

        result += trueDmg;

        return result;
    }

    public override string ToString()
    {
        return $"({physical}, {magical}, {trueDmg})";
    }
}
