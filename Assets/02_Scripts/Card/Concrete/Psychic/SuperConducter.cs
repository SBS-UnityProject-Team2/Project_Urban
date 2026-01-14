using UnityEngine;

public class SuperConducter : BuffCard
{   
    [SerializeField] private int Kineticturn;
    public override CardName Name => CardName.Superconducter;

    public override int Use(Player player, Target target)
    {   

        target.KineticVeil(Kineticturn);
        target.Frozen(turns);
        target.Nullification(turns);
        
        return curCost;
    }
}