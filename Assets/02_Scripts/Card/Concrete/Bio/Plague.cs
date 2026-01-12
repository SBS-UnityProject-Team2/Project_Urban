using UnityEngine;

public class Plague : Attack
{
    public override CardName Name => CardName.Plague;

    public override int Use(Target target)
    {   
        // 3장까지 버리고인데 이러면 1~3장 중 내가 결정한대로 버리는 매수가 정해지는지?
        return cost;
    }
}