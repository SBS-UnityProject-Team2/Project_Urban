using UnityEngine;

public class Reforge : Defense 
{    
    [SerializeField] private int turn;
    public override CardName Name => CardName.Reforge;
    public override int Use(Target target)
    {
        target.Protect(armor);
        // 자신에게 부여된 화상 1당 방어1 추가부여

        target.Refined(turn);
        

        return cost;
    }
}