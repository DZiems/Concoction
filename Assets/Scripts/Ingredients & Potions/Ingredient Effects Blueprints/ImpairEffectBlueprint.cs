using UnityEngine;

public abstract class ImpairEffectBlueprint : ScriptableObject
{
    public abstract Impairment Generate();
}