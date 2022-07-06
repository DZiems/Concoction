
[System.Serializable]
public class InventoryData
{
    public IngredientData[] ingredientDatas;
    public ulong[] ingredientDataOrdersAcquired;
    public ulong numIngredientsAcquired;

    public InventoryData()
    {
        this.ingredientDatas = new IngredientData[0];
        this.numIngredientsAcquired = 0;
    }

    public InventoryData(IngredientData[] ingredientDatas, ulong[] ordersIngredientsAcquired, ulong numIngredientsAcquired)
    {
        this.ingredientDatas = ingredientDatas;
        this.ingredientDataOrdersAcquired = ordersIngredientsAcquired;
        this.numIngredientsAcquired = numIngredientsAcquired;
    }
}