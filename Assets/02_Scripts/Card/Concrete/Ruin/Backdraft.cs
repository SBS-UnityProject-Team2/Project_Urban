using System.Threading;
using UnityEngine;

public class Backdraft : Debuff
{   
    [SerializeField] private int damage;
    [SerializeField] private int burn;
    public override CardName Name => CardName.Backdraft;

    public override int Use(Player player, Target target)
    {   
        //target.Damage(target, damage);
        // 전체데미지
        target.Burn(burn);  // 적 대상 전체에 화상부여 필요
        return curCost;
    }
}