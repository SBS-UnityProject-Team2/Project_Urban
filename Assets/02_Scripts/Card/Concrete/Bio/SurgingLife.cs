using UnityEngine;

public class SurgingLife : BuffCard
{   
    [SerializeField] private int count = 5;
    public override CardName Name => CardName.SurgingLife;

    public override int Use(Target target)
    {   
        target.Regeneration(count);
        return cost;
    }
}