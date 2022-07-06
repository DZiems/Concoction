[System.Serializable]
public class EffectData
{ 
    public SerializableDictionary<string, float> parameters;

    public EffectId id;

    public EffectData(EffectId id, SerializableDictionary<string, float> parameters)
    {
        this.id = id;
        this.parameters = parameters;
    }
}