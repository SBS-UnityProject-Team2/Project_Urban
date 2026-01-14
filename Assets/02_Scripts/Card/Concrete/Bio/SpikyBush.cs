using UnityEngine;

public class SpikyBush : Defense
{       
    [SerializeField] private int turn;      // 생체반응장갑 지정턴
    [SerializeField] private int count;     // 가시 버프 반사뎀
    public override CardName Name => CardName.SpikyBush;

    public override int Use(Player player, Target target)
    {   
        target.Protect(armor);
        target.BioActiveShell(turn);
        target.Spike(count);

        return cost;
    }
}