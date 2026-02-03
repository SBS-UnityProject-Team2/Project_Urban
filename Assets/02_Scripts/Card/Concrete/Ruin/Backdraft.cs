using UnityEngine;
using System.Collections;

public class Backdraft : Attack
{
    [SerializeField] private int burnCount = 5;   // 전체공격이후 부여할 화상 수치
    [SerializeField] GameObject HandBackdraft;

    public override CardName Name => CardName.Backdraft;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {        
        // 타겟의 Burn 수만큼 추가 반복
        int repeatCount = target.Status.Burn.Count;
        Transform handSpawnPoint = GameObject.Find("Panel_MachineArm").transform;
        GameObject handEffect = Instantiate(HandBackdraft, handSpawnPoint.position, handSpawnPoint.rotation);

        for (int i = 0; i < repeatCount + 1; i++)
        {
            yield return PlayEffect(target);
            EnemyManager.Instance.DamageAll(damage, Element.Ruin);
        }

        // 자신과 타겟에게 화상 부여
        user.Burn(burnCount);
        target.Burn(burnCount);
        Destroy(handEffect);
    }
}