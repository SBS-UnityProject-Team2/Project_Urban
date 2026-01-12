using UnityEngine;

public class SuperConducter : BuffCard
{   
    [SerializeField] private int Kineticturn;
    public override CardName Name => CardName.Superconducter;

    public override int Use(Target target)
    {   
        Player player = target as Player; 

        target.KineticVeil(Kineticturn);
        target.Frozen(turns);
        target.SuperConduct(turns);
        
        return cost;
    }
}