
[System.Serializable]
public class Shield
{
    public float amount;
    public float defense;
    public float resistance;

    public float rechargeSpeed;
    public float rechargeDelay;

    public Shield (float amount, float defense, float resistance, float rechargeSpeed, float rechargeDelay)
    {
        this.amount = amount;
        this.defense = defense;
        this.resistance = resistance;

        this.rechargeSpeed = rechargeSpeed;
        this.rechargeDelay = rechargeDelay;
    }

    public Shield()
    {
        amount = 0;
        defense = 0;
        resistance = 0;

        rechargeSpeed = 0;
        rechargeDelay = 0;
    }

    public static Shield operator +(Shield lhs, Shield rhs)
    {
        if (lhs.amount == 0 && rhs.amount == 0) return new Shield();

        float newAmount = lhs.amount + rhs.amount;

        float newDef = ((lhs.defense * lhs.amount) + (rhs.defense * rhs.amount)) / newAmount; 
        float newRes = ((lhs.resistance * lhs.amount) + (rhs.resistance * rhs.amount)) / newAmount;

        float newSpeed = ((lhs.rechargeSpeed * lhs.amount) + (rhs.rechargeSpeed * rhs.amount)) / newAmount;
        float newDelay = ((lhs.rechargeDelay * lhs.amount) + (rhs.rechargeDelay * rhs.amount)) / newAmount;


        return new Shield(newAmount, newDef, newRes, newSpeed, newDelay);
    }
    public static Shield operator -(Shield lhs, Shield rhs)
    {
        float newAmount = lhs.amount - rhs.amount;
        if (newAmount <= 0) return new Shield();

        float newDef = ((lhs.defense * lhs.amount) - (rhs.defense * rhs.amount)) / newAmount;
        float newRes = ((lhs.resistance * lhs.amount) - (rhs.resistance * rhs.amount)) / newAmount;

        float newSpeed = ((lhs.rechargeSpeed * lhs.amount) - (rhs.rechargeSpeed * rhs.amount)) / newAmount;
        float newDelay = ((lhs.rechargeDelay * lhs.amount) - (rhs.rechargeDelay * rhs.amount)) / newAmount;

        return new Shield(newAmount, newDef, newRes, newSpeed, newDelay);
    }

    public override string ToString()
    {
        return $"(pts: {amount}, def: {defense}, res: {resistance}, rechargeSpeed: {rechargeSpeed}, rechargeDelay: {rechargeDelay})";
    }
}
