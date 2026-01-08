public class Status
{
    private int attack;
    private int armor;
    private int dummy;
    private int additionalCost;
    
    public int Attack => attack;
    public int Armor => armor;
    public int Dummy => dummy;
    public int AdditionalCost => additionalCost;

    public bool IsBlock { get; set; }
    public bool IsBroken { get; set; }
    public bool IsFrozen { get; set; }
    public bool IsAnointed { get; set; }
    public bool IsDelirium { get; set; }
    public bool IsInfested { get; set; }

    public void IncreaseAttack(int amount)
    {
        attack += amount;
    }

    public void DecreaseAttack(int amount)
    {
        attack -= amount;
        if (attack < 0)
            attack = 0;
    }

    public void IncreaseArmor(int amount)
    {
        armor += amount;
    }

    public void DecreaseArmor(int amount)
    {
        armor -= amount;
        if (armor < 0)
            armor = 0;
    }

    public void IncreaseDummy(int amount)
    {
        dummy += amount;
    }

    public void DecreaseDummy(int amount)
    {
        dummy -= amount;

        if (dummy < 0)
            dummy = 0;
    }
}