using UnityEngine;

public class MoltenArms : Attack 
{
    [SerializeField] private new int damage;    // 적에게 줄 피해량
    [SerializeField] private int selfburn;   // 나에게 부여할 화상 수치

    public override CardName Name => CardName.MoltenArms;

    public override int Use(Player player, Target target)
    {    
        // 2. 나에게 화상 디버프 걸기
        target.Burn(selfburn);
        

        return curCost;
    }
}