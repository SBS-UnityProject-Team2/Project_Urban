using UnityEngine;
using System.Collections.Generic;

public class Backdraft : Attack
{
    [SerializeField] private int burncount = 5;   // 전체공격이후 부여할 화상 수치

    public override CardName Name => CardName.Backdraft;

    public override int Use(Player player, Target target)
    {
        // 1. 공격자 확인
        Player user = player as Player;

        // 2. 반복 횟수 결정 (지정 타겟의 화상 스택 수치)
        int repetitionCount = target.Status.Burn.Count;

        // 3. 전투 중인 모든 적 가져오기
        List<Enemy> enemies = EnemyManager.Instance.EnemyList; 

        // 4. 화상 수치만큼 전체 공격 반복
        if (repetitionCount > 0)
        {
            for (int i = 0; i < repetitionCount; i++)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.Health.CurrentHp > 0)
                    {
                        enemy.Damage(user, damage, Element.Ruin);
                    }
                }
            }
        }
        
        // 5-1. 나 자신(플레이어)에게 화상 부여
        user.Burn(burncount);

        // 5-2. 적 전체에게 화상 부여
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Health.CurrentHp > 0)
            {
                enemy.Burn(burncount);
            }
        }

        return curCost;
    }
}