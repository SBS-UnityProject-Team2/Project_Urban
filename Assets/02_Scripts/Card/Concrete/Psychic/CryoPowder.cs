using UnityEngine;
using System.Collections.Generic; 

public class CryoPowder : Debuff
{
    [SerializeField] private int turn = 1; // 빙결 지속 턴 

    public override CardName Name => CardName.CryoPowder;

    public override int Use(Player player, Target target)
    {
        // EnemyManager에서 현재 살아있는 모든 적 리스트 받아옴
        List<Enemy> enemies = EnemyManager.Instance.EnemyList;

        if (enemies != null)
        {
            foreach (Enemy enemy in enemies)
            {
                // 적이 죽어있거나 비활성화 상태가 아닐 때만 적용
                if (enemy.gameObject.activeSelf)
                {
                    enemy.Frozen(turn);
                }
            }
        }
        return curCost;
    }
}