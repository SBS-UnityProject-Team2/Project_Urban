using System.Collections;
using TMPro;
using UnityEngine;

public class AbsorbingStrike : Attack
{   
    [SerializeField] GameObject HandAbsorbingStrike;
    public override CardName Name => CardName.AbsorbingStrike;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int hpBefore = target.Health.CurrentHp;
        Transform handSpawnPoint = GameObject.Find("Panel_MachineArm").transform;
        GameObject handEffect = Instantiate(HandAbsorbingStrike, handSpawnPoint.position, handSpawnPoint.rotation);
        
        yield return PlayEffect(target);
        Destroy(handEffect);
        
        target.Damage(user, damage, Element.Bio);
        int hpAfter = target.Health.CurrentHp;
        int actualDamageDealt = hpBefore - hpAfter;

        if (actualDamageDealt > 0)
        {
            user.Heal(actualDamageDealt);
        }
    }
}