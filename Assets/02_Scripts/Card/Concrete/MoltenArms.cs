using UnityEngine;

public class MoltenArms : Attack 
{
    [SerializeField] private new int damage = 12;    // 적에게 줄 피해량
    [SerializeField] private int selfBurn = 3;   // 나에게 부여할 화상 수치

    public override CardName Name => CardName.MoltenArms;

    public override int Use(Target target)
    {
        // 1. 적에게 데미지 주기        

        // 2. 나에게 화상 디버프 걸기
        

        return cost;
    }
}