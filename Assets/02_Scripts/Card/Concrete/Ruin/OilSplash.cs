using UnityEngine;

public class OilSplash : Debuff
{
    public override CardName Name => CardName.OilSplash;

    public override int Use(Player player, Target target)
    {
        Burn burn = target.Status.Burn;

        // 2. 화상이 걸려있다면
        if (burn.IsActive)
            target.Burn(burn.Count * 2);            

        return curCost;
    }
}