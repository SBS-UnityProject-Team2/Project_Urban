using System.Threading;
using UnityEngine;

public class Inferno : Debuff
{   
    public override CardName Name => CardName.Inferno;

    public override int Use(Target target)
    {
        // 1. 소멸카드 1장당 적에게 데미지 8       
        
        return cost;
    }
}