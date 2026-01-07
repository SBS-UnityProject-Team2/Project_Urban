public class Armor : ModifierEffect
{
    public override void Modify(Status status, int count)
    {
        status.IncreaseArmor(count);
        stack += count;
    }
}