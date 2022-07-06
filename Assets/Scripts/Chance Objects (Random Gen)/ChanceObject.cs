using System.Collections;
using UnityEngine;

[System.Serializable]
public class ChanceObject<T>
{
    [SerializeField] private T value;
    public float probability = 10;
    public bool isEnabled = true;
    public bool isUnique = true;
    public bool isGuaranteed = false;

    public ChanceObject(T value, float probability = 10f, bool isEnabled = true, bool isUnique = true, bool isGuaranteed = false)
    {
        this.value = value;
        this.probability = probability;
        this.isEnabled = isEnabled;
        this.isUnique = isUnique;
        this.isGuaranteed = isGuaranteed;
    }

    public T Value() => value;

    public bool IsNull => Value() == null;
}
