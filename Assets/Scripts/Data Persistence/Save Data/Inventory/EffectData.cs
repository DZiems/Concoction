[System.Serializable]
public class EffectData
{
    public EffectGroup group;
    public EffectID type;

    public SerializableDictionary<string, float> parameters;

    public EffectData(EffectGroup group, EffectID type, SerializableDictionary<string, float> parameters)
    {
        this.group = group;
        this.type = type;
        this.parameters = parameters;
    }
}