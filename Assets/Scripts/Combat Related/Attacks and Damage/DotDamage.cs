using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamage : Damage
{
    public float tickTime { get; private set; }
    public float duration { get; private set; }
    public Damage dmgRemainder { get; private set; }
    public int numTicks { get; private set; }


    public DotDamage(float physicalDmg, float magicalDmg, float trueDmg, float duration, float tickTime) : base(physicalDmg, magicalDmg, trueDmg)
    {
        this.tickTime = tickTime;
        this.duration = duration;

        float numTicksWithRemainder = duration / tickTime;
        numTicks = (int)numTicksWithRemainder;
        dmgRemainder = new Damage(physicalDmg, magicalDmg, trueDmg) * (numTicksWithRemainder - numTicks);
    }



    private const float durationAtWhichFlatDmgDoubled = 16f;
    private const float tickTimeAtWhichFlatDmgDoubled = 8f;
    /// <summary>
    /// This indicates how much of a buff a DoT should receive based on how strung out it is across a duration.
    /// </summary>
    /// <returns>A float amount that should apply each DotDamage.period across a DotDamage.duration</returns>
    public static DotDamage FlatToDotConversion(Damage flatDmg, float duration, float tickTime)
    {
        if (tickTime <= 0f)
        {
            Debug.LogError("DotDamage has a ticktime of zero, which would lead to infinite damage...");
            return new DotDamage(0f, 0f, 0f, 0f, 1f);
        }

        if (duration <= 0f)
        {
            Debug.LogError("DotDamage has a duration of zero, which makes it basically flat damage.");
            return new DotDamage(flatDmg.physical, flatDmg.magical, flatDmg.trueDmg, 0f, 1f);
        }

        float durationMultiplier = 1 + (duration / durationAtWhichFlatDmgDoubled);
        float tickTimeMultiplier = 1 + (tickTime / tickTimeAtWhichFlatDmgDoubled);

        int numTicks = (int)(duration / tickTime);

        if (numTicks <= 0)
        {
            return new DotDamage(flatDmg.physical, flatDmg.magical, flatDmg.trueDmg, duration, tickTime) * tickTimeMultiplier;
        }

        float multiplier = durationMultiplier * tickTimeMultiplier / numTicks;
        DotDamage result = new DotDamage(flatDmg.physical, flatDmg.magical, flatDmg.trueDmg, duration, tickTime) * multiplier;

        return result;
    }



    public static DotDamage operator *(DotDamage lhs, float rhs) =>
        new DotDamage(lhs.physical * rhs, lhs.magical * rhs, lhs.trueDmg * rhs, lhs.duration, lhs.tickTime);

    public static DotDamage operator /(DotDamage lhs, float rhs) =>
        new DotDamage(lhs.physical / rhs, lhs.magical / rhs, lhs.trueDmg / rhs, lhs.duration, lhs.tickTime);


    public override string ToString()
    {
        return $"({physical}, {magical}, {trueDmg}, each {tickTime}s for {duration}s)";
    }
}
