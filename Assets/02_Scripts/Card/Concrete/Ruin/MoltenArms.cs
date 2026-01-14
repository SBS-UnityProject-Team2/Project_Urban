using UnityEngine;

public class MoltenArms : Attack 
{
    [SerializeField] private int burn;

    public override CardName Name => CardName.MoltenArms;

    public override int Use(Player player, Target target)
    {   
        // 1. 적에게 데미지 적용
        target.Damage(player, damage, Element.Ruin);

        // 2. 자신(Player)에게 화상 디버프 적용

        return curCost;
    }
}