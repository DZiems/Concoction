using System.Collections.Generic;

[System.Serializable]
public class ChanceTable<T>
{
    public int amountSelected;

    public ChanceObject<T>[] content;

    public ChanceTable(int amountSelected, ChanceObject<T>[] content)
    {
        this.amountSelected = amountSelected;
        this.content = content;
    }
}

