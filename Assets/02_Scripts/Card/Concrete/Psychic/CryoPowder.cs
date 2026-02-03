using System.Collections;
using UnityEngine;
using System.Collections.Generic; 

public class CryoPowder : Debuff
{
    [SerializeField] GameObject HandCryoPowder;
    [SerializeField] private int turn = 1;

    public override CardName Name => CardName.CryoPowder;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        Transform handSpawnPoint = GameObject.Find("Panel_MachineArm").transform;
        GameObject handEffect = Instantiate(HandCryoPowder, handSpawnPoint.position, handSpawnPoint.rotation);
        yield return PlayEffect(target);
        EnemyManager.Instance.ApplyAll(enemy => enemy.Frozen(turn));
        
        Destroy(handEffect);
    }
}