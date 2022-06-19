using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{

    public EffectGroup Group { get; protected set; }
    public EffectID Type { get; protected set; }

    public abstract void RunEffect(Entity receiver, Entity dealer);

    public abstract EffectData GetData();


    public static readonly string durationKey = "duration";

    public static readonly string multiplierKey = "multiplier";

    public static readonly string percentageKey = "percentage";

    public static readonly string healAmountKey = "healAmount";
    public static readonly string tickTimeKey = "tickTime";

    public static readonly string shieldAmountKey = "shieldAmount";
    public static readonly string shieldDefenseKey = "shieldDefense";
    public static readonly string shieldResistanceKey = "shieldResistance";
    public static readonly string shieldRechargeSpeedKey = "shieldRechargeSpeed";
    public static readonly string shieldRechargeDelayKey = "shieldRechargeDelay";

    public static readonly string physicalThornsFactorKey = "physicalThornsFactor";
    public static readonly string magicalThornsFactorKey = "magicalThornsFactor";
    public static readonly string trueDmgThornsFactorKey = "trueDmgThornsFactor";

    public static readonly string damagePhysicalKey = "damagePhysical";
    public static readonly string damageMagicalKey = "damageMagical";
    public static readonly string damageTrueKey = "damageTrue";
}


public abstract class Enhancement : Effect
{
    
}
public abstract class Impairment : Effect
{

}


