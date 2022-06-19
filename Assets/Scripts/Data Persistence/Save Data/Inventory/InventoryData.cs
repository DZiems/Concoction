
[System.Serializable]
public class InventoryData
{
    public PotionData[] potionDatas;
    public IngredientData[] ingredientDatas;

    public InventoryData()
    {
        this.potionDatas = new PotionData[0];
        this.ingredientDatas = new IngredientData[0];
    }

    public InventoryData(PotionData[] potionDatas, IngredientData[] ingredientDatas)
    {
        this.potionDatas = potionDatas;
        this.ingredientDatas = ingredientDatas;
    }
}