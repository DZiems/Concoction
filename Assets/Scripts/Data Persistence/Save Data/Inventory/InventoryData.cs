
[System.Serializable]
public class InventoryData
{
    public IngredientData[] ingredientDatas;

    public InventoryData()
    {
        this.ingredientDatas = new IngredientData[0];
    }

    public InventoryData(IngredientData[] ingredientDatas)
    {
        this.ingredientDatas = ingredientDatas;
    }
}