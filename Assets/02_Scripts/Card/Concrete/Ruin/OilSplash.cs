using UnityEngine;

public class OilSplash : Debuff
{
    public override CardName Name => CardName.OilSplash;

    public override int Use(Target target)
    {
        // 1. 적의 현재 화상 수치
        int currentBurn = target.Status.Burn.Count;

        // 2. 화상이 걸려있다면
        if (currentBurn > 0)
        {
            // 3. 현재 수치의 2배로
            target.Burn(currentBurn * 2);            
        }

        return cost;
    }
}