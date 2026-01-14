using UnityEngine;
using System.Collections.Generic; 

public class EnfeebleSludge : Debuff
{
    [SerializeField] private int turn = 1;
    public override CardName Name => CardName.EnfeebleSludge;

    public override int Use(Player player, Target target)
    {
        // 1. EnemyManager에서 현재 살아있는 모든 적 리스트 받아옴
        List<Enemy> enemies = EnemyManager.Instance.EnemyList;

        if (enemies != null)
        {
            // 2. 반복문을 돌며 모든 적에게 약화 디버프 적용
            foreach (Enemy enemy in enemies)
            {
                // 적이 죽어있거나 비활성화 상태가 아닐 때만 적용
                if (enemy.gameObject.activeSelf)
                {
                    enemy.Weaken(turn);
                }
            }
        }
        return cost;
    }
}