using UnityEngine;

public class Reforge : Defense 
{
    [SerializeField] private int turn;           // 정련됨 버프 지속 시간

    public override CardName Name => CardName.Reforge;

    public override int Use(Player player, Target target)
    {
        // 1. 현재 자신의 화상 수치 가져오기
        int currentBurn = player.Status.Burn.Count;

        // 2. 총 방어도 계산 
        int totalShield = armor + currentBurn;

        // 3. 방어도 적용
        target.Armor(totalShield);

        // 4. 정련됨 버프 
        target.Refined(turn);

        return curCost;
    }
}