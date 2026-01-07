public class Reinforce : ModifierEffect
{
    public override void Modify(Status status, int count)
    {
        status.IncreaseAttack(count);
        stack += count;
    }
}